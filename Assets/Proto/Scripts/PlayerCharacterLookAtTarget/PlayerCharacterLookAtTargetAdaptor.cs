using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IPlayerCharacterLookAtTargetAdaptor: IMonoBehaviourAdaptor{
		IPlayerCharacterLookAtTarget GetLookAtTarget();
	}
	public class PlayerCharacterLookAtTargetAdaptor: MonoBehaviourAdaptor, IPlayerCharacterLookAtTargetAdaptor{
		public ProcessManager processManager;
		IPlayerCharacterLookAtTarget thisLookAtTarget;
		public int processOrder;
		public override void SetUp(){
			IAppleShooterProcessFactory processFactory = new AppleShooterProcessFactory(
				processManager
			);
			ILookAtTargetConstArg arg = new LookAtTargetConstArg(
				this,
				processFactory,
				processOrder
			);
			thisLookAtTarget = new PlayerCharacterLookAtTarget(arg);
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
	}	
}
