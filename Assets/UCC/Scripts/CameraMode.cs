using UnityEngine;
using System.Collections;

namespace UCC {
	public class CameraMode : MonoBehaviour {
		public bool smoothTransitionToNextMode = false;

		public bool smoothDistanceTransition = true;
		public float maxDistanceTransitionSpeed = 10;

		public bool smoothFieldOfViewTransition = true;
		public float maxFieldOfViewChangeSpeed = 30;

		public bool smoothOrthoSizeTransition = true;
		public float maxOrthoSizeChangeSpeed = 10;

		public bool smoothPivotPosTransition = false;
		public float maxPivotTransitionSpeed = 10;

		public bool smoothAngleTransition  = false;
		public float maxAngleChangeSpeed = 90;

	   	protected CameraBehaviour[] mCameraBehaviours;
		public CameraController ParentCamController{ get; private set; } 

		public Vector3 TargetEulers{ get; protected set;}
		public Vector3 Eulers { get; protected set; }

		public Vector3 TargetPivotPos{ get; protected set;}
		public Vector3 PivotPos{ get; protected set; }

		public float TargetOrthoSize{ get; protected set;}
		public float OrthoSize { get; protected set; }

		public float TargetFieldOfView{ get; protected set;}
		public float FieldOfView{ get; protected set; }

		public float TargetDistFromPivot{ get; protected set;}
		public float DistFromPivot{ get; protected set; }

		public Vector3 TargetCamPosition{ get; protected set; }
		public Quaternion TargetCamRotation { get; protected set; }

		public Quaternion CamRotation { get; protected set; }
		public Vector3 CamPosition { get; protected set; }



		void UpdateTargetPos() {
			TargetCamPosition = TargetPivotPos + 
				Quaternion.Euler (TargetEulers) * (-TargetDistFromPivot * Vector3.forward);		
		}

		void UpdatePos() {
			CamPosition = PivotPos + 
				Quaternion.Euler (Eulers) * (-DistFromPivot * Vector3.forward);	
		}

		public void CopyCamParams(CameraMode other) {
			Eulers = other.TargetEulers;
			PivotPos = other.TargetPivotPos;
			DistFromPivot = other.TargetDistFromPivot;
			CamRotation = other.CamRotation;
			OrthoSize = other.TargetOrthoSize;
			FieldOfView = other.TargetFieldOfView;
		}

		Vector2 eulers;
		Vector2 target;
	    public void SetCameraParams() {
			for(int i = 0; i < mCameraBehaviours.Length; ++i) {
				CameraBehaviour b = mCameraBehaviours[i];
				if(b.IsActive) {
					b.Compute();
					if(b.HasEulers) {
						target  = b.TargetEulerAngles;
						target.x  = target.x % 360;
						if(target.x < 0) {
							target.x += 360;
						}

						target.y = target.y % 360;
						if(target.y < 0) { 
							target.y += 360;
						} 
						TargetEulers = target;
					}

					if(b.HasPivotPos) {
						TargetPivotPos = b.TargetPivotPosition;
					}

					if(b.HasDistance) {
						TargetDistFromPivot = b.TargetDistanceFromPivot;
					}

					if(b.HasRotation) {
						TargetCamRotation = b.TargetRotation;
					}

					if(b.HasOrthoSize) {
						TargetOrthoSize = b.TargetOrthoSize;
					}

					if(b.HasFieldOfView) {
						TargetFieldOfView = b.TargetFieldOfView;
					}

					UpdateTargetPos();
				}
			}
			PivotPos = smoothPivotPosTransition ? Vector3.MoveTowards (PivotPos, TargetPivotPos, maxPivotTransitionSpeed * Time.deltaTime) : TargetPivotPos;
			DistFromPivot = smoothDistanceTransition ? Mathf.MoveTowards (DistFromPivot, TargetDistFromPivot, maxDistanceTransitionSpeed * Time.deltaTime) : TargetDistFromPivot;
			OrthoSize = smoothOrthoSizeTransition ? Mathf.MoveTowards (OrthoSize, TargetOrthoSize, maxOrthoSizeChangeSpeed * Time.deltaTime) : TargetOrthoSize;
			FieldOfView = smoothFieldOfViewTransition ? Mathf.MoveTowards (FieldOfView, TargetFieldOfView, maxFieldOfViewChangeSpeed * Time.deltaTime) : TargetFieldOfView;

			//Vector2 eulers;
			if (smoothAngleTransition) {
				if(Mathf.Abs(Eulers.x - TargetEulers.x) > 180) {
					int sign = Eulers.x > 180 ? 1:-1;
					eulers.x = Mathf.MoveTowards(Eulers.x, TargetEulers.x + sign * 360, maxAngleChangeSpeed * Time.deltaTime);
				} else {
					eulers.x = Mathf.MoveTowards(Eulers.x, TargetEulers.x, maxAngleChangeSpeed * Time.deltaTime);
				}
				if(Mathf.Abs(Eulers.y - TargetEulers.y) > 180) {
					int sign = Eulers.y > 180 ? 1:-1;
					eulers.y = Mathf.MoveTowards(Eulers.y, TargetEulers.y + sign * 360, maxAngleChangeSpeed * Time.deltaTime);
				} else {
					eulers.y = Mathf.MoveTowards(Eulers.y, TargetEulers.y, maxAngleChangeSpeed * Time.deltaTime);
				}

				CamRotation = Quaternion.RotateTowards(CamRotation, TargetCamRotation, maxAngleChangeSpeed * Time.deltaTime);
			} else {
				eulers = TargetEulers;
				CamRotation = TargetCamRotation;
			}

			eulers.x  = eulers.x % 360;
			if(eulers.x < 0) {
				eulers.x += 360;
			} 

			eulers.y = eulers.y % 360;
			if(eulers.y < 0) { 
				eulers.y += 360;
			}

			Eulers = eulers;

			UpdatePos ();
	    }

	    public T GetBehaviour<T>() where T: CameraBehaviour {
			for (int i =0; i < mCameraBehaviours.Length; ++i) {
				if(mCameraBehaviours[i] is T) {
					return mCameraBehaviours as T;
				}
			}
	        return null;
	    }

	    public void Init(CameraController camController) {
			ParentCamController = camController;
			mCameraBehaviours = GetComponents<CameraBehaviour> ();
			TargetFieldOfView = camController.Cam.fieldOfView;
			TargetOrthoSize = camController.Cam.orthographicSize;
			for (int i =0; i < mCameraBehaviours.Length; ++i) {
				mCameraBehaviours[i].Init(this);
			}
			Prepare ();
	    }

		public void Prepare() {
			bool smoothDistance = smoothDistanceTransition;
			bool smoothAngles = smoothAngleTransition;
			bool smoothPivot = smoothPivotPosTransition;
			bool smoothFov = smoothFieldOfViewTransition;
			bool smoothOrtho = smoothOrthoSizeTransition;
			
			smoothDistanceTransition = false;
			smoothAngleTransition = false;
			smoothPivotPosTransition = false;
			smoothFieldOfViewTransition = false;
			smoothOrthoSizeTransition = false;
			
			SetCameraParams ();
			
			smoothDistanceTransition = smoothDistance;
			smoothAngleTransition = smoothAngles;
			smoothPivotPosTransition = smoothPivot;
			smoothFieldOfViewTransition = smoothFov;
			smoothOrthoSizeTransition = smoothOrtho;
		}

		public void Disable() {
			for(int i = 0; i < mCameraBehaviours.Length; ++i) {
				mCameraBehaviours[i].Disable();
			}
		}

	}
}