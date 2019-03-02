using UnityEngine;
using System.Collections;

namespace UCC {
	public class TransparencyBehaviour : CameraBehaviour {
		[Range(0,1)]
		public float fadeStrength = 1;
		public float minFadeRadius = 2;
		public float maxFadeRadius = 8;
		public float worldUVScale = 0.4f;
		public Texture fadeTex;

		public override void Init (CameraMode camMode)
		{
			base.Init (camMode);
			Disable ();
		}

		public override void Compute ()
		{
			base.Compute ();
			Vector3 pos = mParentCameraMode.ParentCamController.targetTransform.position;
			Shader.SetGlobalVector("_FocusPointPosition", new Vector4(pos.x,pos.y,pos.z,1));
			Shader.SetGlobalFloat ("_FadeStrength", fadeStrength);
			Shader.SetGlobalFloat ("_MinFadeRadius", minFadeRadius);
			Shader.SetGlobalFloat ("_MaxFadeRadius", maxFadeRadius);
			Shader.SetGlobalFloat ("_WorldUVScale", worldUVScale);
			Shader.SetGlobalTexture ("_FadeTex", fadeTex);
		}

		public override void Disable ()
		{
			Shader.SetGlobalFloat ("_FadeStrength", 0);
		}
	}
}