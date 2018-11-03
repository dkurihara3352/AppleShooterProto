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
			thisEvent = CreateWaypointEvent();
		}
		public GlidingTargetStartGlideWaypointEvent CreateWaypointEvent(){
			AbsWaypointEvent.IConstArg arg = new AbsWaypointEvent.ConstArg(
				eventPoint
			);
			return new GlidingTargetStartGlideWaypointEvent(arg);
		}
		public GlidingTargetReserveAdaptor glidingTargetReserveAdaptor;
		public WaypointCurveCycleManagerAdaptor glidingTargetWaypointsManagerAdaptor;
		public GlidingTargetWaypointCurveAdaptor glidingTargetWaypointCurveAdaptor;
		public override void SetUpReference(){
			IGlidingTargetReserve reserve = glidingTargetReserveAdaptor.GetGlidingTargetReserve();
			thisEvent.SetGlidingTargetReserve(reserve);
			IGlidingTargetWaypointCurve curve = glidingTargetWaypointCurveAdaptor.GetGlidingTargetWaypointCurve();
		}
	}
}
