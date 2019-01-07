using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IFlyingTargetWaypointManager: ISlickBowShootingSceneObject{
		IFlyingTargetWaypoint[] GetWaypoints();
		void SetWaypoints(IFlyingTargetWaypoint[] waypoints);
	}
	public class FlyingTargetWaypointManager : SlickBowShootingSceneObject, IFlyingTargetWaypointManager {
		public FlyingTargetWaypointManager(
			IConstArg arg
		): base(arg){

		}
		IFlyingTargetWaypoint[] thisWaypoints;
		public IFlyingTargetWaypoint[] GetWaypoints(){
			return thisWaypoints;
		}
		public void SetWaypoints(IFlyingTargetWaypoint[] waypoints){
			thisWaypoints = waypoints;
		}
	}
}
