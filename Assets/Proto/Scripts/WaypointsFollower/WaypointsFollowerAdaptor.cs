using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IWaypointsFollowerAdaptor: IMonoBehaviourAdaptor{
		IWaypointsFollower GetWaypointsFollower();
		float GetSpeed();
	}
	public class WaypointsFollowerAdaptor : MonoBehaviourAdaptor, IWaypointsFollowerAdaptor {
		public ProcessManager processManager;
		IWaypointsFollower thisFollower;
		public IWaypointsFollower GetWaypointsFollower(){
			return thisFollower;
		}
		IAppleShooterProcessFactory thisProcessFactory;
		public float followSpeed;
		public float GetSpeed(){return followSpeed;}
		public WaypointsManager thisWaypointsManager;
		public override void SetUp(){
			thisProcessFactory = new AppleShooterProcessFactory(
				processManager
			);
			IWaypointsFollowerConstArg arg = new WaypointsFollowerConstArg(
				thisWaypointsManager,
				this, 
				thisProcessFactory,
				followSpeed
			);
			thisFollower = new WaypointsFollower(arg);
		}
	}
}
