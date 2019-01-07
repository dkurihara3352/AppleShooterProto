using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IShootingTargetNormalHitDetectorAdaptor: IArrowHitDetectorAdaptor{
		IShootingTargetNormalHitDetector GetShootingTargetNormalHitDetector();
	}
	public class ShootingTargetNormalHitDetectorAdaptor : ArrowHitDetectorAdaptor, IShootingTargetNormalHitDetectorAdaptor {

		public IShootingTargetNormalHitDetector GetShootingTargetNormalHitDetector(){
			return (IShootingTargetNormalHitDetector)thisDetector;
		}
		protected override IArrowHitDetector CreateArrowHitDetector(){
			ShootingTargetNormalHitDetector.IConstArg arg = new ShootingTargetNormalHitDetector.ConstArg(
				this
			);
			return new ShootingTargetNormalHitDetector(arg);
		}
	}
}
