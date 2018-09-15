using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public class ProtoGameManager : MonoBehaviour {

		public WaypointsManager waypointsManager;
		public WaypointsFollowerAdaptor waypointsFollowerAdaptor;
		public SmoothFollowerAdaptor pcSmoothFollowerAdaptor;
		public SmoothLookerAdaptor pcSmoothLookerAdaptor;
		public PlayerCharacterLookAtTargetAdaptor pcLookAtTargetAdaptor;
		public SmoothFollowerAdaptor camSmoothFollowerAdaptor;
		public SmoothLookerAdaptor camSmoothLookerAdaptor;

		public MonoBehaviourAdaptorManager mbAdaptorManager;
		public UIManagerAdaptor uiManagerAdaptor;

		public CoreGameplayInputScrollerAdaptor coreGameplayInputScrollerAdaptor;
		public PlayerInputManagerAdaptor playerInputManagerAdaptor;

		public void SetUpUISystem(){
			IUIManager uim = uiManagerAdaptor.GetUIManager();
			uim.GetReadyForUISystemActivation();
		}
		public void SetUpMBAdaptors(){
			SetUpAllMonoBehaviourAdaptors();
			SetUpAdaptorReference();
		}
		void SetUpAllMonoBehaviourAdaptors(){
			mbAdaptorManager.SetUpAllMonoBehaviourAdaptors();
		}
		void SetUpAdaptorReference(){
			mbAdaptorManager.SetUpAdaptorReference();
		}

		public void RunSystem(){
			FinalizeUISystemSetUp();
			ActivateUISystem();

			waypointsManager.PlaceWaypointGroups();
			StartWaypointsFollower();
			StartSmoothFollower();
			StartPCSmoothLook();
			StartCameraSmoothFollow();
			StartCameraSmoothLook();
		}
		void FinalizeUISystemSetUp(){
			ICoreGameplayInputScroller scroller = coreGameplayInputScrollerAdaptor.GetInputScroller();
			IPlayerInputManager inputManager = playerInputManagerAdaptor.GetInputManager();
			scroller.SetPlayerInputManager(inputManager);
		}
		void ActivateUISystem(){
			IUIManager uim = uiManagerAdaptor.GetUIManager();
			uim.ActivateUISystem(false);
		}
		public void StartWaypointsFollower(){
			IWaypointGroup firstWaypointGroup = waypointsManager.GetWaypointGroupsInSequence()[0];
			IWaypointsFollower follower = waypointsFollowerAdaptor.GetWaypointsFollower();
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
		public int GetCurrentWaypointGroupIndex(){
			IWaypointsFollower follower = waypointsFollowerAdaptor.GetWaypointsFollower();
			IWaypointGroup group = follower.GetCurrentWaypointGroup();
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
