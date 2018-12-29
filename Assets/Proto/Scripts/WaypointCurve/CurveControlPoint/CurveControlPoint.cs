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
	public class CurveControlPoint: MonoBehaviour, ICurveControlPoint{
		public virtual Transform GetForeHandle(){
			if(transform.childCount > 0)
				return transform.GetChild(0).transform;
			else
				return this.transform;
		}
		public virtual Transform GetBackHandle(){
			int childCount = transform.childCount;
			if(childCount > 0){
				if(childCount > 1)
					return transform.GetChild(1);
				else
					return transform.GetChild(0);
			}else
				return this.transform;

		}

		public void Update(){
			ApplyConstraints();
		}
		protected void LockHandle(Transform handle){
			float originalZ = handle.transform.localPosition.z;
			handle.localPosition = new Vector3(0f, 0f, originalZ);
			handle.localRotation = Quaternion.identity;
		}
		void ApplyConstraints(){
			/* to do  */
			Transform foreHandle = GetForeHandle();
			if(foreHandle != this.transform)
				LockHandle(foreHandle);
			Transform backHandle = GetBackHandle();
			if(backHandle != this.transform)
				LockHandle(backHandle);
			
			LockRotation();
			LockPosition();
		}
		void LockRotation(){
			Vector3 originalLocalEulerAngles = this.transform.localEulerAngles;
			float newEulerAngleX = locksRotationX ? 0f : originalLocalEulerAngles.x;
			float newEulerAngleY = locksRotationY ? 0f : originalLocalEulerAngles.y;
			float newEulerAngleZ = locksRotationZ ? 0f : originalLocalEulerAngles.z;
			this.transform.localEulerAngles = new Vector3(
				newEulerAngleX,
				newEulerAngleY,
				newEulerAngleZ
			);

		}
		public bool locksRotationX = false;
		public bool locksRotationY = false;
		public bool locksRotationZ = false;

		void LockPosition(){
			if(locksPosition){
				this.transform.localPosition = Vector3.zero;
			}
		}
		public bool locksPosition = false;
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
}
