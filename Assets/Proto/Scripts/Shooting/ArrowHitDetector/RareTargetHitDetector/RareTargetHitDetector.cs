using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IRareTargetHitDetector: IShootingTargetNormalHitDetector{
	}
	public class RareTargetHitDetector: ShootingTargetNormalHitDetector, IRareTargetHitDetector{
		public RareTargetHitDetector(IConstArg arg): base(arg){
		}
		int thisCritLayerID = 9;
		int thisLayerMask{
			get{
				return 1 << thisCritLayerID;
			}
		}
		public override void Hit(IArrow arrow){
			Vector3 arrowPrevPosition = arrow.GetPrevPosition();
			RaycastHit critHit;
			Vector3 direction = arrow.GetPosition() - arrowPrevPosition;
			bool hasCritHit = Physics.Raycast(
				arrowPrevPosition,
				direction,
				out critHit,
				10f,
				thisLayerMask
			);
			thisShootingTarget.Hit(arrow, hasCritHit);
		}
	}
}

