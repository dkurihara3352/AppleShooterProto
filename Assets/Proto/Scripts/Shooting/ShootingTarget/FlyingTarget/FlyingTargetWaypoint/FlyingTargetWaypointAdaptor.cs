using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IFlyingTargetWaypointAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IFlyingTargetWaypoint GetWaypoint();
	}
	public class FlyingTargetWaypointAdaptor : AppleShooterMonoBehaviourAdaptor, IFlyingTargetWaypointAdaptor {

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
