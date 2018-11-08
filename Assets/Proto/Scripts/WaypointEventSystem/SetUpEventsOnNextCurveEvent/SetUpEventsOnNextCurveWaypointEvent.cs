using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ISetUpEventsOnNextCurveWaypointEvent: IWaypointEvent{
		void SetPCWaypointsManager(IPCWaypointsManager pcWaypointsManager);
	}
	public class SetUpEventsOnNextCurveWaypointEvent : AbsWaypointEvent, ISetUpEventsOnNextCurveWaypointEvent {
		public SetUpEventsOnNextCurveWaypointEvent(
			IConstArg arg
		): base(arg){
			
		}
		IPCWaypointsManager thisPCWaypointsManager;
		public void SetPCWaypointsManager(IPCWaypointsManager pcWaypointsManager){
			thisPCWaypointsManager = pcWaypointsManager;
		}
		public override void Execute(){
			IPCWaypointCurve nextCurve = thisPCWaypointsManager.GetNextPCCurve();
			if(nextCurve != null)
				nextCurve.SetUpTargetSpawnEvents();
		}
	}
}
