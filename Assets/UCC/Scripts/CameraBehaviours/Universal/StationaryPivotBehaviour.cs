using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UCC {
	public class StationaryPivotBehaviour : CameraBehaviour {
		public Vector3 position;
		public Quaternion rotation;
		public override void Compute ()
		{
			base.Compute ();
			TargetPivotPosition = position;
			TargetRotation = rotation;
			TargetDistanceFromPivot = 0;
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(UCC.StationaryPivotBehaviour))]
public class StationaryBehaviourEditor : Editor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		UCC.StationaryPivotBehaviour myTarget = target as UCC.StationaryPivotBehaviour;
		if (GUILayout.Button ("Bake Current Position And Rotation")) {
			Camera cam = myTarget.GetComponentInParent<Camera>();
			UCC.CameraController cc = myTarget.GetComponentInParent<UCC.CameraController>();
			if(cam == null) {
				Debug.LogError("No Camera on parent object.");
				return;
			}

			if(cc == null) {
				Debug.LogError("No Camera Controller on parent object.");
				return;
			}

			myTarget.position = cam.transform.position;
			myTarget.rotation = cam.transform.rotation;
			EditorUtility.SetDirty(myTarget);
		}
	}
}
#endif