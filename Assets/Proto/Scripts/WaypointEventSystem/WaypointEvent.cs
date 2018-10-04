using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointEvent{
		void Execute();
		float GetEventPoint();
	}
	public abstract class AbsWaypointEvent: IWaypointEvent{
		public AbsWaypointEvent(
			IWaypointEventConstArg arg
		){
			thisEventPoint = arg.eventPoint;
		}
		readonly float thisEventPoint;
		public float GetEventPoint(){
			return thisEventPoint;
		}
		public abstract void Execute();
	}

	public interface IWaypointEventConstArg{
		float eventPoint{get;}
	}
	public struct WaypointEventConstArg: IWaypointEventConstArg{
		public WaypointEventConstArg(
			float eventPoint
		){
			thisEventPoint = eventPoint;
		}
		readonly float thisEventPoint;
		public float eventPoint{get{return thisEventPoint;}}
	}
}

