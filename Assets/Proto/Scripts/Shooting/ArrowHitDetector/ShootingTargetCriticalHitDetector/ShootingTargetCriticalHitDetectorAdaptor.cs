using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IShootingTargetCriticalHitDetectorAdaptor: IArrowHitDetectorAdaptor{
		IShootingTargetCriticalHitDetector GetShootingTargetCriticalHitDetector();
	}
	public class ShootingTargetCriticalHitDetectorAdaptor : ArrowHitDetectorAdaptor, IShootingTargetCriticalHitDetectorAdaptor {

		public IShootingTargetCriticalHitDetector GetShootingTargetCriticalHitDetector(){
			return (IShootingTargetCriticalHitDetector)thisDetector;
		}
		protected override IArrowHitDetector CreateArrowHitDetector(){
			ShootingTargetCriticalHitDetector.IConstArg arg = new ShootingTargetCriticalHitDetector.ConstArg(
				this
			);
			return new ShootingTargetCriticalHitDetector(arg);
		}
	}
}
