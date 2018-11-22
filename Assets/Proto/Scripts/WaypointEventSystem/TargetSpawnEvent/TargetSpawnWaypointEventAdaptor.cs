using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ITargetSpawnWaypointEventAdaptor: IWaypointEventAdaptor{
	}
	public class TargetSpawnWaypointEventAdaptor : AppleShooterMonoBehaviourAdaptor, ITargetSpawnWaypointEventAdaptor {
		public float eventPoint;
		public override void SetUp(){
			SetUpEventsOnNextCurveWaypointEvent.IConstArg arg = new SetUpEventsOnNextCurveWaypointEvent.ConstArg(
				eventPoint
			);
			thisEvent = new SetUpEventsOnNextCurveWaypointEvent(arg);
		}
		public PCWaypointsManagerAdaptor pcWaypointsManagerAdaptor;
		public override void SetUpReference(){
			IPCWaypointsManager pcWaypointsManager = pcWaypointsManagerAdaptor.GetPCWaypointsManager();
			thisEvent.SetPCWaypointsManager(pcWaypointsManager);
		}
		protected ISetUpEventsOnNextCurveWaypointEvent thisEvent;
		public IWaypointEvent GetWaypointEvent(){
			return thisEvent;
		}
	}
}
