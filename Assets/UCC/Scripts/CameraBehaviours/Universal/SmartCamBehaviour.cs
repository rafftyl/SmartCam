using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UCC {
	public class SmartCamBehaviour : CameraBehaviour {
		public struct Padding {
			public float left, right, top, down;
		}
		public List<Transform> transforms;
		public Padding padding;

		public override void Compute ()
		{
			base.Compute ();
			Vector3 meanPos = Vector3.zero;
			for(int i = 0; i < transforms.Count; ++i) {
				meanPos += transforms[i].position;
			}
			meanPos /= transforms.Count;

			TargetPivotPosition = meanPos;
			float fov = mParentCameraMode.FieldOfView;
			float aspect = Screen.width / Screen.height;



		}
	}
}