using UnityEngine;
using System.Collections;

namespace UCC {
	public class MouseRotateBehaviour : CameraBehaviour {
		public float sensitivity = 1;
		public float minVerticalAngle = -45;
		public float maxVerticalAngle = 45;
		public float minHorizontalAngle = 0;
		public float maxHorizontalAngle = 360;

		public bool invertYAxis = true;

		public override void Compute ()
		{
			base.Compute ();
			float mouseX = Input.GetAxis ("Mouse X");
			float mouseY = Input.GetAxis ("Mouse Y") * (invertYAxis ? -1 : 1);
			Vector3 angles = mParentCameraMode.Eulers;
			angles.x += mouseY * sensitivity;
			angles.y += mouseX * sensitivity;
			angles.x = Mathf.Clamp (angles.x, minVerticalAngle, maxVerticalAngle);
			if (angles.y > 360) {
				angles.y = 0;
			} else if (angles.y < 0) {
				angles.y = 360;		
			} else {
				angles.y = Mathf.Clamp(angles.y, minHorizontalAngle, maxHorizontalAngle);
			}
			TargetEulerAngles = angles;
		}
	}
}