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

			StartWaypointsFollower();//		100
			// StartSmoothFollower();//		110 obsolete
			// PCLookAtTargetMotion			120	obsolete
			// StartPCSmoothLook();//		130 obsolete
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
		public void StartWaypointsFollower(){
			// IWaypointCurve firstWaypointGroup = waypointsManager.GetWaypointCurvesInSequence()[0];
			// follower.SetWaypointCurve(firstWaypointGroup);
			IWaypointsFollower follower = waypointsFollowerAdaptor.GetWaypointsFollower();
			follower.StartFollowing();
		}
		// public void StartSmoothFollower(){
		// 	ISmoothFollower smoothFollower = pcSmoothFollowerAdaptor.GetSmoothFollower();
		// 	smoothFollower.StartFollow();
		// }
		// public void StartPCSmoothLook(){
		// 	IPlayerCharacterLookAtTarget lookAtTarget = pcLookAtTargetAdaptor.GetLookAtTarget();
		// 	lookAtTarget.StartLookAtTargetMotion();
		// 	ISmoothLooker looker = pcSmoothLookerAdaptor.GetSmoothLooker();
		// 	looker.StartSmoothLook();
		// }
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
