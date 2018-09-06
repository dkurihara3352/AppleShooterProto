using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IWaypointsFollowerAdaptor: ISmoothFollowTargetMBAdaptor{
		void Initialize();
		void SetUpWaypointsFollower(IWaypointsManager waypointsManager);
		IWaypointsFollower GetWaypointsFollower();
	}
	public class WaypointsFollowerAdaptor : MonoBehaviourAdaptor, IWaypointsFollowerAdaptor {
		public void Initialize(){
			thisProcessFactory = new AppleShooterProcessFactory(
				processManager
			);
		}
		public ProcessManager processManager;
		IWaypointsFollower thisFollower;
		public IWaypointsFollower GetWaypointsFollower(){
			return thisFollower;
		}
		IAppleShooterProcessFactory thisProcessFactory;
		public float followSpeed;
		public void SetUpWaypointsFollower(IWaypointsManager waypointsManager){
			IWaypointsFollowerConstArg arg = new WaypointsFollowerConstArg(
				waypointsManager,
				this, 
				thisProcessFactory,
				followSpeed
			);
			thisFollower = new WaypointsFollower(arg);
		}
	}
}
