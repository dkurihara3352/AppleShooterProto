using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public class GUIManager2 : AbsGUIManager {

		public void Awake(){
			topLeft = GetGUIRect(
				normalizedSize: new Vector2(.2f, .25f),
				normalizedPosition: new Vector2(0f, 0f)
			);
			sTL_1 = GetSubRect(
				topLeft,
				0,
				2
			);
			sTL_2 = GetSubRect(
				topLeft,
				1,
				2
			);
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

		Rect topRight;
		Rect sTR_1;
		Rect sTR_2;
		Rect sTR_3;
		public MonoBehaviourAdaptorManager monoBehaviourAdaptorManager;
		public FlyingTargetAdaptor flyingTargetAdaptor;
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
				thisSystemIsReady = true;
			}
			if(GUI.Button(
				sTL_2,
				"Run"
			)){
				StartFlyingTargetFlight();
			}
		}
		void StartFlyingTargetFlight(){
			IFlyingTarget flyingTarget = (IFlyingTarget)flyingTargetAdaptor.GetShootingTarget();
			flyingTarget.StartFlight();
			thisFlyingTargetIsReady = true;
		}
		bool thisFlyingTargetIsReady = false;
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
	}
}
