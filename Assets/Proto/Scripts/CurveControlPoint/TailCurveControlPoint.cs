using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ICurveControlPoint{
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
		#if UNITY_EDITOR
		void Awake(){
			InitializeHandles();
		}
		#endif
		protected abstract void InitializeHandles();
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
		public void OnDrawGizmos(){
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(
				this.transform.position,
				handle.position
			);
		}
		public void Update(){
			LockTransform();
		}
		protected virtual void LockTransform(){
			LockHandle(handle);
			LockRotation();
		}
		protected virtual void LockRotation(){
			LockRotationOnXZAxis();
		}
	}
	
	public class TailCurveControlPoint : SingleHandleCurveControlPoint{
		public override Transform GetForeHandle(){
			return handle;
		}
		public override Transform GetBackHandle(){
			return null;
		}
		protected override void InitializeHandles(){
			handle = this.transform.GetChild(0);
			// handle.localPosition = new Vector3(0f, 0f, initialHandleLength);
		}
		protected override void LockTransform(){
			base.LockTransform();
			LockPositionAtOrigin();
		}
		void LockPositionAtOrigin(){
			this.transform.localPosition = Vector3.zero;
		}
		protected override void LockRotation(){
			this.transform.localRotation = Quaternion.identity;
		}
	}
}
