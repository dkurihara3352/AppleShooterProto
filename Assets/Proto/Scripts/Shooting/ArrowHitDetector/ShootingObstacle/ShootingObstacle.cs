using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IShootingObstacle: IArrowHitDetector{}
	public class ShootingObstacle : ArrowHitDetector, IShootingObstacle {

		public ShootingObstacle(IConstArg arg): base(arg){}
		public override void Hit(IArrow arrow){
			return;
		}
	}
}
