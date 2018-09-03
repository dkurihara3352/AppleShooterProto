using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public class ProtoGameManager : MonoBehaviour {

		public WaypointsManager waypointsManager;
		public WaypointsFollowerAdaptor followerAdaptor;

		public void SetUpFollowerWithWaypoints(){
			followerAdaptor.Initialize();
			followerAdaptor.SetUpWaypointsFollower();
			IWaypointsFollower follower = followerAdaptor.GetWaypointsFollower();
			waypointsManager.SetFollower(follower);
			waypointsManager.SetUpWaypoints();
			List<IWaypoint> waypoints = waypointsManager.GetWaypoints();
			follower.SetWaypoints(waypoints);
		}
		public void StartFollowerFollow(){
			IWaypointsFollower follower = followerAdaptor.GetWaypointsFollower();
			follower.StartFollowing();
		}
	}
}
