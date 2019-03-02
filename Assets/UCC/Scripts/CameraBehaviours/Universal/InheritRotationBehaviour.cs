using UnityEngine;
using System.Collections;

namespace UCC {
	public class InheritRotationBehaviour : CameraBehaviour {
		public override void Compute ()
		{
			base.Compute ();
			TargetRotation = mParentCameraMode.ParentCamController.targetTransform.transform.rotation;
		}
	}
}
