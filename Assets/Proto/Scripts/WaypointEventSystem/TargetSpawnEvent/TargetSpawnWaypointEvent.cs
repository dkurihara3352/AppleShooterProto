using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ITargetSpawnWaypointEvent: IWaypointEvent{
		void SetPCWaypointsManager(IPCWaypointsManager pcWaypointsManager);
	}
	public class TargetSpawnWaypointEvent : AbsWaypointEvent, ITargetSpawnWaypointEvent {
		public TargetSpawnWaypointEvent(
			IConstArg arg
		): base(arg){
			
		}
		IPCWaypointsManager thisPCWaypointsManager;
		public void SetPCWaypointsManager(IPCWaypointsManager pcWaypointsManager){
			thisPCWaypointsManager = pcWaypointsManager;
		}
		public override void Execute(){
			IPCWaypointCurve nextCurve = thisPCWaypointsManager.GetNextPCCurve();
			nextCurve.SpawnTargets();
		}
	}
}
