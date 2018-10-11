﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public class GUIManager : MonoBehaviour {
		void Awake(){
			topLeftRect = GetGUIRect(
				normalizedSize: new Vector2(.2f, .4f),
				normalizedPosition: new Vector2(0f ,0f)
			);
			sTL_1 = GetSubRect(topLeftRect,0,6);
			sTL_2 = GetSubRect(topLeftRect,1,6);
			sTL_3 = GetSubRect(topLeftRect,2,6);
			sTL_4 = GetSubRect(topLeftRect,3,6);
			sTL_5 = GetSubRect(topLeftRect,4,6);
			sTL_6 = GetSubRect(topLeftRect,5,6);
			topRightRect = GetGUIRect(
 				normalizedSize: new Vector2(.3f, .5f),
				normalizedPosition: new Vector2(1f, 0f)
			);
			sTR_1 = GetSubRect(topRightRect, 0, 4);
			sTR_2 = GetSubRect(topRightRect, 1, 4);
			sTR_3 = GetSubRect(topRightRect, 2, 4);
			sTR_4 = GetSubRect(topRightRect, 3, 4);

			buttomLeftRect = GetGUIRect(
				normalizedSize: new Vector2(.35f, .6f),
				normalizedPosition: new Vector2(0f, 1f)
			);
		}

		Rect topLeftRect; 
		Rect sTL_1; 
		Rect sTL_2; 
		Rect sTL_3; 
		Rect sTL_4; 
		Rect sTL_5; 
		Rect sTL_6;

		Rect buttomLeftRect;

		Rect topRightRect;
		Rect sTR_1;
		Rect sTR_2;
		Rect sTR_3;
		Rect sTR_4;

		public ProtoGameManager gameManager;
		public bool drawsWaypointFollowerSetUp = true;
		public UISystem.UIManagerAdaptor uiManagerAdaptor;
		public PlayerInputManagerAdaptor playerInputManagerAdaptor;
		public CoreGameplayInputScrollerAdaptor inputScrollerAdaptor;
		public LaunchPointAdaptor launchPointAdaptor;
		public ShootingManagerAdaptor shootingManagerAdaptor;
		public WaypointsFollowerAdaptor waypointsFollowerAdaptor;
		public WaypointsManagerAdaptor waypointsManagerAdaptor;
		void OnGUI(){
			/* left */
				DrawControl();
				DrawArrowsState();
			/* right */
				DrawCurrentState(sTR_1);
				// DrawScrollMultiplier();
				// DrawLaunchAngle();
				// DrawFlightSpeed();
				DrawWaypointsFollower(sTR_2);
				DrawCurveSequence(sTR_3);
				DrawSpawnIndices(sTR_4);
		}
		/* left */
			void DrawControl(){
				if(GUI.Button(
					sTL_1,
					"SetUp"
				)){
					gameManager.SetUp();

				}

				if(GUI.Button(
					sTL_2,
					"Run"
				)){
					gameManager.RunSystem();			
					thisSystemIsReady = true;
				}
			}
			void DrawArrowsState(){
				if(thisSystemIsReady){
					IShootingManager shootingManager = shootingManagerAdaptor.GetShootingManager();
					IArrow[] arrows = shootingManager.GetAllArrows();
					foreach(IArrow arrow in arrows){
						Rect guiSubRect = GetSubRect(
							buttomLeftRect, 
							arrow.GetIndex(), 
							arrows.Length
						);
						GUI.Label(
							guiSubRect,
							"id: " + arrow.GetIndex() + ", " +
							"state: " + GetArrowStateString(arrow) + ", " +
							"position: " + arrow.GetPosition().ToString() + ", " +
							"parent : " + arrow.GetParentName()
						);
					}
				}
			}
			string GetArrowStateString(IArrow arrow){
				IArrowState state = arrow.GetCurrentState();
				string result = state.GetName();
				if(!(state is IArrowNockedState)){
					if(state is IArrowShotState)
						result += ": " + arrow.GetFlightID();
					else
						result += ": " + arrow.GetIDInReserve();
				}
				return result;
			}
		/* right */
			bool thisGroupSequenceIsReady = false;
			void DrawCurveSequence(Rect rect){
				if(thisSystemIsReady){
					IWaypointsManager waypointsManager = waypointsManagerAdaptor.GetWaypointsManager();
					GUI.Label(
						rect, 
						"current: " + gameManager.GetCurrentWaypointGroupIndex().ToString() + " ,\n" +
						"sequence: " + GetSequenceIndexString() + ",\n" + 
						"reserved: " + GetReservedCurvesIDString(waypointsManager.GetReservedCurvesIDs()) + ",\n" +
						"idInSQ: " + waypointsManager.GetCurrentCurveIDInSequence().ToString() + ", \n" +
						"eventCount: " + waypointsManager.GetWaypointCurvesInSequence()[waypointsManager.GetCurrentCurveIDInSequence()].GetWaypointEvents().Count.ToString()
					);
				}
			}
			string GetSequenceIndexString(){
				int[] indexes = gameManager.GetCurrentGroupSequence();
				string result = "";
				foreach(int index in indexes)
					result += index.ToString() + ",";
				return result;
			}
			string GetReservedCurvesIDString(int[] ids){
				string result = "";
				foreach(int id in ids){
					result += id.ToString() + ", ";
 				}
				 return result;
			}
			bool thisSystemIsReady = false;
			void DrawCurrentState(Rect rect){
				if(thisSystemIsReady){
					string stateName = playerInputManagerAdaptor.GetStateName();
					GUI.Label(
						rect,
						"CurState: " + stateName
					);
				}
			}
			void DrawScrollMultiplier(){
				if(thisSystemIsReady){
					float scrollMultiplier = inputScrollerAdaptor.GetScrollMultiplier();
					GUI.Label(
						sTR_2,
						"Scroll Mult: " + scrollMultiplier.ToString()
					);
				}
			}
			void DrawFlightSpeed(){
				if(thisSystemIsReady){
					IShootingManager manager = shootingManagerAdaptor.GetShootingManager();
					float flightSpeed = manager.GetFlightSpeed();
					GUI.Label(
						sTR_2,
						"flightSpeed: " + flightSpeed.ToString()
					);
				}
			}
			void DrawLaunchAngle(){
				if(thisSystemIsReady){
					GUI.Label(
						sTR_3,
						"LaunchDirection: " + 
						launchPointAdaptor.GetWorldForwardDirection().ToString()
					);
				}
			}
			void DrawWaypointsFollower(Rect rect){
				if(thisSystemIsReady){
					IWaypointsFollower follower = waypointsFollowerAdaptor.GetWaypointsFollower();
					int currentCurveIndex = follower.GetCurrentWaypointCurveIndex();
					float normalizedPosInCurve = follower.GetNormalizedPositionInCurve();

					GUI.Label(
						rect,
						"curveID: " + currentCurveIndex.ToString() + ", " +
						"normPos: " + normalizedPosInCurve.ToString()
					);
				}
			}
			void DrawSpawnIndices(Rect rect){
				if(thisSystemIsReady){
					IWaypointsManager waypointsManager = waypointsManagerAdaptor.GetWaypointsManager();
					IWaypointCurve currentCurve = waypointsManager.GetCurrentCurve();
					int[] indices = currentCurve.GetSpawnIndices();

					string indicesString = "";
					foreach(int i in indices){
						indicesString += ", " + i.ToString();
					}
					GUI.Label(
						rect,
						indicesString
					);
				}

				
			}
		/*  */
		Rect GetGUIRect(
			Vector2 normalizedSize,
			Vector2 normalizedPosition
		){
			Vector2 size = new Vector2(
				Screen.width * normalizedSize.x,
				Screen.height * normalizedSize.y
			);
			Vector2 position = new Vector2(
				normalizedPosition.x * (Screen.width - size.x),
				normalizedPosition.y * (Screen.height - size.y)
			);
			return new Rect(
				position, 
				size
			);
		}
		Rect GetSubRect(
			Rect guiRect,
			int index,
			int totalCount
		){
			Vector2 subRectSize = new Vector2(
				guiRect.width,
				guiRect.height /totalCount
			);
			Vector2 subRectPosition = new Vector2(
				guiRect.x,
				guiRect.y + (index * subRectSize.y)
			);
			Rect result = new Rect(
				subRectPosition,
				subRectSize
			);
			return result;
		}
	}
}
