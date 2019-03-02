using UnityEngine;
using System.Collections;
namespace UCC {
	public class HeadBobBehaviour : CameraBehaviour {
		public AnimationCurve curve;
		public float walkCycleLength = 2;

		float mCurrentDistance = 0;
		Vector3 mPreviousPosition;

		public override void Init (CameraMode camMode)
		{
			base.Init (camMode);
			mPreviousPosition = mParentCameraMode.PivotPos;
		}

		public override void Compute ()
		{
			base.Compute ();
			float dist = Vector3.Distance (mPreviousPosition, mParentCameraMode.TargetPivotPos);
			mPreviousPosition = mParentCameraMode.TargetPivotPos;
			mCurrentDistance += dist;
			if (mCurrentDistance > walkCycleLength) {
				mCurrentDistance = 0;
			}
			float yOffset = curve.Evaluate (mCurrentDistance / walkCycleLength);
			TargetPivotPosition = mParentCameraMode.TargetPivotPos + Vector3.up * yOffset;
		}
	}
}
