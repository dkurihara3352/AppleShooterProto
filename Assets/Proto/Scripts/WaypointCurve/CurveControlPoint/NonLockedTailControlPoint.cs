using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{	
	public class NonLockedTailControlPoint : TailCurveControlPoint {
		protected override void LockTransform(){
			LockHandle(GetHandle());
			LockPositionAtOrigin();
		}
	}
}
