using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface IGlidingTargetStartGlideWaypointEvent: IWaypointEvent{
		void SetGlidingTarget(IGlidingTarget target);
	}
	public class GlidingTargetStartGlideWaypointEvent: AbsWaypointEvent, IGlidingTargetStartGlideWaypointEvent{
		public GlidingTargetStartGlideWaypointEvent(
			IConstArg arg
		): base(arg){}
		IGlidingTarget thisGlidingTarget;
		public void SetGlidingTarget(IGlidingTarget target){
			thisGlidingTarget = target;
		}
		public override void Execute(){
			thisGlidingTarget.Activate();
		}
	}
}

