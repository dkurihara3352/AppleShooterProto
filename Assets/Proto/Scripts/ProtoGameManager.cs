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

		public AppleShooterMonoBehaviourAdaptorManager mbAdaptorManager;
		public UIManagerAdaptor uiManagerAdaptor;

		public CoreGameplayInputScrollerAdaptor coreGameplayInputScrollerAdaptor;
		public PlayerInputManagerAdaptor playerInputManagerAdaptor;
		public SmoothLookerAdaptor recticleSmoothLookerAdaptor;
		public PlayerCameraAdaptor playerCameraAdaptor;
		public StaticShootingTargetReserveAdaptor staticShootingTargetReserveAdaptor;

		public void SetUp(){
			SetUpMBAdaptors();
			SetUpSceneObjectRefs();
		}
		void SetUpMBAdaptors(){
			mbAdaptorManager.SetUpAllMonoBehaviourAdaptors();
			mbAdaptorManager.SetUpAdaptorReference();
			mbAdaptorManager.FinalizeSetUp();
		}
		void SetUpAllMonoBehaviourAdaptors(){
		}
		void SetUpAdaptorReference(){
		}
		void FinalizeMBAdaptorSetUp(){
		}
		void FinalizeUISystemSetUp(){
			ICoreGameplayInputScroller scroller = coreGameplayInputScrollerAdaptor.GetInputScroller();
			IPlayerInputManager inputManager = playerInputManagerAdaptor.GetInputManager();
			scroller.SetPlayerInputManager(inputManager);
		}
		IHeatManager thisHeatManager;
		public HeatManagerAdaptor heatManagerAdaptor;
		void SetUpSceneObjectRefs(){
			thisPCWaypointsManager = pcWaypointsManagerAdaptor.GetPCWaypointsManager();
			thisHeatManager = heatManagerAdaptor.GetHeatManager();
			thisRootUIElement = GetRootUIElement();
		}

		public void RunSystem(){

			ActivateUISystem();
			// GetTargetsReadyAtReserve();
			SetUpWaypointEventsOnFirstWaypointCurve();

			StartWaypointsFollower();//		100
			StartCameraSmoothFollow();//	140
			StartCameraPivotSmoothLook();//	150
			// StartRecticleSmoothLook();//	155
			StartCameraSmoothLook();//		160
			//Bow drawing					165
				//trajectory here
			StartSmoothZoom();//			170

			StartHeatManager();

			//arrow flight process			200 ->
			
		}
		IUIElement thisRootUIElement;
		public UIAdaptor rootUIAdaptor;
		IUIElement GetRootUIElement(){
			return rootUIAdaptor.GetUIElement();
		}
		void ActivateUISystem(){
			// IUIManager uim = uiManagerAdaptor.GetUIManager();
			// uim.ActivateUISystem(false);
			thisRootUIElement.ActivateRecursively(instantly: false);
		}
		public void SetUpWaypointEventsOnFirstWaypointCurve(){
			IPCWaypointsManager pcWaypointsManager = pcWaypointsManagerAdaptor.GetPCWaypointsManager();
			IPCWaypointCurve firstWaypointCurve = pcWaypointsManager.GetPCWaypointCurvesInSequence()[0];
			firstWaypointCurve.SetUpTargetSpawnEvents();
		}

		public void StartWaypointsFollower(){
			IWaypointsFollower follower = waypointsFollowerAdaptor.GetWaypointsFollower();
			// follower.StartFollowing();
			follower.SmoothStart();
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
		void StartHeatManager(){
			thisHeatManager.StartCountingDown();
		}
	}
}
