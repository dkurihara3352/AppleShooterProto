using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface ISmoothLookerAdaptor: IMonoBehaviourAdaptor{
		void SetUpSmoothLooker();
		ISmoothLooker GetSmoothLooker();
		void LookAt(Vector3 position);
	}
	public class SmoothLookerAdaptor: MonoBehaviourAdaptor, ISmoothLookerAdaptor{
		public ProcessManager processManager;
		public ISmoothLooker GetSmoothLooker(){
			return thisSmoothLooker;
		}
		ISmoothLooker thisSmoothLooker;
		public void SetUpSmoothLooker(){
			IAppleShooterProcessFactory processFactory = new AppleShooterProcessFactory(
				processManager
			);
			ISmoothLookerConstArg arg = new SmoothLookerConstArg(
				this,
				processFactory
			);
			thisSmoothLooker = new SmoothLooker(arg);
		}
		public void LookAt(Vector3 position){
			this.transform.LookAt(position, Vector3.up);
		}
	}
}

