using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public class GUIManager2 : AbsGUIManager {

		public void Awake(){
			topLeft = GetGUIRect(
				normalizedSize: new Vector2(.2f, .5f),
				normalizedPosition: new Vector2(0f, 0f)
			);
			sTL_1 = GetSubRect( topLeft, 0, topLeftRectCount);
			sTL_2 = GetSubRect( topLeft, 1, topLeftRectCount);
			sTL_3 = GetSubRect( topLeft, 2, topLeftRectCount);
			sTL_4 = GetSubRect( topLeft, 3, topLeftRectCount);
			sTL_5 = GetSubRect( topLeft, 4, topLeftRectCount);
			sTL_6 = GetSubRect( topLeft, 5, topLeftRectCount);
			sTL_7 = GetSubRect( topLeft, 6, topLeftRectCount);
			sTL_8 = GetSubRect( topLeft, 7, topLeftRectCount);
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
		int topLeftRectCount = 8;
		Rect sTL_1;
		Rect sTL_2;
		Rect sTL_3;
		Rect sTL_4;
		Rect sTL_5;
		Rect sTL_6;
		Rect sTL_7;
		Rect sTL_8;

		Rect topRight;
		Rect sTR_1;
		Rect sTR_2;
		Rect sTR_3;
		public MonoBehaviourAdaptorManager monoBehaviourAdaptorManager;
		public FlyingTargetReserveAdaptor flyingTargetReserveAdaptor;
		public FlyingTargetWaypointManagerAdaptor flyingTargetWaypointManagerAdaptor;
		/*  */
		bool thisSystemIsReady = false;
		public void OnGUI(){
			DrawControl(sTL_1);
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
			}
		}
		
		/* Left */
			void ActivateFlyingTarget(){
				IFlyingTargetReserve reserve = flyingTargetReserveAdaptor.GetFlyingTargetReserve();
				IFlyingTargetWaypointManager manager = flyingTargetWaypointManagerAdaptor.GetFlyingTargetWaypointManager();
				IFlyingTargetWaypoint[] waypoints = manager.GetWaypoints();

				reserve.ActivateFlyingTargetAt(waypoints);
				thisFlyingTargetIsReady = true;
			}
			bool thisFlyingTargetIsReady = false;

		/* Right */
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
