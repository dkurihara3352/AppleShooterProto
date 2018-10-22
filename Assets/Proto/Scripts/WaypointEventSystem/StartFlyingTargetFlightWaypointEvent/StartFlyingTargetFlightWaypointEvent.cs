using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IStartFlyingTargetFlightWaypointEvent: IWaypointEvent{
		void SetFlyingTarget(IFlyingTarget flyingTarget);
	}
	public class StartFlyingTargetFlightWaypointEvent : AbsWaypointEvent, IStartFlyingTargetFlightWaypointEvent {
		public StartFlyingTargetFlightWaypointEvent(
			IConstArg arg
		): base(arg){

		}
		IFlyingTarget thisFlyingTarget;
		public void SetFlyingTarget(IFlyingTarget target){
			thisFlyingTarget = target;
		}
		public override void Execute(){
			thisFlyingTarget.Activate();
		}
	}
}
