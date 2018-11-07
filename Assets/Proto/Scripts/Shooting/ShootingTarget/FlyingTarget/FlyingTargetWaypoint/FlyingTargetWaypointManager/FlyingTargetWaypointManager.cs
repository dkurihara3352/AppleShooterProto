using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IFlyingTargetWaypointManager: IShootingTargetSpawnPoint{
		IFlyingTargetWaypoint[] GetWaypoints();
		void SetWaypoints(IFlyingTargetWaypoint[] waypoints);
	}
	public class FlyingTargetWaypointManager : AbsShootingTargetSpawnPoint, IFlyingTargetWaypointManager {
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
