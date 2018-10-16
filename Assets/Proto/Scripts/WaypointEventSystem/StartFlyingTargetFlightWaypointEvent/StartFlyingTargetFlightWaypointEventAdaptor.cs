using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IStartFlyingTargetFlightWaypointEventAdaptor: IWaypointEventAdaptor{}
	public class StartFlyingTargetFlightWaypointEventAdaptor: MonoBehaviourAdaptor, IStartFlyingTargetFlightWaypointEventAdaptor{
		IStartFlyingTargetFlightWaypointEvent thisEvent;
		public IWaypointEvent GetWaypointEvent(){
			return thisEvent;
		}
		public float eventPoint;
		public override void SetUp(){
			AbsWaypointEvent.IConstArg arg = new AbsWaypointEvent.ConstArg(
				eventPoint
			);
			thisEvent = new StartFlyingTargetFlightWaypointEvent(arg);
		}
		public FlyingTargetAdaptor flyingTargetAdaptor;
		public override void SetUpReference(){
			IFlyingTarget flyingTarget = flyingTargetAdaptor.GetShootingTarget() as IFlyingTarget;
			thisEvent.SetFlyingTarget(flyingTarget);
		}
	}
}

