using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public class GUIManager : AbsGUIManager {
		void Awake(){
			CalcRects();
		}
		void Update(){
			CalcRects();
		}
		void CalcRects(){
			topLeftRect = GetGUIRect(
				normalizedSize: new Vector2(.2f, .4f),
				normalizedPosition: new Vector2(0f ,0f)
			);
			sTL_1 = GetSubRect(topLeftRect,0,7);
				sTL_1_0 = GetHorizontalSubRect(sTL_1, 0, 4);
				sTL_1_1 = GetHorizontalSubRect(sTL_1, 1, 4);
				sTL_1_2 = GetHorizontalSubRect(sTL_1, 2, 4);
				sTL_1_3 = GetHorizontalSubRect(sTL_1, 3, 4);
			sTL_2 = GetSubRect(topLeftRect,1,7);
			sTL_3 = GetSubRect(topLeftRect,2,7);
			sTL_4 = GetSubRect(topLeftRect,3,7);
			sTL_5 = GetSubRect(topLeftRect,4,7);
			sTL_6 = GetSubRect(topLeftRect,5,7);
			sTL_7 = GetSubRect(topLeftRect,6,7);
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
			Rect sTL_1_0; 
			Rect sTL_1_1; 
			Rect sTL_1_2; 
			Rect sTL_1_3; 
		Rect sTL_2; 
		Rect sTL_3; 
		Rect sTL_4; 
		Rect sTL_5; 
		Rect sTL_6;
		Rect sTL_7;


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
		public UISystem.UIManagerAdaptor uiManagerAdaptor;
		void OnGUI(){
			/* left */
				DrawControl();
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
				// DrawScrollerDebug(bottomRightRect);
				DrawBottomRight();
		}
		/* left */
			int thisControlContext = 0;
			void DrawControl(){
				DrawControlContexSwitch();	
				if(thisControlContext == 0)
					DrawContexZero();
				if(thisControlContext == 1)
					DrawContextOne();
			}
			void DrawControlContexSwitch(){
				if(GUI.Button(
					sTL_1_0,
					"0"
				)){
					thisControlContext = 0;
				}
				if(GUI.Button(
					sTL_1_1,
					"1"
				)){
					thisControlContext = 1;
				}
				if(GUI.Button(
					sTL_1_2,
					"2"
				)){
					thisControlContext = 2;
				}
				if(GUI.Button(
					sTL_1_3,
					"3"
				)){
					thisControlContext = 3;
				}
			}
			void DrawContexZero(){
				if(GUI.Button(
					sTL_2,
					"SetUp"
				)){
					gameManager.SetUp();
					thisSystemIsReady = true;
				}
				if(GUI.Button(
					sTL_3,
					"WarmUp"
				)){
					gameManager.WarmUp();
				}
				if(GUI.Button(
					sTL_4,
					"ActivateRootUIE"
				))
					gameManager.ActivateRootUI();
				if(GUI.Button(
					sTL_5,
					"Start"
				))
					gameManager.StartGameplaySequence();
				if(GUI.Button(
					sTL_6,
					"End"
				))
					gameManager.StartEndGameplaySequence();
			}
			void DrawContextOne(){
				DrawFileManagement(sTL_2);
				DrawEquippedBowSwitch(sTL_3);
				DrawAttributeSwitch(sTL_4);
				DrawAttributeControl(sTL_5);
			}
			void DrawFileManagement(Rect rect){
				Rect sub_0 = GetHorizontalSubRect(
					rect, 0, 3
				);
				Rect sub_1 = GetHorizontalSubRect(
					rect, 1, 3
				);
				Rect sub_2 = GetHorizontalSubRect(
					rect, 2, 3
				);
				if((GUI.Button(
					sub_0,
					"Initialize"
				)))
					InitializePlayerData();
				if((GUI.Button(
					sub_1,
					"Load"
				)))
					LoadPlayerData();
				if((GUI.Button(
					sub_2,
					"Save"
				)))
					SavePlayerData();
			}
			void DrawEquippedBowSwitch(Rect rect){
				Rect sub_0 = GetHorizontalSubRect(rect, 0, 4);
				Rect sub_1 = GetHorizontalSubRect(rect, 1, 4);
				Rect sub_2 = GetHorizontalSubRect(rect, 2, 4);
				Rect sub_3 = GetHorizontalSubRect(rect, 3, 4);
				IPlayerDataManager manager = playerDataManagerAdaptor.GetPlayerDataManager();
				GUI.Label(
					sub_0,
					"eqpBow: " + GetEquippedBowIndexString(manager)
				);
				if(GUI.Button(
					sub_1,
					"0"
				))
					manager.SetEquippedBow(0);
				if(GUI.Button(
					sub_2,
					"1"
				))
					manager.SetEquippedBow(1);
				if(GUI.Button(
					sub_3,
					"2"
				))
					manager.SetEquippedBow(2);
			}
			string GetEquippedBowIndexString(IPlayerDataManager manager){
				if(manager.PlayerDataIsLoaded())
					return manager.GetEquippedBowIndex().ToString();
				else
					return "not loaded";
			}
			void DrawAttributeSwitch(Rect rect){
				Rect sub_0 = GetHorizontalSubRect(rect, 0, 4);
				Rect sub_1 = GetHorizontalSubRect(rect, 1, 4);
				Rect sub_2 = GetHorizontalSubRect(rect, 2, 4);
				Rect sub_3 = GetHorizontalSubRect(rect, 3, 4);

				GUI.Label(sub_0, "attr: " + thisAttributeToModIndex.ToString());
				if(GUI.Button(sub_1, "0"))
					thisAttributeToModIndex = 0;
				if(GUI.Button(sub_2, "1"))
					thisAttributeToModIndex = 1;
				if(GUI.Button(sub_3, "2"))
					thisAttributeToModIndex = 2;
			}
			void DrawAttributeControl(Rect rect){
				Rect sub_0 = GetHorizontalSubRect(rect, 0, 3);
				Rect sub_1 = GetHorizontalSubRect(rect, 1, 3);
				Rect sub_2 = GetHorizontalSubRect(rect, 2, 3);
				if(GUI.Button(sub_0, "Up"))
					ModifyAttributeLevel(true);
				if((GUI.Button(sub_1, "Down")))
					ModifyAttributeLevel(false);
				if(GUI.Button(sub_2, "Calc"))
					CalculateShootingData();
			}
			void ModifyAttributeLevel(bool increment){
				IPlayerDataManager manager = playerDataManagerAdaptor.GetPlayerDataManager();
				int equippedBowIndex = manager.GetEquippedBowIndex();
				IBowConfigData data = manager.GetBowConfigDataArray()[equippedBowIndex];
				int[] newAttributeLevelArray = data.GetAttributeLevelArray();
				newAttributeLevelArray[thisAttributeToModIndex] = newAttributeLevelArray[thisAttributeToModIndex] + (increment?1: -1);

				data.SetAttributeLevelArray(newAttributeLevelArray);
			}
			int thisAttributeToModIndex;
			int currentTier = 0;
			int maxTier = 1;
			public ShootingDataManagerAdaptor shootingDataManagerAdaptor;
			void CalculateShootingData(){
				IShootingDataManager shootingDataManager = shootingDataManagerAdaptor.GetShootingDataManager();
				shootingDataManager.CalculateShootingData();
			}
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
			void InitializePlayerData(){
				IPlayerDataManager manager = playerDataManagerAdaptor.GetPlayerDataManager();
				manager.InitializePlayerData();
			}
			void LoadPlayerData(){
				IPlayerDataManager manager = playerDataManagerAdaptor.GetPlayerDataManager();
				manager.Load();
			}
			void SavePlayerData(){
				IPlayerDataManager manager = playerDataManagerAdaptor.GetPlayerDataManager();
				manager.Save();
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
			void DrawScrollerDebug(Rect rect){
				if(thisSystemIsReady){
					string result = "";
					IUIManager uiManager = uiManagerAdaptor.GetUIManager();
					result += "scroller: ";
					IScroller handlingScroller = uiManager.GetInputHandlingScroller();
					if(handlingScroller != null)
						result += handlingScroller.GetName() + "\n";
					else
						result += "null\n";
					result += "event: " + uiManager.GetEventName();
					GUI.Label(
						rect,
						result
					);
				}
			}
			public PlayerDataManagerAdaptor playerDataManagerAdaptor;
			void DrawPlayerData(Rect rect){
				if(thisSystemIsReady){
					IPlayerDataManager manager = playerDataManagerAdaptor.GetPlayerDataManager();
					GUI.Label(
						rect,
						manager.GetDebugString()
					);
				}
			}
			void DrawBottomRight(){
				Rect sub_0 = GetHorizontalSubRect(bottomRightRect, 0, 2);
				Rect sub_1 = GetHorizontalSubRect(bottomRightRect, 1, 2);
				DrawPlayerData(sub_0);
				DrawBowData(sub_1);
			}
			void DrawBowData(Rect rect){
				if(thisSystemIsReady){
					IShootingManager shootingManager = shootingManagerAdaptor.GetShootingManager();
					string result = shootingManager.GetDebugString();
					GUI.Label(
						rect,
						result
					);
				}
			}

		/*  */
	}
}
