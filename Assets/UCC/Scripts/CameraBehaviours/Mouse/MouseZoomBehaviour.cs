using UnityEngine;
using System.Collections;

namespace UCC {
	public class MouseZoomBehaviour : CameraBehaviour {
		public float sensitivity = 1;
		public float minCamDist = 2;
		public float maxCamDist = 10;

		public override void Compute ()
		{
			base.Compute ();
			float dist = mParentCameraMode.TargetDistFromPivot;
			dist -= sensitivity * Input.GetAxis ("Mouse ScrollWheel");
			dist = Mathf.Clamp (dist, minCamDist, maxCamDist);
			TargetDistanceFromPivot = dist;
		}
	}
}