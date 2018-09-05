using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public class GUIManager : MonoBehaviour {
		void Awake(){
			topLeftRect = GetGUIRect(
				normalizedSize: new Vector2(.2f, .2f),
				normalizedPosition: new Vector2(0f ,0f)
			);
			sTL_1 = GetSubRect(topLeftRect,0,4);
			sTL_2 = GetSubRect(topLeftRect,1,4);
			sTL_3 = GetSubRect(topLeftRect,2,4);
			sTL_4 = GetSubRect(topLeftRect,3,4);
			topRightRect = GetGUIRect(
 				normalizedSize: new Vector2(.1f, .1f),
				normalizedPosition: new Vector2(1f, 0f)
			);
			sTR_1 = GetSubRect(topRightRect, 0, 2);
			sTR_2 = GetSubRect(topRightRect, 1, 2);
		}
		Rect topLeftRect; Rect sTL_1; Rect sTL_2; Rect sTL_3; Rect sTL_4;
		Rect topRightRect; Rect sTR_1; Rect sTR_2;
		public ProtoGameManager gameManager;
		public bool drawsWaypointFollowerSetUp = true;

		void OnGUI(){
			DrawWaypointFollowerSetUp();
			DrawGroupSequence();
		}

		void DrawWaypointFollowerSetUp(){
			if(drawsWaypointFollowerSetUp){
				if(GUI.Button(
					sTL_1,
					"SetUpFollwer"
				))
					gameManager.SetUpFollowerAndWithManager();
				if(GUI.Button(
					sTL_2,
					"PlaceWaypointGroups"
				))
					gameManager.PlaceWaypointGroups();
				if(GUI.Button(
					sTL_3,
					"SetUpSmoothFollower"
				)){
					gameManager.SetUpSmoothFollower();
				}
				if(GUI.Button(
					sTL_4,
					"StartFollowing"
				)){
					gameManager.StartFollowerFollow();
					thisGroupSequenceIsReady = true;
				}
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
