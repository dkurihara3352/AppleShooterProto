using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public class TailCurveControlPoint : SingleHandleCurveControlPoint{
		public override Transform GetForeHandle(){
			return handle;
		}
		public override Transform GetBackHandle(){
			return null;
		}
		protected override void InitializeHandles(){
			handle = this.transform.GetChild(0);
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
