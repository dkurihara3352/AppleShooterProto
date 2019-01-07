using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface IWaypointsFollowerAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		IWaypointsFollower GetWaypointsFollower();
		float GetSpeed();
		float GetSmoothStartTime();
		AnimationCurve GetSmoothStartCurve();
		float GetSmoothStopTime();
		AnimationCurve GetSmoothStopCurve();
	}
	public class WaypointsFollowerAdaptor : SlickBowShootingMonoBehaviourAdaptor, IWaypointsFollowerAdaptor {
		public float followSpeed;
		public float GetSpeed(){return followSpeed;}
		public WaypointCurveCycleManagerAdaptor waypointsManagerAdaptor;
		public int processOrder = 100;
		public override void SetUp(){
			thisFollower = CreateFollower();
		}
		protected IWaypointsFollower thisFollower;
		public IWaypointsFollower GetWaypointsFollower(){
			return thisFollower;
		}
		protected virtual IWaypointsFollower CreateFollower(){
			WaypointsFollower.IConstArg arg = new WaypointsFollower.ConstArg(
				this,
				followSpeed,
				processOrder
			);
			return new WaypointsFollower(arg);
		}
		public override void SetUpReference(){
			if(waypointsManagerAdaptor != null){
				IWaypointCurveCycleManager waypointsManager = waypointsManagerAdaptor.GetWaypointsManager();
				thisFollower.SetWaypointsManager(waypointsManager);
			}
		}
		public AnimationCurve smoothStartCurve;
		public AnimationCurve GetSmoothStartCurve(){
			return smoothStartCurve;
		}
		public float smoothStartTime;
		public float GetSmoothStartTime(){
			return smoothStartTime;
		}
		public AnimationCurve smoothStopCurve;
		public AnimationCurve GetSmoothStopCurve(){
			return smoothStopCurve;
		}
		public float smoothStopTime;
		public float GetSmoothStopTime(){
			return smoothStopTime;
		}
	}
}
