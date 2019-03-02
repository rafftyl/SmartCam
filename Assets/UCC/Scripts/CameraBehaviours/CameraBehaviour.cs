using UnityEngine;
using System.Collections;

namespace UCC {
	[RequireComponent(typeof(CameraMode))]
	public abstract class CameraBehaviour : MonoBehaviour {
		protected CameraMode mParentCameraMode;

		public bool HasEulers { get; private set; }
		private Vector3 mTargetEulerAngles;
		public bool HasPivotPos { get; private set; }
		private Vector3 mTargetPivotPosition;
		public bool HasOrthoSize { get; private set; }
		private float mTargetOrthoSize;
		public bool HasDistance { get; private set; }
		private float mTargetDistanceFromPivot;
		public bool HasRotation { get; private set; }
		private Quaternion mTargetRotation;
		public bool HasFieldOfView{get; private set;}
		private float mTargetFieldOfView;

		private bool mIsActive;
		public bool IsActive {
			get{
				return mIsActive;
			}
			set {
				mIsActive = false;
				Disable();
			}
		}
		public Vector3 TargetEulerAngles {
			get { return mTargetEulerAngles;}
			protected set {
				mTargetEulerAngles = value;
				HasEulers = true;
			}
		}

		public Vector3 TargetPivotPosition {
			get { return mTargetPivotPosition;}
			protected set {
				mTargetPivotPosition = value;
				HasPivotPos = true;
			}
		}

		public float TargetOrthoSize {
			get { return mTargetOrthoSize;}
			protected set {
				mTargetOrthoSize = value;
				HasOrthoSize = true;
			}
		}

		public float TargetDistanceFromPivot {
			get { return mTargetDistanceFromPivot;}
			protected set {
				mTargetDistanceFromPivot = value;
				HasDistance = true;			
			}
		}

		public Quaternion TargetRotation {
			get { return mTargetRotation;}
			protected set {
				mTargetRotation = value;
				HasRotation = true;
			}
		}

		public float TargetFieldOfView {
			get { return mTargetFieldOfView;}
			protected set {
				mTargetFieldOfView = value;
				HasFieldOfView = true;
			}
		}

		public virtual void Compute () {
			//CheckTimer ();
			HasEulers = false;
			HasPivotPos = false;	
			HasOrthoSize = false;
			HasDistance = false;
			HasRotation = false;
			HasFieldOfView = false;
		}

	    public virtual void Init(CameraMode camMode) {
			mIsActive = true;
			mParentCameraMode = camMode;
	    }

		public virtual void Disable() {
		}

	}
}