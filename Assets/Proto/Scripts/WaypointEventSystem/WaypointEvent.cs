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
			IAbsConstArg arg
		){
			thisEventPoint = arg.eventPoint;
		}
		readonly float thisEventPoint;
		public float GetEventPoint(){
			return thisEventPoint;
		}
		public abstract void Execute();
		public interface IAbsConstArg{
			float eventPoint{get;}
		}
	}
}

