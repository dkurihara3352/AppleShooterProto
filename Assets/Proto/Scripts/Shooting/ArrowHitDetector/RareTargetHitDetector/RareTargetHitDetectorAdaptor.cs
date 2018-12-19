using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IRareTargetHitDetectorAdaptor: IShootingTargetNormalHitDetectorAdaptor{
		IRareTargetHitDetector GetRareTargetHitDetector();
	}
	public class RareTargetHitDetectorAdaptor: ShootingTargetNormalHitDetectorAdaptor, IRareTargetHitDetectorAdaptor{
		IRareTargetHitDetector thisRareTargetHitDetector{
			get{
				return (IRareTargetHitDetector)thisDetector;
			}
		}
		public IRareTargetHitDetector GetRareTargetHitDetector(){
			return thisRareTargetHitDetector;
		}
		protected override IArrowHitDetector CreateArrowHitDetector(){
			RareTargetHitDetector.IConstArg arg = new RareTargetHitDetector.ConstArg(this);
			return new RareTargetHitDetector(arg);
		}
	}
}

