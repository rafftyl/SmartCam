using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UCC {
	public class FollowBehaviour : CameraBehaviour {
		public Vector3 cameraPivotOffset;
		public override void Compute ()
		{
			base.Compute ();
			Transform trans = mParentCameraMode.ParentCamController.targetTransform;
			TargetPivotPosition = trans.position + cameraPivotOffset.x * trans.right + cameraPivotOffset.y * trans.up + cameraPivotOffset.z * trans.forward;
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(UCC.FollowBehaviour))]
public class FollowBehaviourEditor : Editor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		UCC.FollowBehaviour myTarget = target as UCC.FollowBehaviour;
		if (GUILayout.Button ("Bake Current Offset")) {
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
			
			if(cc.targetTransform == null) {
				Debug.LogError("Pivot transform of Camera Controller is not assigned.");
				return;
			}
			myTarget.cameraPivotOffset = cc.targetTransform.InverseTransformPoint(cam.transform.position);
			EditorUtility.SetDirty(myTarget);
		}
	}
}
#endif