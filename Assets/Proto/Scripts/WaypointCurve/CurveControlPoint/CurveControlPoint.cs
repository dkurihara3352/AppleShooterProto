using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ICurveControlPoint{
		void SetWaypointCurveAdaptor(IWaypointCurveAdaptor adaptor);
		Transform GetForeHandle();
		Transform GetBackHandle();
		Vector3 GetPosition();
		Quaternion GetRotation();
		Vector3 GetUpDirection();
	}
	[ExecuteInEditMode]
	public abstract class AbsCurveControlPoint: MonoBehaviour, ICurveControlPoint{
		protected float initialHandleLength = 3f;
		public abstract Transform GetForeHandle();
		public abstract Transform GetBackHandle();
		protected IWaypointCurveAdaptor thisWaypointCurveAdaptor;
		public void SetWaypointCurveAdaptor(IWaypointCurveAdaptor adaptor){
			thisWaypointCurveAdaptor = adaptor;
		}

		// #if UNITY_EDITOR
		// #endif
		//these shits doesn't work
			void Awake(){
				InitializeHandles();
			}
			public void OnDrawGizmos(){
				DrawHandles();
				DrawSelf();
			}
			public void Update(){
				if(!UnityEditor.EditorApplication.isPlaying){
					LockTransform();
					if(thisWaypointCurveAdaptor != null)
						thisWaypointCurveAdaptor.UpdateCurve();
				}
			}

		protected abstract void InitializeHandles();
		protected abstract void DrawHandles();
		public Color pointColor;
		void DrawSelf(){
			Gizmos.color = pointColor;
			Gizmos.DrawCube(this.transform.position, Vector3.one * .5f);
		}
		protected abstract void LockTransform();
		protected void LockHandle(Transform handle){
			float originalZ = handle.transform.localPosition.z;
			handle.localPosition = new Vector3(0f, 0f, originalZ);
			handle.localRotation = Quaternion.identity;
		}
		protected void LockRotationOnXZAxis(){
			float originalLocalEulerAngleY = this.transform.localEulerAngles.y;
			this.transform.localEulerAngles = new Vector3(0f, originalLocalEulerAngleY, 0f);
		}
		public Vector3 GetPosition(){
			return transform.position;
		}
		public Quaternion GetRotation(){
			return transform.rotation;
		}
		public Vector3 GetUpDirection(){
			return transform.up;
		}
	}
	public abstract class SingleHandleCurveControlPoint: AbsCurveControlPoint{
		protected Transform handle;
		protected override void DrawHandles(){
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(
				this.transform.position,
				handle.position
			);
		}
		protected override void LockTransform(){
			LockHandle(handle);
			LockRotation();
		}
		protected virtual void LockRotation(){
			LockRotationOnXZAxis();
		}
	}
}
