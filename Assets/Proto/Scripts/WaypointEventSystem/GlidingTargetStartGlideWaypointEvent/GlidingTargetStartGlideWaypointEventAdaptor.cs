using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetStartGlideWaypointEventAdaptor: IWaypointEventAdaptor{

	}
	public class GlidingTargetStartGlideWaypointEventAdaptor: MonoBehaviourAdaptor, IGlidingTargetStartGlideWaypointEventAdaptor{
		IGlidingTargetStartGlideWaypointEvent thisEvent;
		public IWaypointEvent GetWaypointEvent(){
			return thisEvent;
		}
		public float eventPoint;
		public override void SetUp(){
			AbsWaypointEvent.IConstArg arg = new AbsWaypointEvent.ConstArg(
				eventPoint
			);
			thisEvent = new GlidingTargetStartGlideWaypointEvent(arg);
		}
		public GlidingTargetAdaptor glidingTargetAdaptor;
		public override void SetUpReference(){
			IGlidingTarget target = (IGlidingTarget)glidingTargetAdaptor.GetShootingTarget();
			thisEvent.SetGlidingTarget(target);
		}
	}
}
