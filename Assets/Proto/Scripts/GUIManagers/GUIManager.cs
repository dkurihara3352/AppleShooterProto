using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public class GUIManager : AbsGUIManager {
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
 				normalizedSize: new Vector2(.5f, .5f),
				normalizedPosition: new Vector2(1f, .5f)
			);
			sTR_1 = GetSubRect(topRightRect, 0, 1);
			// sTR_2 = GetSubRect(topRightRect, 1, 4);
			// sTR_3 = GetSubRect(topRightRect, 2, 4);
			// sTR_4 = GetSubRect(topRightRect, 3, 4);

			bottomLeftRect = GetGUIRect(
				normalizedSize: new Vector2(.5f, .5f),
				normalizedPosition: new Vector2(0f, 1f)
			);
			bottomRightRect = GetGUIRect(
				normalizedSize: new Vector2(.5f, .5f),
				normalizedPosition: new Vector2(1f, 1f)
			);
		}

		Rect topLeftRect; 
		Rect sTL_1; 
		Rect sTL_2; 
		Rect sTL_3; 
		Rect sTL_4; 
		Rect sTL_5; 
		Rect sTL_6;


		Rect bottomLeftRect;

		Rect topRightRect;
		Rect sTR_1;
		Rect sTR_2;
		Rect sTR_3;
		Rect sTR_4;

		Rect bottomRightRect;

		public ProtoGameManager gameManager;
		public PlayerInputManagerAdaptor playerInputManagerAdaptor;
		public CoreGameplayInputScrollerAdaptor inputScrollerAdaptor;
		public LaunchPointAdaptor launchPointAdaptor;
		public ShootingManagerAdaptor shootingManagerAdaptor;
		public WaypointsFollowerAdaptor waypointsFollowerAdaptor;
		public PCWaypointsManagerAdaptor pcWaypointsManagerAdaptor;
		public ArrowReserveAdaptor arrowReserveAdaptor;
		void OnGUI(){
			/* left */
				DrawControl();
				// DrawArrowsState();
				// DrawSpawnedShootingTargets();
				// DrawLandedArrows();
			/* right */
				// DrawCurrentState(sTR_1);
				// DrawScrollMultiplier();
				// DrawLaunchAngle();
				// DrawFlightSpeed();
				// DrawWaypointsFollower(sTR_1);
				// DrawCurveSequence(sTR_2);
				// DrawSpawnIndices(sTR_4);
				// DrawShootingMetrics(sTR_3);
				// DrawWaypointEvents(bottomRightRect);
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
					"WarmUp"
				)){
					gameManager.WarmUp();
					thisSystemIsReady = true;
				}
				// if(GUI.Button(
				// 	sTL_3,
				// 	"StartSpawn"
				// ))
				// 	gameManager.StartTargetSpawn();
				// if(GUI.Button(
				// 	sTL_4,
				// 	"StopSpawn"
				// ))
				// 	gameManager.StopTargetSpawn();
				// if(GUI.Button(
				// 	sTL_3,
				// 	"StartGameplay"
				// ))
				// 	StartGameplay();
				// if(GUI.Button(
				// 	sTL_4,
				// 	"EndGameplay"
				// ))
				// 	EndGameplay();
				if(GUI.Button(
					sTL_3,
					"ActivateRootUIE"
				))
					gameManager.ActivateRootUI();
				if(GUI.Button(
					sTL_4,
					"Start"
				))
					gameManager.StartGameplaySequence();
				if(GUI.Button(
					sTL_5,
					"End"
				))
					gameManager.StartEndGameplaySequence();
				// if(GUI.Button(
				// 	sTL_3,
				// 	"AddHeat"
				// )){
				// 	AddHeat();
				// }

				// if(GUI.Button(
				// 	sTL_4,
				// 	"SmoothStop"
				// )){
				// 	SmoothStopFollower();
				// }
				
				// if(GUI.Button(
				// 	sTL_5,
				// 	"SmoothStart"
				// )){
				// 	SmoothStartFollower();
				// }
				// if(GUI.Button(
				// 	sTL_3,
				// 	"Tier Up"
				// )){
				// 	currentTier++;
				// 	if(currentTier > maxTier)
				// 		currentTier = maxTier;
				// 	SetTierOnAllTargetReserves(currentTier);
				// }
				// if(GUI.Button(
				// 	sTL_4,
				// 	"Tier Down"
				// )){
				// 	currentTier--;
				// 	if(currentTier < 0)
				// 		currentTier = 0;
				// 	SetTierOnAllTargetReserves(currentTier);
				// }

			}
			
			int currentTier = 0;
			int maxTier = 1;
			void DrawArrowsState(){
				if(thisSystemIsReady){
					// IShootingManager shootingManager = shootingManagerAdaptor.GetShootingManager();
					// IArrow[] arrows = shootingManager.GetAllArrows();
					IArrowReserve arrowReserve = arrowReserveAdaptor.GetArrowReserve();
					IArrow[] arrows = arrowReserve.GetArrows();
					foreach(IArrow arrow in arrows){
						Rect guiSubRect = GetSubRect(
							bottomLeftRect, 
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
				ArrowStateEngine.IState state = arrow.GetCurrentState();
				string result = state.GetName();
				IArrowReserve reserve = arrowReserveAdaptor.GetArrowReserve();
				if(!(state is ArrowStateEngine.NockedState)){
					if(state is ArrowStateEngine.FlightState)
						result += ": " + reserve.GetIndexInFlight(arrow);
					else
						result += ": " + reserve.GetIndexInReserve(arrow);
				}
				return result;
			}

		/* right */
			bool thisGroupSequenceIsReady = false;
			void DrawCurveSequence(Rect rect){
				if(thisSystemIsReady){
					IPCWaypointsManager waypointsManager = pcWaypointsManagerAdaptor.GetPCWaypointsManager();
					GUI.Label(
						rect, 
						"current: " + gameManager.GetCurrentWaypointGroupIndex().ToString() + " ,\n" +
						"sequence: " + GetSequenceIndexString() + ",\n" + 
						"reserved: " + GetReservedCurvesIDString(waypointsManager.GetReservedCurvesIDs()) + ",\n" +
						"idInSQ: " + waypointsManager.GetCurrentCurveIDInSequence().ToString() + ", \n" /* +
						"eventCount: " + waypointsManager.GetWaypointCurvesInSequence()[waypointsManager.GetCurrentCurveIDInSequence()].GetWaypointEvents().Count.ToString() */
						
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
						launchPointAdaptor.GetForwardDirection().ToString()
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
			void DrawShootingMetrics(Rect rect){
				if(thisSystemIsReady){
					IShootingManager shootingManager = shootingManagerAdaptor.GetShootingManager();
					float drawTime = shootingManager.GetDrawElapsedTime();
					float drawStrength = shootingManager.GetDrawStrength();
					float arrowAttack = shootingManager.GetArrowAttack();
					float flightSpeed = shootingManager.GetFlightSpeed();
					GUI.Label(
						rect,
						"drawTime: " + drawTime.ToString() + "\n" +
						"drawStrength: " + drawStrength.ToString() + "\n" +
						"arrowAttack: " + arrowAttack.ToString() + "\n" +
						"flightSpeed: " + flightSpeed.ToString() + "\n"
					);
				}
			}
			public GlidingTargetSpawnPointGroupAdaptor glidingTargetSpawnPointGroupAdaptor;
			void CalcCurveOnGliderSpawnPointGroup(){
				IGlidingTargetSpawnPointGroup group = glidingTargetSpawnPointGroupAdaptor.GetGlidingTargetSpawnPointGroup();
				IGlidingTargetSpawnPoint[] spawnPoints = group.GetGlidingTargetSpawnPoints();
				foreach(IGlidingTargetSpawnPoint point in spawnPoints){
					IGlidingTargetWaypointCurve curve = point.GetGlidingTargetWaypointCurve();
					curve.CalculateCurve();
				}
			}
			public HeatManagerAdaptor heatManagerAdaptor;
			void AddHeat(){
				IHeatManager manager = heatManagerAdaptor.GetHeatManager();
				manager.AddHeat(.05f);
			}
			
			void SmoothStopFollower(){
				IWaypointsFollower follower = waypointsFollowerAdaptor.GetWaypointsFollower();
				follower.SmoothStop();
			}
			void SmoothStartFollower(){
				IWaypointsFollower follower = waypointsFollowerAdaptor.GetWaypointsFollower();
				follower.SmoothStart();
			}
			public WaypointCurveCycleManagerAdaptor curveCycleManagerAdaptor;
			void DrawWaypointEvents(Rect rect){
				if(thisSystemIsReady){
					string result = "";
					IWaypointsFollower follower = waypointsFollowerAdaptor.GetWaypointsFollower();
					float normalizedPosOnCurve = follower.GetNormalizedPositionInCurve();
					result += "normPos: " + normalizedPosOnCurve.ToString("N2") + "\n";

					IWaypointCurveCycleManager curveManager = curveCycleManagerAdaptor.GetWaypointsManager();
					IWaypointCurve currentCurve = curveManager.GetWaypointCurvesInSequence()[0];
					IWaypointEvent[] evnets = currentCurve.GetWaypointEvents();
					result += "curve id: " + currentCurve.GetIndex().ToString() + "\n";
					foreach(IWaypointEvent wpEvent in evnets){
						string thisEventString = "\n";
						thisEventString += wpEvent.GetName() + ": " + wpEvent.GetEventPoint().ToString("N2");
						TargetType thisType = TargetType.Flyer;
						if(wpEvent is IShootingTargetSpawnWaypointEvent){
							IShootingTargetSpawnWaypointEvent spawnEvent = (IShootingTargetSpawnWaypointEvent)wpEvent;
							thisType = spawnEvent.GetTargetType();
							IShootingTargetSpawnPoint spawnPoint = spawnEvent.GetSpawnPoint();
							thisEventString += ", sp: " + spawnPoint.GetName();
							IShootingTarget target = spawnPoint.GetSpawnedTarget();
							if(target != null)
								thisEventString += ", tar: " + target.GetName();
							else
								thisEventString += ", tar: null";

						}
						Color col = GetStringColorForType(thisType);
						thisEventString = DKUtility.DebugHelper.StringInColor(thisEventString, col);
						result += thisEventString;
						string executedString = ", ";
						if(!wpEvent.IsExecuted())
							executedString += "pending";
						else
							executedString += DKUtility.DebugHelper.RedString("executed");
						result += executedString;
					}
					GUI.Label(
						rect,
						result
					);
				}
			}
			Color GetStringColorForType(TargetType targetType){
				switch(targetType){
					case TargetType.Static:
						return new Color(0f, .7f, .3f);
					case TargetType.Fatty:
						return new Color(.5f, 1f, 0f);
					case TargetType.Glider:
						return new Color(.3f, 1f, 1f);
					default: return Color.white;
				}
			}
			public AbsShootingTargetReserveAdaptor[] targetReserveAdaptors;
			IShootingTargetReserve[] GetAllReserves(){
				List<IShootingTargetReserve> resultList = new List<IShootingTargetReserve>();
				foreach(AbsShootingTargetReserveAdaptor adaptor in targetReserveAdaptors){
					resultList.Add(adaptor.GetReserve());
				}
				return resultList.ToArray();
			}

			void SetTierOnAllTargetReserves(int tier){
				foreach(IShootingTargetReserve reserve in GetAllReserves()){
					reserve.SetTier(tier);
				}
			}
			// void StartGameplay(){
			// 	gameManager.StartWaitAndStartGameplay();
			// }
			// void EndGameplay(){
			// 	DeactivateInputUI();
			// 	DeactivateHUD();
			// 	gameManager.StopTargetSpawn();
			// }
			// public HeadUpDisplayAdaptor headUpDisplayAdaptor;
			// void ActivateHUD(){
			// 	IHeadUpDisplay hud = headUpDisplayAdaptor.GetHeadUpDisplay();
			// 	hud.Activate();
			// }
			// void DeactivateHUD(){
			// 	IHeadUpDisplay hud = headUpDisplayAdaptor.GetHeadUpDisplay();
			// 	hud.Deactivate();
			// }
			// void ActivateInputUI(){
			// 	gameManager.ActivateGameplayUI();
			// }
			// void DeactivateInputUI(){
			// 	gameManager.DeactivateGameplayUI();
			// }
			// public UnityBase.FrostGlassAdaptor frostGlassAdaptor;
			// void Frost(){
			// 	UnityBase.IFrostGlass glass = frostGlassAdaptor.GetFrostGlass();
			// 	glass.Frost();
			// }
			// void Defrost(){
			// 	UnityBase.IFrostGlass glass = frostGlassAdaptor.GetFrostGlass();
			// 	glass.Defrost();
			// }
		/*  */
	}
}
