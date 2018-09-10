using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public class ProtoGameManager : MonoBehaviour {

		public WaypointsManager waypointsManager;
		public WaypointsFollowerAdaptor wpFollowerAdaptor;
		public SmoothFollowerAdaptor pcSmoothFollowerAdaptor;
		public SmoothLookerAdaptor pcSmoothLookerAdaptor;
		public PlayerCharacterLookAtTargetAdaptor pcLookAtTargetAdaptor;
		public SmoothFollowerAdaptor camSmoothFollowerAdaptor;
		public SmoothLookerAdaptor camSmoothLookerAdaptor;
		public SmoothFollowerAdaptor camLookAtTarget;

		public MonoBehaviourAdaptorManager mbAdaptorManager;
		IWaypointsFollower thisFollower;
		public void SetUp(){
			SetUpAllMonoBehaviourAdaptors();
			SetUpAdaptorReference();
			thisFollower = wpFollowerAdaptor.GetWaypointsFollower();
		}
		void SetUpAllMonoBehaviourAdaptors(){
			mbAdaptorManager.SetUpAllMonoBehaviourAdaptors();
		}
		void SetUpAdaptorReference(){
			mbAdaptorManager.SetUpAdaptorReference();
		}


		public void RunSystem(){
			waypointsManager.PlaceWaypointGroups();
			StartWaypointsFollower();
			StartSmoothFollower();
			StartPCSmoothLook();
			StartCameraSmoothFollow();
			StartCameraSmoothLook();
			StartCameraLookAtTargetSmoothFollow();
		}
		public void StartWaypointsFollower(){
			IWaypointGroup firstWaypointGroup = waypointsManager.GetWaypointGroupsInSequence()[0];
			IWaypointsFollower follower = wpFollowerAdaptor.GetWaypointsFollower();
			follower.SetWaypointGroup(firstWaypointGroup);
			follower.StartFollowing();
		}
		public void StartSmoothFollower(){
			ISmoothFollower smoothFollower = pcSmoothFollowerAdaptor.GetSmoothFollower();
			smoothFollower.StartFollow();
		}
		public void StartPCSmoothLook(){
			IPlayerCharacterLookAtTarget lookAtTarget = pcLookAtTargetAdaptor.GetLookAtTarget();
			lookAtTarget.StartLookAtTargetMotion();
			ISmoothLooker looker = pcSmoothLookerAdaptor.GetSmoothLooker();
			looker.StartSmoothLook();
		}
		public void StartCameraSmoothFollow(){
			ISmoothFollower follower = camSmoothFollowerAdaptor.GetSmoothFollower();
			follower.StartFollow();
		}
		void StartCameraSmoothLook(){
			ISmoothLooker looker = camSmoothLookerAdaptor.GetSmoothLooker();
			looker.StartSmoothLook();
		}
		void StartCameraLookAtTargetSmoothFollow(){
			ISmoothFollower follower = camLookAtTarget.GetSmoothFollower();
			follower.StartFollow();
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
