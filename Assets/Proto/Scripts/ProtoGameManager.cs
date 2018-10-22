using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public class ProtoGameManager : MonoBehaviour {

		public PCWaypointsManagerAdaptor pcWaypointsManagerAdaptor;
		IPCWaypointsManager thisPCWaypointsManager;
		public WaypointsFollowerAdaptor waypointsFollowerAdaptor;
		public SmoothFollowerAdaptor camSmoothFollowerAdaptor;
		public SmoothLookerAdaptor cameraPivotSmoothLookerAdaptor;
		public SmoothLookerAdaptor camSmoothLookerAdaptor;

		public MonoBehaviourAdaptorManager mbAdaptorManager;
		public UIManagerAdaptor uiManagerAdaptor;

		public CoreGameplayInputScrollerAdaptor coreGameplayInputScrollerAdaptor;
		public PlayerInputManagerAdaptor playerInputManagerAdaptor;
		public SmoothLookerAdaptor recticleSmoothLookerAdaptor;
		public PlayerCameraAdaptor playerCameraAdaptor;
		public StaticShootingTargetReserveAdaptor staticShootingTargetReserveAdaptor;

		public void SetUp(){
			SetUpUISystem();
			SetUpMBAdaptors();
			FinalizeUISystemSetUp();
		}

		void SetUpUISystem(){
			IUIManager uim = uiManagerAdaptor.GetUIManager();
			uim.GetReadyForUISystemActivation();
		}
		void SetUpMBAdaptors(){
			SetUpAllMonoBehaviourAdaptors();
			SetUpAdaptorReference();
			FinalizeMBAdaptorSetUp();
			thisPCWaypointsManager = pcWaypointsManagerAdaptor.GetPCWaypointsManager();
		}
		void SetUpAllMonoBehaviourAdaptors(){
			mbAdaptorManager.SetUpAllMonoBehaviourAdaptors();
		}
		void SetUpAdaptorReference(){
			mbAdaptorManager.SetUpAdaptorReference();
		}
		void FinalizeMBAdaptorSetUp(){
			mbAdaptorManager.FinalizeSetUp();
		}
		void FinalizeUISystemSetUp(){
			ICoreGameplayInputScroller scroller = coreGameplayInputScrollerAdaptor.GetInputScroller();
			IPlayerInputManager inputManager = playerInputManagerAdaptor.GetInputManager();
			scroller.SetPlayerInputManager(inputManager);
		}

		public void RunSystem(){

			ActivateUISystem();
			GetTargetsReadyAtReserve();
			SpawnTargetsOnFirstWaypointCurve();

			StartWaypointsFollower();//		100
			StartCameraSmoothFollow();//	140
			StartCameraPivotSmoothLook();//	150
			// StartRecticleSmoothLook();//	155
			StartCameraSmoothLook();//		160
			//Bow drawing					165
				//trajectory here
			StartSmoothZoom();//			170

			//arrow flight process			200 ->
			
		}
		void ActivateUISystem(){
			IUIManager uim = uiManagerAdaptor.GetUIManager();
			uim.ActivateUISystem(false);
		}
		public void SpawnTargetsOnFirstWaypointCurve(){
			IPCWaypointsManager pcWaypointsManager = pcWaypointsManagerAdaptor.GetPCWaypointsManager();
			IPCWaypointCurve firstWaypointCurve = pcWaypointsManager.GetPCWaypointCurvesInSequence()[0];
			firstWaypointCurve.SpawnTargets();
		}

		public void GetTargetsReadyAtReserve(){
			IStaticShootingTargetReserve reserve = staticShootingTargetReserveAdaptor.GetStaticShootingTargetReserve();
			reserve.GetTargetsReadyInReserve();
		}
		public void StartWaypointsFollower(){
			IWaypointsFollower follower = waypointsFollowerAdaptor.GetWaypointsFollower();
			follower.StartFollowing();
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
			IWaypointCurve group = follower.GetCurrentWaypointCurve();
			return thisPCWaypointsManager.GetWaypointCurveIndex(group);
		}
		public int[] GetCurrentGroupSequence(){
			List<IWaypointCurve> curvesInSequence = thisPCWaypointsManager.GetWaypointCurvesInSequence();
			List<int> resultList = new List<int>();
			foreach(IWaypointCurve group in curvesInSequence){
				int index = thisPCWaypointsManager.GetWaypointCurveIndex(group);
				resultList.Add(index);
			}
			return resultList.ToArray();
		}
	}
}
