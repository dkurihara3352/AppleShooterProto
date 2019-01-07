using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IShootingTargetCriticalHitDetector: IArrowHitDetector{
		void SetShootingTarget(IShootingTarget target);
	}
	public class ShootingTargetCriticalHitDetector : ArrowHitDetector, IShootingTargetCriticalHitDetector {

		public ShootingTargetCriticalHitDetector(IConstArg arg): base(arg){}
		IShootingTarget thisShootingTarget;
		public void SetShootingTarget(IShootingTarget target){
			thisShootingTarget = target;
		}
		public override void Hit(IArrow arrow){
			thisShootingTarget.Hit(arrow, true);
		}
		public override bool ShouldSpawnLandedArrow(){
			return thisShootingTarget.IsActivated();
		}
	}
}
