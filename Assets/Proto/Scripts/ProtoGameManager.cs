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
		public SmoothLookerAdaptor cameraPivotSmoothLookerAdaptor;
		public SmoothLookerAdaptor camSmoothLookerAdaptor;

		public MonoBehaviourAdaptorManager mbAdaptorManager;
		public UIManagerAdaptor uiManagerAdaptor;

		public CoreGameplayInputScrollerAdaptor coreGameplayInputScrollerAdaptor;
		public PlayerInputManagerAdaptor playerInputManagerAdaptor;
		public SmoothLookerAdaptor recticleSmoothLookerAdaptor;
		public PlayerCameraAdaptor playerCameraAdaptor;

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
			StartWaypointsFollower();//		100
			StartSmoothFollower();//		110
			// PCLookAtTargetMotion			120
			StartPCSmoothLook();//			130
			StartCameraSmoothFollow();//	140
			StartCameraPivotSmoothLook();//	150
			// StartRecticleSmoothLook();//	155
			StartCameraSmoothLook();//		160
			//Bow drawing					165
				//trajectory here
			StartSmoothZoom();//			170

			//arrow flight process			200 ->
			
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
		void StartCameraPivotSmoothLook(){
			ISmoothLooker cameraPivotSmoothLooker = cameraPivotSmoothLookerAdaptor.GetSmoothLooker();
			cameraPivotSmoothLooker.StartSmoothLook();
		}
		void StartCameraSmoothLook(){
			ISmoothLooker looker = camSmoothLookerAdaptor.GetSmoothLooker();
			looker.StartSmoothLook();
		}
		void StartRecticleSmoothLook(){
			ISmoothLooker looker = recticleSmoothLookerAdaptor.GetSmoothLooker();
			looker.StartSmoothLook();
		}
		void StartSmoothZoom(){
			IPlayerCamera playerCamera = playerCameraAdaptor.GetPlayerCamera();
			playerCamera.StartSmoothZoom();
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
