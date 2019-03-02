using UnityEngine;
using System.Collections;

namespace UCC {
	public class AvoidObstaclesBehaviour : CameraBehaviour {
		public LayerMask collisionMask;
		public float separationDistance;
		public override void Init (CameraMode camMode)
		{
			base.Init (camMode);
		}

		public override void Compute ()
		{
			base.Compute ();
			RaycastHit hit;
			Vector3 posDif = mParentCameraMode.TargetCamPosition - mParentCameraMode.PivotPos;
			if (Physics.Raycast (mParentCameraMode.PivotPos, posDif, out hit, posDif.magnitude, collisionMask)) {
				Vector3 relativePos = hit.point - mParentCameraMode.PivotPos;
				TargetDistanceFromPivot = relativePos.magnitude - separationDistance;
			}
		}
	}
}
