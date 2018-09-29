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
 				normalizedSize: new Vector2(.3f, .3f),
				normalizedPosition: new Vector2(1f, 0f)
			);
			sTR_1 = GetSubRect(topRightRect, 0, 2);
			sTR_2 = GetSubRect(topRightRect, 1, 2);
			// sTR_3 = GetSubRect(topRightRect, 2, 3);

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

		public ProtoGameManager gameManager;
		public bool drawsWaypointFollowerSetUp = true;
		public UISystem.UIManagerAdaptor uiManagerAdaptor;
		public PlayerInputManagerAdaptor playerInputManagerAdaptor;
		public CoreGameplayInputScrollerAdaptor inputScrollerAdaptor;
		public LaunchPointAdaptor launchPointAdaptor;
		public ShootingManagerAdaptor shootingManagerAdaptor;
		public EventReferencePointAdaptor eventReferencePointAdaptor;
		void OnGUI(){
			/* left */
				DrawControl();
				DrawArrowsState();
			/* right */
				DrawCurrentState(sTR_1);
				// DrawScrollMultiplier();
				// DrawLaunchAngle();
				// DrawFlightSpeed();
				DrawEventReferencePoint(sTR_2);
		}

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
		bool thisGroupSequenceIsReady = false;
		void DrawGroupSequence(){
			if(thisGroupSequenceIsReady){
				GUI.Label(
					sTR_1, 
					"current: " + gameManager.GetCurrentWaypointGroupIndex().ToString()
				);
				GUI.Label(
					sTR_2,
					"sequence: " + GetSequenceIndexString()
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
		void DrawEventReferencePoint(Rect rect){
			if(thisSystemIsReady){
				IEventReferencePoint refPoint = eventReferencePointAdaptor.GetEventReferencePoint();
				int groupIndex = refPoint.GetCurrentWaypointGroupIndex();
				float normalizedPos = refPoint.GetNormalizedPositionInGroup();
				GUI.Label(
					rect,

					"group: " + groupIndex.ToString() + ", " +
					"normalizedPos: " + normalizedPos.ToString() + ", " + 
					"normPOnSeg: " + refPoint.GetNormalizedPositionOnSegment().ToString()
				);
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
