using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public class ProtoGameManager : MonoBehaviour {

		public WaypointsManager waypointsManager;
		public WaypointsFollowerAdaptor followerAdaptor;

		public void SetUpFollowerAndWithManager(){
			SetUpFollwerAdaptor();
			IWaypointsFollower follower = followerAdaptor.GetWaypointsFollower();
			SetUpWaypointsManager(follower);
		}
		void SetUpFollwerAdaptor(){
			followerAdaptor.Initialize();
			followerAdaptor.SetUpWaypointsFollower(waypointsManager);
		}
		void SetUpWaypointsManager(IWaypointsFollower follower){
			waypointsManager.SetFollower(follower);
			waypointsManager.SetUpAllWaypointGroups();
		}
		public void PlaceWaypointGroups(){
			waypointsManager.PlaceWaypointGroups();
		}
		public void StartFollowerFollow(){
			IWaypointsFollower follower = followerAdaptor.GetWaypointsFollower();
			IWaypointGroup firstWaypointGroup = waypointsManager.GetWaypointGroups()[0];
			follower.SetWaypointGroup(firstWaypointGroup);
			follower.StartFollowing();
		}
	}
}
