using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public class GUIManager2 : AbsGUIManager {

		public void Awake(){
			topLeft = GetGUIRect(
				normalizedSize: new Vector2(.2f, .4f),
				normalizedPosition: new Vector2(0f, 0f)
			);
			sTL_1 = GetSubRect( topLeft, 0, 5);
			sTL_2 = GetSubRect( topLeft, 1, 5);
			sTL_3 = GetSubRect( topLeft, 2, 5);
			sTL_4 = GetSubRect( topLeft, 3, 5);
			sTL_5 = GetSubRect( topLeft, 4, 5);
			topRight  = GetGUIRect(
				normalizedSize: new Vector2(.2f, .5f),
				normalizedPosition: new Vector2(1f, 0f)
			);
			sTR_1 = GetSubRect(
				topRight,
				0,
				3
			);
			sTR_2 = GetSubRect(
				topRight,
				1,
				3
			);
			sTR_3 = GetSubRect(
				topRight,
				2,
				3
			);
		}
		Rect topLeft;
		Rect sTL_1;
		Rect sTL_2;
		Rect sTL_3;
		Rect sTL_4;
		Rect sTL_5;

		Rect topRight;
		Rect sTR_1;
		Rect sTR_2;
		Rect sTR_3;
		public MonoBehaviourAdaptorManager monoBehaviourAdaptorManager;
		public FlyingTargetAdaptor flyingTargetAdaptor;
		// public WaypointsFollowerAdaptor glidingTargetWPFollowerAdaptor;
		public GlidingTargetAdaptor glidingTargetAdaptor;
		public WaypointsManagerAdaptor waypointsManagerAdaptor;
		public MarkerUIAdaptor markerUIAdaptor;
		/*  */
		bool thisSystemIsReady = false;
		public void OnGUI(){
			DrawControl(sTL_1);
			DrawFlyingTarget(sTR_1);
		}

		void DrawControl(Rect rect){
			if(GUI.Button(
				sTL_1,
				"SetUp"
			)){
				monoBehaviourAdaptorManager.SetUpAllMonoBehaviourAdaptors();
				monoBehaviourAdaptorManager.SetUpAdaptorReference();
				monoBehaviourAdaptorManager.FinalizeSetUp();
				thisSystemIsReady = true;
			}
			if(GUI.Button(
				sTL_2,
				"Run"
			)){
				ActivateFlyingTarget();
				// FinalizeWaypointCurves();
				// StartGlidingTargetGlide();
			}
			if(GUI.Button(
				sTL_3,
				"Reset"
			)){
				DeactivateGlidingTarget();
			}
			if(
				(GUI.Button(
					sTL_4,
					"ActivateMarkerUI"
				))
			){
				ActivateMarkerUI();
			}
			if(GUI.Button(
				sTL_5,
				"DeactivateMarkerUI"
			)){
				DeactivateMarkerUI();
			}
		}
		
		/* Left */
			void ActivateFlyingTarget(){
				IFlyingTarget flyingTarget = (IFlyingTarget)flyingTargetAdaptor.GetShootingTarget();
				flyingTarget.Activate();
				thisFlyingTargetIsReady = true;
			}
			bool thisFlyingTargetIsReady = false;
			void ActivateGlidingTarget(){
				// IWaypointsFollower follower = glidingTargetWPFollowerAdaptor.GetWaypointsFollower();
				// follower.StartFollowing();
				IGlidingTarget target = (IGlidingTarget)glidingTargetAdaptor.GetShootingTarget();
				target.Activate();
			}
			void DeactivateGlidingTarget(){
				IGlidingTarget target = (IGlidingTarget)glidingTargetAdaptor.GetShootingTarget();
				target.Deactivate();
			}
			void FinalizeWaypointCurves(){
				IWaypointsManager waypointsManager = waypointsManagerAdaptor.GetWaypointsManager();
				waypointsManager.PlaceWaypointCurves();
			}
			void ActivateMarkerUI(){
				IMarkerUI markerUI  = markerUIAdaptor.GetSceneUI() as IMarkerUI;
				markerUI.Activate();
			}
			void DeactivateMarkerUI(){
				IMarkerUI markerUI  = markerUIAdaptor.GetSceneUI() as IMarkerUI;
				markerUI.Deactivate();
			}
		/* Right */
			void DrawFlyingTarget(Rect rect){
				if(thisFlyingTargetIsReady){
					IFlyingTarget target = flyingTargetAdaptor.GetShootingTarget() as IFlyingTarget;
					int[] waypointsInSeqIndices = target.GetWaypointsInSequenceIndices();
					int[] waypointsNotInUseIndices = target.GetWaypointsNotInUseIndices();
					int currentWaypointIndex = waypointsInSeqIndices[0];
					float curDist = target.GetCurrentDist();
					Rect sub0 = GetSubRect(rect, 0, 4);
					Rect sub1 = GetSubRect(rect, 1, 4);
					Rect sub2 = GetSubRect(rect, 2, 4);
					Rect sub3 = GetSubRect(rect, 3, 4);
					GUI.Label(
						sub0,
						"in seq: " + GetIndicesString(waypointsInSeqIndices)
					);
					GUI.Label(
						sub1,
						"not in use: " + GetIndicesString(waypointsNotInUseIndices)
					);
					GUI.Label(
						sub2,
						"current: " + currentWaypointIndex.ToString()
					);
					GUI.Label(
						sub3,
						"dist: " + curDist.ToString()
					);
				}
			}
			string GetIndicesString(int[] indices){
				string result = "";
				foreach(int i in indices){
					result += i.ToString() + ", ";
				}
				return result;
			}
		/*  */
	}
}
