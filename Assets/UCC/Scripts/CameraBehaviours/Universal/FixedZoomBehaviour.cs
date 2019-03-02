using UnityEngine;
using System.Collections;

namespace UCC {
	public class FixedZoomBehaviour : CameraBehaviour {
		public float distanceFromPivot = 5;
		public override void Compute ()
		{
			base.Compute ();
			TargetDistanceFromPivot = distanceFromPivot;
		}
	}
}
