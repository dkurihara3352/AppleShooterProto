using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public class ProtoGameManager : MonoBehaviour {

		public WaypointsManager waypointsManager;
		public WaypointsFollowerAdaptor followerAdaptor;
		IWaypointsFollower thisFollower;
		public SmoothFollowerAdaptor smoothFollowerAdaptor;
		public SmoothLookerAdaptor smoothLookerAdaptor;
		public LookAtTargetAdaptor lookAtTargetAdaptor;

		public void SetUpFollowerAndWithManager(){
			SetUpFollwerAdaptor();
			thisFollower = followerAdaptor.GetWaypointsFollower();
			SetUpWaypointsManager();
		}
		void SetUpFollwerAdaptor(){
			followerAdaptor.Initialize();
			followerAdaptor.SetUpWaypointsFollower(waypointsManager);
		}
		void SetUpWaypointsManager(){
			waypointsManager.SetFollower(thisFollower);
			waypointsManager.SetUpAllWaypointGroups();
		}
		public void PlaceWaypointGroups(){
			waypointsManager.PlaceWaypointGroups();
		}

		public void SetUpSmoothFollower(){
			smoothFollowerAdaptor.CreateAndSetSmoothFollower();
		}
		public void SetUpSmoothLooker(){
			smoothLookerAdaptor.SetUpSmoothLooker();
		}
		public void SetUpLookAtTarget(ISmoothLooker looker){
			lookAtTargetAdaptor.SetUpLookAtTarget(looker);
		}
		public void StartFollowerFollow(){
			IWaypointGroup firstWaypointGroup = waypointsManager.GetWaypointGroupsInSequence()[0];
			thisFollower.SetWaypointGroup(firstWaypointGroup);
			thisFollower.StartFollowing();

			ISmoothFollower smoothFollower = smoothFollowerAdaptor.GetSmoothFollower();
			smoothFollower.StartFollow();
		}
		public void StartSmootLook(){
			ILookAtTarget lookAtTarget = lookAtTargetAdaptor.GetLookAtTarget();
			lookAtTarget.StartLookAtTargetMotion();
			ISmoothLooker looker = smoothLookerAdaptor.GetSmoothLooker();
			looker.StartSmoothLook();
		}
		public int GetCurrentWaypointGroupIndex(){
			IWaypointGroup group = thisFollower.GetCurrentWaypointGroup();
			return waypointsManager.GetWaypointGroupIndex(group);
		}
		public int[] GetCurrentGroupSequence(){
			List<IWaypointGroup> groupsInSequence = waypointsManager.GetWaypointGroupsInSequence();
			List<int> resultList = new List<int>();
			foreach(IWaypointGroup group in groupsInSequence){
				int index = waypointsManager.GetWaypointGroupIndex(group);
				resultList.Add(index);
			}
			return resultList.ToArray();
		}
	}
}
