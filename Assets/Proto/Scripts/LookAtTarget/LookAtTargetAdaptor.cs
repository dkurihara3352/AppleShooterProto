using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface ILookAtTargetAdaptor: IMonoBehaviourAdaptor{
		void SetUpLookAtTarget(ISmoothLooker looker);
		ILookAtTarget GetLookAtTarget();
	}
	public class LookAtTargetAdaptor: MonoBehaviourAdaptor, ILookAtTargetAdaptor{
		public ProcessManager processManager;
		public float smoothCoefficient = 1f;
		ILookAtTarget thisLookAtTarget;
		public void SetUpLookAtTarget(ISmoothLooker looker){
			IAppleShooterProcessFactory processFactory = new AppleShooterProcessFactory(
				processManager
			);
			ILookAtTargetConstArg arg = new LookAtTargetConstArg(
				this,
				looker,
				processFactory,
				smoothCoefficient
			);
			thisLookAtTarget = new LookAtTarget(arg);
		}
		public ILookAtTarget GetLookAtTarget(){
			return thisLookAtTarget;
		}
	}	
}
