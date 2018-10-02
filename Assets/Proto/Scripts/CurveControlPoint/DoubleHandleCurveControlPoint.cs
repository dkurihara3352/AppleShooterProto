using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public class DoubleHandleCurveControlPoint : AbsCurveControlPoint, ICurveControlPoint {

		Transform foreHandle;
		public override Transform GetForeHandle(){
			return foreHandle;
		}
		Transform backHandle;
		public override Transform GetBackHandle(){
			return backHandle;
		}

		Transform[] handles;
		protected override void InitializeHandles(){
			Transform[] tempHandles = new Transform[2];
			for(int i = 0; i < 2; i ++){
				tempHandles[i] = transform.GetChild(i);
			}
			foreHandle = tempHandles[0];
			backHandle = tempHandles[1];
			// foreHandle.localPosition = new Vector3(0f, 0f, initialHandleLength);
			// backHandle.localPosition = new Vector3(0f, 0f, -initialHandleLength);
			handles = new Transform[]{foreHandle, backHandle};
		}
		public void OnDrawGizmos(){
			foreach(Transform handle in handles){
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(
					this.transform.position,
					handle.position
				);
			}
		}
		public void Update(){
			LockHandles();
		}
		void LockHandles(){
			foreach(Transform handle in handles){
				LockHandle(handle);
			}
		}
	}
}

