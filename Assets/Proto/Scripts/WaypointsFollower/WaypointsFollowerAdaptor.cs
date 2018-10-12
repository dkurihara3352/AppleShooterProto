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
		public WaypointsManagerAdaptor waypointsManagerAdaptor;
		public int processOrder = 100;
		public override void SetUp(){
			thisProcessFactory = new AppleShooterProcessFactory(
				processManager
			);
			IWaypointsFollowerConstArg arg = new WaypointsFollowerConstArg(
				this, 
				thisProcessFactory,
				followSpeed,
				processOrder
			);
			thisFollower = new WaypointsFollower(arg);
		}
		public override void SetUpReference(){
			IWaypointsManager waypointsManager = waypointsManagerAdaptor.GetWaypointsManager();
			thisFollower.SetWaypointsManager(waypointsManager);
		}
	}
}
