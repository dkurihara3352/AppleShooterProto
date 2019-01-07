using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface IPlayerCharacterLookAtTargetAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		IPlayerCharacterLookAtTarget GetLookAtTarget();
	}
	public class PlayerCharacterLookAtTargetAdaptor: SlickBowShootingMonoBehaviourAdaptor, IPlayerCharacterLookAtTargetAdaptor{
		IPlayerCharacterLookAtTarget thisLookAtTarget;
		public int processOrder;
		public override void SetUp(){
			thisLookAtTarget = CreateLookAtTarget();
		}
		public SmoothLookerAdaptor smoothLookerAdaptor;
		public override void SetUpReference(){
			ISmoothLooker looker = smoothLookerAdaptor.GetSmoothLooker();
			IPlayerCharacterLookAtTarget lookAtTarget = GetLookAtTarget();
			lookAtTarget.SetSmoothLooker(looker);
			lookAtTarget.SetDirection(new Vector3(0f, 0f, 1f));
		}
		public IPlayerCharacterLookAtTarget GetLookAtTarget(){
			return thisLookAtTarget;
		}
		IPlayerCharacterLookAtTarget CreateLookAtTarget(){
			PlayerCharacterLookAtTarget.IConstArg arg = new PlayerCharacterLookAtTarget.ConstArg(
				this,
				processOrder
			);
			return new PlayerCharacterLookAtTarget(arg);
		}
	}	
}
