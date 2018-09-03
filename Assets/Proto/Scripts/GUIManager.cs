using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public class GUIManager : MonoBehaviour {

		public ProtoGameManager gameManager;
		public bool drawsWaypointFollowerSetUp = true;

		void OnGUI(){
			DrawWaypointFollowerSetUp();
		}
		void DrawWaypointFollowerSetUp(){
			Rect masterRect = GetGUIRect(
				normalizedSize: new Vector2(.2f, .2f),
				normalizedPosition: new Vector2(0f ,0f)
			);
			Rect subR_1 = GetSubRect(
				masterRect,
				0,
				2
			);
			Rect subR_2 = GetSubRect(
				masterRect,
				1,
				2
			);
			if(drawsWaypointFollowerSetUp){
				if(GUI.Button(
					subR_1,
					"SetUpFollwer"
				))
					gameManager.SetUpFollowerWithWaypoints();
				if(GUI.Button(
					subR_2,
					"StartFollowing"
				))
					gameManager.StartFollowerFollow();
			}
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
