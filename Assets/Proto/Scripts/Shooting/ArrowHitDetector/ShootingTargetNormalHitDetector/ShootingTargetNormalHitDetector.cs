using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetNormalHitDetector: IArrowHitDetector{
		void SetShootingTarget(IShootingTarget target);
	}
	public class ShootingTargetNormalHitDetector : ArrowHitDetector, IShootingTargetNormalHitDetector {

		public ShootingTargetNormalHitDetector(IConstArg arg ): base(arg){}
		IShootingTarget thisShootingTarget;
		public void SetShootingTarget(IShootingTarget target){
			thisShootingTarget = target;
		}
		public override void Hit(IArrow arrow){
			thisShootingTarget.Hit(arrow, false);
		}
	}
}
