using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IShootingObstacleAdaptor: IArrowHitDetectorAdaptor{
		IShootingObstacle GetShootingObstacle();
	}
	public class ShootingObstacleAdaptor : ArrowHitDetectorAdaptor, IShootingObstacleAdaptor {
		public IShootingObstacle GetShootingObstacle(){
			return (IShootingObstacle)thisDetector;
		}
		protected override IArrowHitDetector CreateArrowHitDetector(){
			ShootingObstacle.IConstArg arg = new ShootingObstacle.ConstArg(
				this
			);
			return new ShootingObstacle(arg);
		}
	}
}
