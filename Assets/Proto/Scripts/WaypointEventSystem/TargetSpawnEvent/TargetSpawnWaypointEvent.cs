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
			// thisTargetSpawnManager.Spawn();
			thisWaypointsManager.SpawnTargetsOnNextWaypointCurve();
		}
		/* Const */
		public interface IConstArg: IAbsConstArg{
		}
		public struct ConstArg: IConstArg{
			public ConstArg(
				float eventPoint
			){
				thisEventPoint = eventPoint;
			}
			readonly float thisEventPoint;
			public float eventPoint{get{return thisEventPoint;}}
		}
	}
}
