using UnityEngine;
using System.Collections;

namespace UCC {
	public class LookAtTargetBehaviour : CameraBehaviour {
		public override void Compute ()
		{
			base.Compute ();
			Vector3 dif = mParentCameraMode.ParentCamController.targetTransform.position - mParentCameraMode.TargetCamPosition;
			if (dif.sqrMagnitude > 0.001f) {
				TargetRotation = Quaternion.LookRotation (dif);
			} else {
				TargetRotation = mParentCameraMode.CamRotation;
			}
		}
	}
}