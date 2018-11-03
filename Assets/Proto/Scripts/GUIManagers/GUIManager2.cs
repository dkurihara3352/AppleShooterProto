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
		public FlyingTargetWaypointManagerAdaptor flyingTargetWaypointManagerAdaptor;
		/*  */
		bool thisSystemIsReady = false;
		public void OnGUI(){
			DrawControl(sTL_1);
			DrawStaticTargetSpawnStatus(bottomLeft);
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
				// ActivateFlyingTarget();
				// ActivateNextGlidingTargetAtWaypointsManager();
				// TestPool();
				ActivateGlidingTargetAtDrawnWaypointsManager();
				// ActivateStaticShootingTargetAtDrawnSpawnPoint();
			}
			if(GUI.Button(
				sTL_3,
				"ShowDeactivateControl"
			)){
				ToggleShowDeactivateControl();
			}
				ShowDeactivateControl(sTL_4);
		}
		
		/* Left */
			void ActivateFlyingTarget(){
				IFlyingTargetReserve reserve = flyingTargetReserveAdaptor.GetFlyingTargetReserve();
				IFlyingTargetWaypointManager manager = flyingTargetWaypointManagerAdaptor.GetFlyingTargetWaypointManager();

				reserve.ActivateFlyingTargetAt(manager);
				thisFlyingTargetIsReady = true;
			}
			bool thisFlyingTargetIsReady = false;

			public ArrowReserveAdaptor arrowReserveAdaptor;
			void ActivateArrow(){
				IArrowReserve reserve = arrowReserveAdaptor.GetArrowReserve();
				IArrow arrow = reserve.GetNextArrow();
				arrow.Nock();
			}
			public GlidingTargetWaypointCurveAdaptor glidingTargetWaypointCurveAdaptor;
			public GlidingTargetReserveAdaptor glidingTargetReserveAdaptor;
			void ActivateNextGlidingTargetAtWaypointsManager(){
				IGlidingTargetWaypointCurve curve = glidingTargetWaypointCurveAdaptor.GetGlidingTargetWaypointCurve();
				IGlidingTargetReserve reserve = glidingTargetReserveAdaptor.GetGlidingTargetReserve();
				
				reserve.ActivateGlidingTargetAt(curve);
			}
			public WaypointsManagerPoolAdaptor waypointsManagerPoolAdaptor;
			public void TestPool(){
				ISceneObjectPool<IWaypointCurveCycleManager> pool = waypointsManagerPoolAdaptor.GetSceneObjectPool();
				pool.Draw();
				pool.Log();
			}
			public GlidingTargetWaypointCurvePoolAdaptor glidingTargetWaypointCurvePoolAdaptor;
			public void ActivateGlidingTargetAtDrawnWaypointsManager(){
				ISceneObjectPool<IGlidingTargetWaypointCurve> curvePool = glidingTargetWaypointCurvePoolAdaptor.GetSceneObjectPool();
				IGlidingTargetWaypointCurve curve = curvePool.Draw();
				IGlidingTargetReserve targetReserve = glidingTargetReserveAdaptor.GetGlidingTargetReserve();
				targetReserve.ActivateGlidingTargetAt(curve);

				curvePool.Log();
			}
			public ShootingTargetSpawnPointPoolAdaptor shootingTargetSpawnPointPoolAdaptor;
			public StaticShootingTargetReserveAdaptor staticShootingTargetReserveAdaptor;
			public void ActivateStaticShootingTargetAtDrawnSpawnPoint(){
				ISceneObjectPool<IShootingTargetSpawnPoint> pool = shootingTargetSpawnPointPoolAdaptor.GetSceneObjectPool();
				IShootingTargetSpawnPoint spawnPoint = pool.Draw();

				IStaticShootingTargetReserve reserve = staticShootingTargetReserveAdaptor.GetStaticShootingTargetReserve();
				reserve.ActivateStaticShootingTargetAt(spawnPoint);

				pool.Log();
			}
			bool thisShowsDeactivateControl = false;
			void ToggleShowDeactivateControl(){
				thisShowsDeactivateControl = !thisShowsDeactivateControl;
			}
			public void ShowDeactivateControl(Rect rect){
				if(thisSystemIsReady){
					if(thisShowsDeactivateControl){
						IStaticShootingTargetReserve reserve = staticShootingTargetReserveAdaptor.GetStaticShootingTargetReserve();
						IStaticShootingTarget[] targets = reserve.GetStaticShootingTargets();
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
			public ShootingTargetSpawnPointGroupAdaptor shootingTargetSpawnPointGroupAdaptor;
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
			void DrawTargetsBySpawnPoints(Rect rect){
				if(thisSystemIsReady){
					IShootingTargetSpawnPointGroup group  = shootingTargetSpawnPointGroupAdaptor.GetShootingTargetSpawnPointGroup();
					IShootingTargetSpawnPoint[] spawnPoints = group.GetShootingTargetSpawnPoints();
					string result = "";
					int indexOfPoint = 0;
					foreach(IShootingTargetSpawnPoint point in spawnPoints){
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
					IStaticShootingTargetReserve reserve = staticShootingTargetReserveAdaptor.GetStaticShootingTargetReserve();
					IStaticShootingTarget[] targets = reserve.GetStaticShootingTargets();
					string result  = "";
					foreach(IStaticShootingTarget target in targets){
						IShootingTargetSpawnPoint spawnPoint = target.GetShootingTargetSpawnPoint();
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
