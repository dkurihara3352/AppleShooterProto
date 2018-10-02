using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public class HeadCurveControlPoint : SingleHandleCurveControlPoint{
		protected override void InitializeHandles(){
			handle = this.transform.GetChild(0);
			// handle.localPosition = new Vector3(0f, 0f, -initialHandleLength);
		}
		public override Transform GetForeHandle(){
			return null;
		}
		public override Transform GetBackHandle(){
			return handle;
		}
	}
}
