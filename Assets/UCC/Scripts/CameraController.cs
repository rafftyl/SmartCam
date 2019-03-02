using UnityEngine;
using System.Collections;

namespace UCC {
	[RequireComponent(typeof(Camera))]
	public class CameraController : MonoBehaviour {
		protected int mCurrentCameraModeIndex;
		public Camera Cam { get; protected set;}
	    public CameraMode[] cameraModes;
	    public CameraMode CurrentCameraMode {
			get { return cameraModes [mCurrentCameraModeIndex];}
		}

	    public Transform targetTransform;
	    public void SetCameraMode(int index) {
			bool smooth = CurrentCameraMode.smoothTransitionToNextMode;
			CurrentCameraMode.Disable ();
	        if (index > cameraModes.Length - 1 || index < 0 || cameraModes[index] == null) {
				Debug.LogError("No camera mode with index " + index.ToString());
				return;
			}
			mCurrentCameraModeIndex = index;
			if (!smooth) {
				CurrentCameraMode.Prepare ();
			}
	    }

	    public void SetNextCameraMode() {
			int nextInd = mCurrentCameraModeIndex + 1;
			if (nextInd > cameraModes.Length - 1) {
				nextInd =  0;
			}
			SetCameraMode (nextInd);
	    }

		public void SetPrevCameraMode() {
			int prevInd = mCurrentCameraModeIndex  - 1;
			if (prevInd < 0) {
				prevInd = cameraModes.Length -1;
			}
			SetCameraMode (prevInd);
		}

		public void SetPivotTransform(Transform pivotTransform) {
			this.targetTransform = pivotTransform;
		}

		void Awake() {
			Cam = GetComponent<Camera> ();
			for (int i = 0; i < cameraModes.Length; ++i) {
				cameraModes[i].Init(this);
				if(i != 0) {
					cameraModes[i].Disable();
				}
			}

			mCurrentCameraModeIndex = 0;

		}

		void LateUpdate() {
			if (Input.GetButtonDown("Jump")) {//TODO: wywalić
				SetNextCameraMode();
			}
			if(mCurrentCameraModeIndex < cameraModes.Length)
				CurrentCameraMode.SetCameraParams ();
			transform.position = CurrentCameraMode.CamPosition;
			transform.rotation = CurrentCameraMode.CamRotation;
			Cam.orthographicSize = CurrentCameraMode.OrthoSize;
			Cam.fieldOfView = CurrentCameraMode.FieldOfView;

			if (CurrentCameraMode.smoothTransitionToNextMode) {
				for (int i = 0; i < cameraModes.Length; ++i) {
					if(i != mCurrentCameraModeIndex) {
						cameraModes[i].CopyCamParams(CurrentCameraMode);
					}
				}
			}
		}
	}
}