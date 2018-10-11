using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public class ProtoGameManager : MonoBehaviour {

		public WaypointsManagerAdaptor waypointsManagerAdaptor;
		IWaypointsManager thisWaypointsManager;
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
		public TestShootingTargetReserveAdaptor testShootingTargetReserveAdaptor;

		public void SetUp(){
			SetUpUISystem();
			SetUpMBAdaptors();
			FinalizeUISystemSetUp();
			FinalizeWaypointSetUp();
		}

		void SetUpUISystem(){
			IUIManager uim = uiManagerAdaptor.GetUIManager();
			uim.GetReadyForUISystemActivation();
		}
		void SetUpMBAdaptors(){
			SetUpAllMonoBehaviourAdaptors();
			SetUpAdaptorReference();
			thisWaypointsManager = waypointsManagerAdaptor.GetWaypointsManager();
		}
		void SetUpAllMonoBehaviourAdaptors(){
			mbAdaptorManager.SetUpAllMonoBehaviourAdaptors();
		}
		void SetUpAdaptorReference(){
			mbAdaptorManager.SetUpAdaptorReference();
		}
		void FinalizeUISystemSetUp(){
			ICoreGameplayInputScroller scroller = coreGameplayInputScrollerAdaptor.GetInputScroller();
			IPlayerInputManager inputManager = playerInputManagerAdaptor.GetInputManager();
			scroller.SetPlayerInputManager(inputManager);
		}
		void FinalizeWaypointSetUp(){
			thisWaypointsManager.PlaceWaypointCurves();
			IWaypointCurve initialWaypointCurve = thisWaypointsManager.GetWaypointCurvesInSequence()[0];
			IWaypointsFollower follower = waypointsFollowerAdaptor.GetWaypointsFollower();
			follower.SetWaypointCurve(initialWaypointCurve);
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
			IWaypointsManager waypointsManager = waypointsManagerAdaptor.GetWaypointsManager();
			IWaypointCurve firstWaypointCurve = waypointsManager.GetWaypointCurvesInSequence()[0];
			firstWaypointCurve.SpawnTargets();
		}

		public void GetTargetsReadyAtReserve(){
			ITestShootingTargetReserve reserve = testShootingTargetReserveAdaptor.GetTestShootingTargetReserve();
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
			return thisWaypointsManager.GetWaypointCurveIndex(group);
		}
		public int[] GetCurrentGroupSequence(){
			List<IWaypointCurve> curvesInSequence = thisWaypointsManager.GetWaypointCurvesInSequence();
			List<int> resultList = new List<int>();
			foreach(IWaypointCurve group in curvesInSequence){
				int index = thisWaypointsManager.GetWaypointCurveIndex(group);
				resultList.Add(index);
			}
			return resultList.ToArray();
		}
	}
}
