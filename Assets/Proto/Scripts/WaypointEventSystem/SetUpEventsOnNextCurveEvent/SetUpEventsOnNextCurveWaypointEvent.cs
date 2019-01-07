using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
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
		protected override void ExecuteImple(){
			IPCWaypointCurve nextCurve = thisPCWaypointsManager.GetNextPCCurve();
			if(nextCurve != null)
				nextCurve.SetUpTargetSpawnEvents();
		}
		public override string GetName(){
			return "SetUpEventsOnNextCurveWaypointEvent";
		}
	}
}
