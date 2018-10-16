using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ITargetSpawnWaypointEvent: IWaypointEvent{
		void SetWaypointsManager(IWaypointsManager waypointsManager);
	}
	public class TargetSpawnWaypointEvent : AbsWaypointEvent, ITargetSpawnWaypointEvent {
		public TargetSpawnWaypointEvent(
			IConstArg arg
		): base(arg){
			
		}
		IWaypointsManager thisWaypointsManager;
		public void SetWaypointsManager(IWaypointsManager waypointsManager){
			thisWaypointsManager = waypointsManager;
		}
		public override void Execute(){
			thisWaypointsManager.SpawnTargetsOnNextWaypointCurve();
		}
	}
}
