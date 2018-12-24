﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using DKUtility;

namespace AppleShooterProto{
	public class ProtoGameManager : MonoBehaviour{
		public void SetUp(){
			SetUpMBAdaptors();
			SetUpSceneObjectRefs();
		}
		void SetUpMBAdaptors(){
			mbAdaptorManager.SetUpAllMonoBehaviourAdaptors();
			mbAdaptorManager.SetUpAdaptorReference();
			mbAdaptorManager.FinalizeSetUp();
		}
		public AppleShooterMonoBehaviourAdaptorManager mbAdaptorManager;
		void SetUpSceneObjectRefs(){
			thisPCWaypointsManager = pcWaypointsManagerAdaptor.GetPCWaypointsManager();
			thisPlayerCharacterWaypointsFollower = GetPlayerCharacterWaypointsFollower();
			thisRootUIElement = rootUIAdaptor.GetUIElement();
			thisScoreManager = scoreManagerAdaptor.GetScoreManager();
			thisPlayerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
		}
		/* ActivateRootUI */
			public void ActivateRootUI(){
				thisRootUIElement.ActivateRecursively(false);
			}
			IUIElement thisRootUIElement;
			public UIAdaptor rootUIAdaptor;
		/* WarmUp */
			public void WarmUp(){
				//RunSystem minus starting heat manager
				SetUpWaypointEventsOnFirstWaypointCurve();

				StartWaypointsFollower();//		100
				StartCameraSmoothFollow();//	140
				StartCameraPivotSmoothLook();//	150
				StartCameraSmoothLook();//		160
				//bow drawing 					165
				StartSmoothZoom();//			170
				//arrow flight 					200
			}
			public void SetUpWaypointEventsOnFirstWaypointCurve(){
				IPCWaypointsManager pcWaypointsManager = pcWaypointsManagerAdaptor.GetPCWaypointsManager();
				IPCWaypointCurve firstWaypointCurve = pcWaypointsManager.GetPCWaypointCurvesInSequence()[0];
				firstWaypointCurve.SetUpTargetSpawnEvents();
			}
			public PCWaypointsManagerAdaptor pcWaypointsManagerAdaptor;
			public void StartWaypointsFollower(){
				thisPlayerCharacterWaypointsFollower.SmoothStart();
			}
			IPlayerCharacterWaypointsFollower thisPlayerCharacterWaypointsFollower;
			IPlayerCharacterWaypointsFollower GetPlayerCharacterWaypointsFollower(){
				return playerCharacterWaypointsFollowerAdaptor.GetPlayerCharacterWaypointsFollower();
			}
			public PlayerCharacterWaypointsFollowerAdaptor playerCharacterWaypointsFollowerAdaptor;
			public void StartCameraSmoothFollow(){
				ISmoothFollower follower = camSmoothFollowerAdaptor.GetSmoothFollower();
				follower.StartFollow();
			}
			public SmoothFollowerAdaptor camSmoothFollowerAdaptor;
			void StartCameraPivotSmoothLook(){
				ISmoothLooker cameraPivotSmoothLooker = cameraPivotSmoothLookerAdaptor.GetSmoothLooker();
				cameraPivotSmoothLooker.StartSmoothLook();
			}
			public SmoothLookerAdaptor cameraPivotSmoothLookerAdaptor;
			void StartCameraSmoothLook(){
				ISmoothLooker looker = camSmoothLookerAdaptor.GetSmoothLooker();
				looker.StartSmoothLook();
			}
			public SmoothLookerAdaptor camSmoothLookerAdaptor;
			void StartSmoothZoom(){
				IPlayerCamera playerCamera = playerCameraAdaptor.GetPlayerCamera();
				playerCamera.StartSmoothZoom();
			}
			public PlayerCameraAdaptor playerCameraAdaptor;
		/* weird debug */
			public int GetCurrentWaypointGroupIndex(){
				IWaypointCurve group = thisPlayerCharacterWaypointsFollower.GetCurrentWaypointCurve();
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
			IPCWaypointsManager thisPCWaypointsManager;
		/*  */
			public void LoadHighScore(){
				thisPlayerDataManager.Load();
				int highScore = thisPlayerDataManager.GetHighScore();
				thisScoreManager.SetHighScore(highScore);
			}
			public void SaveHighScore(){
				int score = thisScoreManager.GetScore();

				int highScore = thisScoreManager.GetHighScore();
				if(
					highScore == 0 ||
					score > highScore
				){
					thisPlayerDataManager.SetHighScore(score);
					thisPlayerDataManager.Save();
				}
			}
			public ScoreManagerAdaptor scoreManagerAdaptor;
			IScoreManager thisScoreManager;
			public PlayerDataManagerAdaptor playerDataManagerAdaptor;
			IPlayerDataManager thisPlayerDataManager;

		/*  */
			

		}
}
