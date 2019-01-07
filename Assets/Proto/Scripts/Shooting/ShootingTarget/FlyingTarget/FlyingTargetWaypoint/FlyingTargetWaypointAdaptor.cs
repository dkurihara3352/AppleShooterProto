using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IFlyingTargetWaypointAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		IFlyingTargetWaypoint GetWaypoint();
	}
	public class FlyingTargetWaypointAdaptor : SlickBowShootingMonoBehaviourAdaptor, IFlyingTargetWaypointAdaptor {

		public override void SetUp(){
			FlyingTargetWaypoint.IConstArg arg =  new FlyingTargetWaypoint.ConstArg(
				this
			);
			thisWaypoint = new FlyingTargetWaypoint(arg);
		}
		IFlyingTargetWaypoint thisWaypoint; 
		public IFlyingTargetWaypoint GetWaypoint(){
			return thisWaypoint;
		}
	}
}
