﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IWaypointsFollowerAdaptor: IMonoBehaviourAdaptor{
		IWaypointsFollower GetWaypointsFollower();
		float GetSpeed();
	}
	public class WaypointsFollowerAdaptor : MonoBehaviourAdaptor, IWaypointsFollowerAdaptor {
		IWaypointsFollower thisFollower;
		public IWaypointsFollower GetWaypointsFollower(){
			return thisFollower;
		}
		public float followSpeed;
		public float GetSpeed(){return followSpeed;}
		public WaypointCurveCycleManagerAdaptor waypointsManagerAdaptor;
		public int processOrder = 100;
		public override void SetUp(){
			IWaypointsFollowerConstArg arg = new WaypointsFollowerConstArg(
				this, 
				processFactory,
				followSpeed,
				processOrder
			);
			thisFollower = new WaypointsFollower(arg);
		}
		public override void SetUpReference(){
			if(waypointsManagerAdaptor != null){
				IWaypointCurveCycleManager waypointsManager = waypointsManagerAdaptor.GetWaypointsManager();
				thisFollower.SetWaypointsManager(waypointsManager);
			}
		}
	}
}
