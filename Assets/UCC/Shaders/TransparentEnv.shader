// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UCC/TransparentEnv" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[NoScaleOffset] _NormalMap("Normals", 2D) = "bump"{}
		_BumpScale  ("Bump Scale", Range(0,1)) = 0		
		[NoScaleOffset] _Smoothness("Smoothness", 2D) = "black" {} 		
	}
	SubShader {
		Tags { "RenderType"="Opaque"}				
		LOD 200
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0
			
			#include "Lighting.cginc"

			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			sampler2D _NormalMap;		
			sampler2D _Smoothness;
			sampler2D _FadeTex;
			fixed4 _Color;
			fixed _BumpScale;	
			
			float _MinFadeRadius = 0;
			float _MaxFadeRadius = 0;			
			float4 _FocusPointPosition = float4(0,0,0,0);
			float _FadeStrength = 0;
			float _WorldUVScale = 0;
					
			struct VertexInput {
				float4 position : POSITION;
				half2 uv : TEXCOORD0;
				float3 normal: NORMAL;
				half4 tangent: TANGENT;			
			};
			
			struct Interpolators {
				float4 position : SV_POSITION;
				half2 uv : TEXCOORD0;
				half3 normal: TEXCOORD1;
				half4 tangent: TEXCOORD2;
				float3 worldPosition : TEXCOORD3;				
			};
			
			Interpolators vert(VertexInput i) {
				Interpolators o;
				o.position = UnityObjectToClipPos(i.position);
				o.worldPosition = mul(unity_ObjectToWorld, i.position.xyz);
				o.uv = TRANSFORM_TEX(i.uv, _MainTex);
				o.normal = UnityObjectToWorldNormal(i.normal);
				o.tangent = float4(UnityObjectToWorldDir(i.tangent.xyz),i.tangent.w);				
				return o;
			}
			
			fixed4 frag(Interpolators i): SV_TARGET {						
				float3 focusViewDir = normalize(_WorldSpaceCameraPos - _FocusPointPosition.xyz);
				float3 focusToFragment = i.worldPosition - _FocusPointPosition.xyz;	
				float distFromFocusLine = length(focusToFragment - focusViewDir * dot(focusViewDir, focusToFragment));	
				float distCoef = 1 - (distFromFocusLine - _MinFadeRadius)/(_MaxFadeRadius - _MinFadeRadius);			
				distCoef *= _FadeStrength;				
				float viewSign = distCoef < 0 ? 1 : sign(dot(focusViewDir, normalize(focusToFragment)));			
				
				float3 blendWeights = abs(i.normal);
				blendWeights /= (blendWeights.x + blendWeights.y + blendWeights.z);
				
				float3 scaledPos = i.worldPosition * _WorldUVScale;
				float yz = tex2D(_FadeTex,scaledPos.yz).r;
				float xz = tex2D(_FadeTex,scaledPos.xz).r;
				float xy = tex2D(_FadeTex,scaledPos.xy).r;				
				float transparencyBase = yz * blendWeights.x + xz * blendWeights.y + xy * blendWeights.z; 
							
				if(transparencyBase - distCoef * viewSign < 0) {
					discard;
				}
				
				i.uv = i.uv * _MainTex_ST.xy + _MainTex_ST.zw;
				float3 tangentSpaceNormal = UnpackScaleNormal(tex2D(_NormalMap, i.uv), _BumpScale);
				float3 binormal = normalize(cross(i.normal, i.tangent.xyz)* i.tangent.w);
				i.normal = normalize(
					tangentSpaceNormal.x * i.tangent +
					tangentSpaceNormal.y * binormal +
					tangentSpaceNormal.z * i.normal
				); 
				
					
																
				float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPosition);				
				float3 halfVector = normalize(viewDir + _WorldSpaceLightPos0.xyz);
				fixed specularColor = dot(i.normal, halfVector);
				fixed diffuseColor = dot(i.normal, _WorldSpaceLightPos0.xyz);
				fixed smoothness = tex2D(_Smoothness, i.uv).r;
				fixed4 col = tex2D(_MainTex, i.uv) * _Color * _LightColor0 * (smoothness * specularColor + (1 - smoothness) * diffuseColor);
							
				return col;
			}
			ENDCG
		}
	} 
	//FallBack "Diffuse"
}
