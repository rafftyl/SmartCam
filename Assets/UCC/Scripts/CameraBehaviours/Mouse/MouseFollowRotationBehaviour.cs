using UnityEngine;
using System.Collections;

namespace UCC {
	public class MouseFollowRotationBehaviour : CameraBehaviour {
		public float sensitivity = 1;
		public bool mouseRotateVertical = true;
		public bool mouseRotateHorizontal  = true;
		public float minVerticalAngle = 0;
		public float maxVerticalAngle = 45;
		public float minHorizontalAngle = 0;
		public float maxHorizontalAngle = 360;
		public bool horizontalReturn = true;
		public float horizontalReturnTime = 2;
		public bool invertYAxis = true;

		float horizontalReturnTimer  = 0;
		Vector3 angles;
		public override void Compute ()
		{
			base.Compute ();
			if (mouseRotateVertical) {
				float mouseY = Input.GetAxis ("Mouse Y") * (invertYAxis ? -1 : 1);
				angles.x += mouseY * sensitivity;
				angles.x = Mathf.Clamp (angles.x, minVerticalAngle, maxVerticalAngle);
				TargetEulerAngles = mParentCameraMode.ParentCamController.targetTransform.rotation.eulerAngles + angles;

			}
			if (mouseRotateHorizontal) {
				float mouseX = Input.GetAxis("Mouse X");
				horizontalReturnTimer += Time.deltaTime;
				if(mouseX > 0.02f) {
					angles.y += mouseX * sensitivity;
					if(angles.y  > 360) {
						angles.y = 0;
					}
					if(angles.y < 0) {
						angles.y = 360;
					}
					horizontalReturnTimer = 0;
				}
				
				if(horizontalReturnTimer > horizontalReturnTime) {
					angles.y = 0;
				}
			}
			if(!mouseRotateVertical && !mouseRotateHorizontal) {
				TargetEulerAngles = mParentCameraMode.ParentCamController.targetTransform.rotation.eulerAngles;
			}
		}
	}
}
