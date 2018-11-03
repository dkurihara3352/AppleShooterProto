using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface IGlidingTargetStartGlideWaypointEvent: IWaypointEvent{
		void SetGlidingTargetReserve(IGlidingTargetReserve reserve);
		void SetGlidingTargetWaypointCurve(IGlidingTargetWaypointCurve curve);
	}
	public class GlidingTargetStartGlideWaypointEvent: AbsWaypointEvent, IGlidingTargetStartGlideWaypointEvent{
		public GlidingTargetStartGlideWaypointEvent(
			IConstArg arg
		): base(arg){}
		IGlidingTargetReserve thisGlidingTargetReserve;
		public void SetGlidingTargetReserve(IGlidingTargetReserve reserve){
			thisGlidingTargetReserve = reserve;
		}
		IGlidingTargetWaypointCurve thisWaypointCurve;
		public void SetGlidingTargetWaypointCurve(IGlidingTargetWaypointCurve curve){
			thisWaypointCurve = curve;
		}
		public override void Execute(){
			thisGlidingTargetReserve.ActivateGlidingTargetAt(
				thisWaypointCurve
			);
		}
	}
}

