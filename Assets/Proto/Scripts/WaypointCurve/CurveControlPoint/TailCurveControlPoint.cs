// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace SlickBowShooting{
// 	public class TailCurveControlPoint : SingleHandleCurveControlPoint{
// 		public override Transform GetForeHandle(){
// 			// return handle;
// 			return transform.GetChild(0);
// 		}
// 		public override Transform GetBackHandle(){
// 			return null;
// 		}
// 		protected override void InitializeHandles(){
// 			// handle = this.transform.GetChild(0);
// 		}
// 		protected override Transform GetHandle(){
// 			return GetForeHandle();
// 		}
// 		protected override void LockTransform(){
// 			base.LockTransform();
// 			LockPositionAtOrigin();
// 		}
// 		protected void LockPositionAtOrigin(){
// 			this.transform.localPosition = Vector3.zero;
// 		}
// 		protected override void LockRotation(){
// 			this.transform.localRotation = Quaternion.identity;
// 		}
// 	}	
// }
