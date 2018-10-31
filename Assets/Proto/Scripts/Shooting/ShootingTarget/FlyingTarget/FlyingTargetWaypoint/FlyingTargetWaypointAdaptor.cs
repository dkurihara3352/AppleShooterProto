using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IFlyingTargetWaypointAdaptor: IMonoBehaviourAdaptor{
		IFlyingTargetWaypoint GetWaypoint();
	}
	public class FlyingTargetWaypointAdaptor : MonoBehaviourAdaptor, IFlyingTargetWaypointAdaptor {

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
