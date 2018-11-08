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
			sTR_1 = GetSubRect( topRight, 0, 3);
			sTR_2 = GetSubRect( topRight, 1, 3);
			sTR_3 = GetSubRect( topRight, 2, 3);

			bottomLeft = GetGUIRect(
				normalizedSize: new Vector2(.2f, .5f),
				normalizedPosition: new Vector2(0f, 1f)
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

		Rect bottomLeft;




		Rect topRight;
		Rect sTR_1;
		Rect sTR_2;
		Rect sTR_3;
		public MonoBehaviourAdaptorManager monoBehaviourAdaptorManager;
		public FlyingTargetReserveAdaptor flyingTargetReserveAdaptor;
		/*  */
		bool thisSystemIsReady = false;
		public void OnGUI(){
			DrawControl(sTL_1);
			// DrawStaticTargetSpawnStatus(bottomLeft);
			// DrawGlidingTargetSpawnStatus(bottomLeft);
			// DrawDestroyedTargetSpawnStatus(bottomLeft);
			// DrawPopUISpawnStatus(bottomLeft);
			DrawLandedArrowSpawnStatus(bottomLeft);
		}
		public void Update(){
			// if(Input.GetKeyDown("space"))
			// 	StopAllDestroyedTargetParticleSystems();
		}
		void SetUpRefs(){
			thisFlyingTargetReserve = flyingTargetReserveAdaptor.GetFlyingTargetReserve();
			thisFlyingTargetSpawnPoint = flyingTargetSpawnPointAdaptor.GetFlyingTargetSpawnPoint();
			thisArrowReserve = arrowReserveAdaptor.GetArrowReserve();
			thisGlidingTargetWaypointCurve  = glidingTargetWaypointCurveAdaptor.GetGlidingTargetWaypointCurve();
			thisGlidingTargetReserve = glidingTargetReserveAdaptor.GetGlidingTargetReserve();
			thisStaticShootingTargetReserve = staticShootingTargetReserveAdaptor.GetStaticShootingTargetReserve();
			thisStaticTargetSpawnPointGroup = staticTargetSpawnPointGroupAdaptor.GetSpawnPointGroup();
			thisDestroyedTargetReserve = destroyedTargetReserveAdaptor.GetDestroyedTargetReserve();
			thisPopUIReserve = popUIReserveAdaptor.GetPopUIReserve();
			thisLandedArrowReserve = landedArrowReserveAdaptor.GetLandedArrowReserve();
			thisGlidingTargetSpawnPoint = glidingTargetSpawnPointAdaptor.GetGlidingTargetSpawnPoint();
			thisGlidingTargetSpawnPointGroup = glidingTargetSpawnPointGroupAdaptor.GetGroup();
		}
		IFlyingTargetReserve thisFlyingTargetReserve;

		void DrawControl(Rect rect){
			if(GUI.Button(
				sTL_1,
				"SetUp"
			)){
				monoBehaviourAdaptorManager.SetUpAllMonoBehaviourAdaptors();
				monoBehaviourAdaptorManager.SetUpAdaptorReference();
				monoBehaviourAdaptorManager.FinalizeSetUp();
				thisSystemIsReady = true;

				SetUpRefs();
			}
			if(GUI.Button(
				sTL_2,
				"Run"
			)){
				ActivatePopUIAtDrawnShootingTarget();

				// done
				// ActivateFlyingTarget();
				// ActivateNextGlidingTargetAtSpawnPoint();
				// ActivateGlidingTargetAtDrawnSpawnPoint();
				// ActivateStaticShootingTargetAtDrawnSpawnPoint();
				// ActivateDestroyedTargetAtDrawnShootingTarget();

				// ActivateLandedArrowAtDrawnShootingTarget();
			}
		}
		
		/* Left */
			public FlyingTargetSpawnPointAdaptor flyingTargetSpawnPointAdaptor;
			IFlyingTargetSpawnPoint thisFlyingTargetSpawnPoint;
			void ActivateFlyingTarget(){

				thisFlyingTargetReserve.ActivateShootingTargetAt(thisFlyingTargetSpawnPoint);
				thisFlyingTargetIsReady = true;
			}
			bool thisFlyingTargetIsReady = false;

			public ArrowReserveAdaptor arrowReserveAdaptor;
			IArrowReserve thisArrowReserve;
			void ActivateArrow(){
				IArrow arrow = thisArrowReserve.GetNextArrow();
				arrow.Nock();
			}
			public GlidingTargetWaypointCurveAdaptor glidingTargetWaypointCurveAdaptor;
			public GlidingTargetReserveAdaptor glidingTargetReserveAdaptor;
			IGlidingTargetWaypointCurve thisGlidingTargetWaypointCurve;
			IGlidingTargetReserve thisGlidingTargetReserve;
			IGlidingTargetSpawnPoint thisGlidingTargetSpawnPoint;
			public GlidingTargetSpawnPointAdaptor glidingTargetSpawnPointAdaptor;
			void ActivateNextGlidingTargetAtSpawnPoint(){
				
				thisGlidingTargetReserve.ActivateShootingTargetAt(thisGlidingTargetSpawnPoint);
			}


			public void ActivateGlidingTargetAtDrawnSpawnPoint(){
				IGlidingTargetSpawnPoint drawnPoint = thisGlidingTargetSpawnPointGroup.Draw();
				thisGlidingTargetReserve.ActivateShootingTargetAt(drawnPoint);

				thisGlidingTargetSpawnPointGroup.Log();
			}
			public StaticShootingTargetReserveAdaptor staticShootingTargetReserveAdaptor;
			IStaticShootingTargetReserve thisStaticShootingTargetReserve;
			public void ActivateStaticShootingTargetAtDrawnSpawnPoint(){
				IStaticTargetSpawnPoint spawnPoint = thisStaticTargetSpawnPointGroup.Draw();

				thisStaticShootingTargetReserve.ActivateShootingTargetAt(spawnPoint);

				thisStaticTargetSpawnPointGroup.Log();
			}
			bool thisShowsDeactivateControl = false;
			void ToggleShowDeactivateControl(){
				thisShowsDeactivateControl = !thisShowsDeactivateControl;
			}
			public void ShowDeactivateControl(Rect rect){
				if(thisSystemIsReady){
					if(thisShowsDeactivateControl){
						IStaticShootingTarget[] targets = thisStaticShootingTargetReserve.GetStaticShootingTargets();
						for(int i = 0; i < targets.Length; i ++){
							Rect subRect = GetHorizontalSubRect(
								rect,
								i,
								targets.Length
							);
							if(GUI.Button(
								subRect,
								i.ToString()
							)){
								targets[i].Deactivate();
							}
						}
					}
				}
			}
			public StaticTargetSpawnPointGroupAdaptor staticTargetSpawnPointGroupAdaptor;
			void DrawStaticTargetSpawnStatus(Rect rect){
				Rect subTop = GetSubRect(
					rect,
					0,
					2
				);
				Rect subBottom = GetSubRect(
					rect,
					1,
					2
				);
				DrawTargetsBySpawnPoints(subTop);
				DrawSpawnPointForEachTarget(subBottom);
			}
			IStaticTargetSpawnPointGroup thisStaticTargetSpawnPointGroup;
			void DrawTargetsBySpawnPoints(Rect rect){
				if(thisSystemIsReady){
					IStaticTargetSpawnPoint[] spawnPoints = thisStaticTargetSpawnPointGroup.GetShootingTargetSpawnPoints();
					string result = "";
					int indexOfPoint = 0;
					foreach(IStaticTargetSpawnPoint point in spawnPoints){
						IShootingTarget assignedTarget = point.GetSpawnedTarget();
						result += 
						"spawnPoint #" + indexOfPoint.ToString() + ": ";
						if(assignedTarget == null)
							result += " null ";
						else
							result += "target # " + assignedTarget.GetIndex().ToString();
						
						result += "\n";
						indexOfPoint ++;
					}
					GUI.Label(
						rect,
						result
					);
				}
			}
			void DrawSpawnPointForEachTarget(Rect rect){
				if(thisSystemIsReady){
					IStaticShootingTarget[] targets = thisStaticShootingTargetReserve.GetStaticShootingTargets();
					string result  = "";
					foreach(IStaticShootingTarget target in targets){
						IStaticTargetSpawnPoint spawnPoint = target.GetShootingTargetSpawnPoint();
						result += "target # " + target.GetIndex().ToString() + ", ";
						if(spawnPoint == null)
							result += "null";
						else
							result += spawnPoint.GetIndex().ToString();
						
						result += "\n";
					}
					GUI.Label(
						rect,
						result
					);
				}
			}
			void DrawGlidingTargetSpawnStatus(Rect rect){
				Rect subTop  = GetSubRect(
					rect,
					0,
					2
				);
				Rect subBottom = GetSubRect(
					rect,
					1,
					2
				);
				DrawGlidingTargetsByWaypointCurve(subTop);
				DrawWaypointsByGlidingTarget(subBottom);
			}
			public GlidingTargetSpawnPointGroupAdaptor glidingTargetSpawnPointGroupAdaptor;
			IGlidingTargetSpawnPointGroup thisGlidingTargetSpawnPointGroup;
			void DrawGlidingTargetsByWaypointCurve(Rect rect){
				if(thisSystemIsReady){
					IGlidingTargetSpawnPoint[] spawnPoints = thisGlidingTargetSpawnPointGroup.GetSpawnPoints();
					string result = "";
					foreach(IGlidingTargetSpawnPoint spawnPoint in spawnPoints){
						IGlidingTarget target = (IGlidingTarget)spawnPoint.GetSpawnedTarget();
						result += "point #" + spawnPoint.GetIndex().ToString() + ": "
						;
						if(target == null) 
							result += "null";
						else
							result += "target# " + target.GetIndex().ToString();
						result += "\n";
					}
					GUI.Label(
						rect,
						result
					);
				}
			}
			void DrawWaypointsByGlidingTarget(Rect rect){
				if(thisSystemIsReady){
					IGlidingTarget[] targets = thisGlidingTargetReserve.GetGlidingTargets();
					string result = "";
					foreach(IGlidingTarget target in targets){
						IGlidingTargetWaypointCurve curve  = target.GetGlidingTargetWaypointCurve();
						result += "target # " + target.GetIndex() + " ";
						if(curve == null)
							result += "null";
						else
							result += "curve# " + curve.GetIndex().ToString();
						result += "\n";
					}
					GUI.Label(
						rect,
						result
					);
				}
			}
			bool thisStaticTargetsAreSet = false;
			public DestroyedTargetReserveAdaptor destroyedTargetReserveAdaptor;
			IDestroyedTargetReserve thisDestroyedTargetReserve;
			IPool thisIndexPool;
			bool thisIndexPoolIsSet = false;
			void ActivateDestroyedTargetAtDrawnShootingTarget(){
				if(!thisStaticTargetsAreSet)
					SetStaticShootingTargetsAtSpawnPoints();
				if(!thisIndexPoolIsSet)
					SetIndexPool();
				IStaticShootingTarget[] targets = thisStaticShootingTargetReserve.GetStaticShootingTargets();

				int index = thisIndexPool.Draw();

				IStaticShootingTarget target = targets[index];
				
				thisDestroyedTargetReserve.ActivateDestoryedTargetAt(target);
			}
			void SetStaticShootingTargetsAtSpawnPoints(){
				IStaticTargetSpawnPoint[] points = thisStaticTargetSpawnPointGroup.GetShootingTargetSpawnPoints();

				IStaticShootingTarget[] targets = thisStaticShootingTargetReserve.GetStaticShootingTargets();
				int targetCount = targets.Length;

				for(int i = 0; i < targetCount; i ++){
					IStaticTargetSpawnPoint point = points[i];
					thisStaticShootingTargetReserve.ActivateShootingTargetAt(point);
				}
				thisStaticTargetsAreSet = true;
			}
			void SetIndexPool(){
				IStaticShootingTarget[] targets = thisStaticShootingTargetReserve.GetStaticShootingTargets();

				List<float> probList = new List<float>();
				for(int i = 0; i < targets.Length; i ++){
					probList.Add(1f);
				}
				float[] probTable = probList.ToArray();
				
				thisIndexPool = new Pool(
					probTable
				);
				thisIndexPoolIsSet = true;
			}
			void DrawDestroyedTargetSpawnStatus(Rect rect){
				if(thisSystemIsReady){
					Rect subTop = GetSubRect(
						rect,
						0,
						2
					);
					Rect subBottom = GetSubRect(
						rect,
						1,
						2
					);
					DrawShootingTargetByDestroyedTarget(subTop);
					DrawDestroyedTargetByShootingTarget(subBottom);
				}
			}
			void DrawShootingTargetByDestroyedTarget(Rect rect){
				IDestroyedTarget[] destroyedTargets = thisDestroyedTargetReserve.GetTargets();

				string result = "";
				foreach(IDestroyedTarget dTarget in destroyedTargets){
					result += "dTarget #" + dTarget.GetIndex().ToString() + ", ";
					IShootingTarget sTarget = dTarget.GetShootingTarget();
					if(sTarget == null)
						result += "null";
					else
						result += sTarget.GetIndex().ToString();
					result += "\n";
				}
				GUI.Label(
					rect,
					result
				);
			}
			void DrawDestroyedTargetByShootingTarget(Rect rect){
				IStaticShootingTarget[] sTargets = thisStaticShootingTargetReserve.GetStaticShootingTargets();

				string result = "";
				foreach(IStaticShootingTarget sTarget in sTargets){
					result += "sTarget #" + sTarget.GetIndex().ToString() + ", ";
					IDestroyedTarget dTarget = sTarget.GetDestroyedTarget();
					if(dTarget == null)
						result += "null";
					else
						result += dTarget.GetIndex();
					result += "\n";
				}
				GUI.Label(
					rect,
					result
				);
			}
			void StopAllDestroyedTargetParticleSystems(){
				IDestroyedTarget[] targets = thisDestroyedTargetReserve.GetTargets();
				foreach(IDestroyedTarget target in targets)
					target.StopParticleSystem();
			}
			IPopUIReserve thisPopUIReserve;
			public PopUIReserveAdaptor popUIReserveAdaptor;
			void ActivatePopUIAtDrawnShootingTarget(){
				if(!thisStaticTargetsAreSet)
					SetStaticShootingTargetsAtSpawnPoints();
				if(!thisIndexPoolIsSet)
					SetIndexPool();
				
				IStaticShootingTarget[] targets = thisStaticShootingTargetReserve.GetStaticShootingTargets();
				int index = thisIndexPool.Draw();
				IStaticShootingTarget target = targets[index];

				thisPopUIReserve.PopText(
					target,
					"WTF"
				);
			}
			void DrawPopUISpawnStatus(Rect rect){
				if(thisSystemIsReady){
					DrawTargetByPopUI(rect);
				}
			}
			void DrawTargetByPopUI(Rect rect){
				IPopUI[] uis = thisPopUIReserve.GetPopUIs();
				string result = "";
				foreach(IPopUI ui in uis){
					result += "ui #" + ui.GetIndex().ToString() + ", ";
					ISceneObject obj = ui.GetSceneObject();
					if(obj != null){
						IStaticShootingTarget target = (IStaticShootingTarget)obj;
						result += "target# " + target.GetIndex().ToString();
					}else
						result += "null";
					result += "\n";
				}
				GUI.Label(
					rect,
					result
				);
			}
			ILandedArrowReserve thisLandedArrowReserve;
			public LandedArrowReserveAdaptor landedArrowReserveAdaptor;
			void ActivateLandedArrowAtDrawnShootingTarget(){
				if(!thisStaticTargetsAreSet)
					SetStaticShootingTargetsAtSpawnPoints();
				if(!thisIndexPoolIsSet)
					SetIndexPool();
				
				IStaticShootingTarget[] targets = thisStaticShootingTargetReserve.GetStaticShootingTargets();
				int index = thisIndexPool.Draw();
				IStaticShootingTarget target = targets[index];
				Vector3 spawnPos = target.GetPosition() + (Vector3.forward * -1f);

				thisLandedArrowReserve.ActivateLandedArrowAt(
					target,
					spawnPos,
					Quaternion.identity
				);
			}
			void DrawLandedArrowSpawnStatus(Rect rect){
				if(thisSystemIsReady){
					Rect subTop = GetSubRect(rect, 0, 2);
					Rect subBtm = GetSubRect(rect, 1, 2);
					DrawArrowByTarget(subTop);
					DrawTargetbyArrow(subBtm);
				}
			}
			void DrawArrowByTarget(Rect rect){
				IStaticShootingTarget[] targets = thisStaticShootingTargetReserve.GetStaticShootingTargets();
				string result = "";
				foreach(IStaticShootingTarget target in targets){
					result += "target #" + target.GetIndex().ToString() + ", ";
					ILandedArrow[] landedArrows = target.GetLandedArrows();
					result += GetLandedArrowString(landedArrows);
					result += "\n";
				}
				GUI.Label(
					rect,
					result
				);
			}
			string GetLandedArrowString(ILandedArrow[] arrows){
				string result  =  "";
				foreach(ILandedArrow arrow in arrows){
					if(arrow == null)
						result += "null, ";
					else
						result += arrow.GetIndex().ToString() + ", ";
				}
				return result;
			}
			void DrawTargetbyArrow(Rect rect){
				ILandedArrow[] arrows = thisLandedArrowReserve.GetLandedArrows();
				string result = "";
				foreach(ILandedArrow arrow in arrows){
					result += "arrow #" + arrow.GetIndex().ToString() + ", ";
					IShootingTarget target = arrow.GetShootingTarget();
					if(target == null)
						result += "null";
					else
						result += target.GetIndex().ToString();
					result += "\n";
				}
				GUI.Label(
					rect,
					result
				);
			}
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
