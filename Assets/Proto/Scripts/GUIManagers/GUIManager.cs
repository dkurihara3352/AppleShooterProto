﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using System.Text.RegularExpressions;

namespace SlickBowShooting{
	public class GUIManager : AbsGUIManager {
		// void Awake(){
		// 	CalcRects();
		// }
		void CalcRects(){
			topLeftRect = GetGUIRect(
				normalizedSize: new Vector2(.2f, .4f),
				normalizedPosition: new Vector2(0f ,1f)
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
			sTR_2 = GetSubRect(topRightRect, 1, 4);
			// sTR_3 = GetSubRect(topRightRect, 2, 4);
			// sTR_4 = GetSubRect(topRightRect, 3, 4);

			bottomLeftRect = GetGUIRect(
				normalizedSize: new Vector2(.5f, .5f),
				normalizedPosition: new Vector2(0f, 1f)
			);
			bottomRightRect = GetGUIRect(
				normalizedSize: new Vector2(.5f, 1f),
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
			CalcRects();
			/* left */
				DrawControl();
				DrawBottomLeft();
			/* right */
				// DrawCurrentState(sTR_1);
				// DrawScrollMultiplier();
				// DrawLaunchAngle();
				// DrawFlightSpeed();
				// DrawWaypointsFollower(sTR_1);
				DrawCurveSequence(sTR_2);
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
				if(thisControlContext == 2)
					DrawContextTwo();
				if(thisControlContext == 3)
					DrawContextThree();
			}
			void DrawControlContexSwitch(){
				string[] textArray = new string[]{
					"cont0",
					"cont1",
					"cont2",
					"cont3"
				};
				thisControlContext = GUI.SelectionGrid(
					sTL_1,
					thisControlContext,
					textArray,
					4
				);
			}
			/* Context 0 */
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
						thisCurvesAreReady = true;
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
						StartGameplay();
					if(GUI.Button(
						sTL_6,
						"End"
					))
						EndGameplay();
					if(GUI.Button(
						sTL_7,
						"DoSomething"
					))
						DoSomething();
				}
				public GameplayWidgetAdaptor gameplayWidgetAdaptor;
				void StartGameplay(){
					IGameplayWidget widget = gameplayWidgetAdaptor.GetGameplayWidget();
					widget.StartGameplay();
				}
				void EndGameplay(){
					IGameplayWidget widget = gameplayWidgetAdaptor.GetGameplayWidget();
					widget.EndGameplay();
				}

				void DoSomething(){
					// StartEndGameSequence();	
					StartUp();
					// ToggleMainMenu();
				}
				bool init;
				void ToggleMainMenu(){
					if(!init){
						gameManager.SetUp();
						gameManager.ActivateRootUI();
						init = true;
					}
					IGameplayWidget widget = gameplayWidgetAdaptor.GetGameplayWidget();
					widget.ToggleMainMenu();
				}
				void StartEndGameSequence(){
					IGameplayWidget widget = gameplayWidgetAdaptor.GetGameplayWidget();
					widget.ActivateMainMenu();
					IUIElementGroupScroller rootScroller = (IUIElementGroupScroller)rootScrollerAdaptor.GetUIElement();
					rootScroller.DisableInputSelf();
					ITitlePane titlePane = titlePaneAdaptor.GetTitlePane();
					titlePane.Hide(true);

					IEndGamePane endGamePane = endGamePaneAdaptor.GetEndGamePane();

					endGamePane.ActivateThruBackdoor(true);
					endGamePane.ResetEndGamePane();
					endGamePane.FeedStats(
						300,
						200,
						10,
						0
					);
					endGamePane.StartSequence();
				}
				void StartUp(){
					gameManager.StartUp();
				}

				public UIElementGroupScrollerAdaptor rootScrollerAdaptor;
				public EndGamePaneAdaptor endGamePaneAdaptor;
				public TitlePaneAdaptor titlePaneAdaptor;
			/* Context 1 */
				void DrawContextOne(){
					DrawFileSwitch(sTL_2);
					DrawFileManagement(sTL_3);
					DrawBowSwitch(sTL_4);
					DrawBowControl(sTL_5);
					DrawAttributeSwitch(sTL_6);
					DrawAttributeControl(sTL_7);
				}

				int thisSelectedFileIndex;
				void DrawFileSwitch(Rect rect){
					if(thisSystemIsReady){
						IPlayerDataManager playerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
						
						string[] filePaths = playerDataManager.GetFilePaths();

						string[] buttonTexts = CreateFilesNameArray(filePaths);
						int prevSelectedFileIndex = thisSelectedFileIndex;
						thisSelectedFileIndex = GUI.SelectionGrid(
							rect,
							thisSelectedFileIndex,
							buttonTexts,
							filePaths.Length
						);
						if(thisSelectedFileIndex > filePaths.Length -1)
							thisSelectedFileIndex = filePaths.Length -1;

						if(thisSelectedFileIndex != prevSelectedFileIndex)
							playerDataManager.SetFileIndex(thisSelectedFileIndex);
						prevSelectedFileIndex = thisSelectedFileIndex;
					}else
						GUI.Box(
							rect,
							"ready the system to load files"
						);
				}
				string[] CreateFilesNameArray(string[] paths){
					List<string> resultList = new List<string>();
					foreach(string path in paths){
						Match match = Regex.Match(path, @"Data_\d*");
						string digits = match.Value.Replace("Data_", "");

						resultList.Add(digits);
					}
					return resultList.ToArray();
				}
				void DrawFileManagement(Rect rect){
					Rect sub_0 = GetHorizontalSubRect(rect, 0, 5);
					Rect sub_1 = GetHorizontalSubRect(rect, 1, 5);
					Rect sub_2 = GetHorizontalSubRect(rect, 2, 5);
					Rect sub_3 = GetHorizontalSubRect(rect, 3, 5);
					Rect sub_4 = GetHorizontalSubRect(rect, 4, 5);
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
					if((GUI.Button(
						sub_3,
						"Delete"
					)))
						DeletePlayerData();
					if((GUI.Button(
						sub_4,
						"Create"
					)))
						CreateNewPlayerData();
				}
				void InitializePlayerData(){
					IPlayerDataManager manager = playerDataManagerAdaptor.GetPlayerDataManager();
					manager.SetFileIndex(thisSelectedFileIndex);
					manager.InitializePlayerData();
					CalculateShootingData();
				}
				void LoadPlayerData(){
					IPlayerDataManager manager = playerDataManagerAdaptor.GetPlayerDataManager();
					manager.SetFileIndex(thisSelectedFileIndex);
					manager.Load();
					CalculateShootingData();
				}
				void SavePlayerData(){
					IPlayerDataManager manager = playerDataManagerAdaptor.GetPlayerDataManager();
					manager.SetFileIndex(thisSelectedFileIndex);
					manager.Save();
				}
				void DeletePlayerData(){
					IPlayerDataManager playerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
					string playerDataPath = playerDataManager.GetDirectory();
					string[] filePaths = System.IO.Directory.GetFiles(playerDataPath);
					string pathToDelete = filePaths[thisSelectedFileIndex];
					
					System.IO.File.Delete(pathToDelete);
				}
				void CreateNewPlayerData(){
					IPlayerDataManager playerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
					int newFileIndex = playerDataManager.CreateNewPlayerDataFile();
					thisSelectedAttributeIndex = newFileIndex;
				}

				int thisSelectedBowIndex;
				void DrawBowSwitch(Rect rect){
					thisSelectedBowIndex = GUI.SelectionGrid(
						rect,
						thisSelectedBowIndex,
						new string[]{
							"bow 0",
							"bow 1",
							"bow 2"
						},
						3
					);
				}
				void DrawBowControl(Rect rect){
					if(thisSystemIsReady){
						IPlayerDataManager playerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
						if(playerDataManager.PlayerDataIsLoaded()){
							//unlock, equip
							Rect sub_0 = GetHorizontalSubRect(rect, 0, 2);
							Rect sub_1 = GetHorizontalSubRect(rect, 1, 2);
							IBowConfigData bowConfigData = playerDataManager.GetBowConfigDataArray()[thisSelectedBowIndex];
							
							bool isUnlocked = bowConfigData.IsUnlocked();
							string lockToggleButtonText = isUnlocked ? "Lock" : "Unlock";
							
							if(GUI.Button(
								sub_0,
								lockToggleButtonText
							)){
								if(isUnlocked)
									playerDataManager.LockBow(thisSelectedBowIndex);
								else
									playerDataManager.UnlockBow(thisSelectedBowIndex);
							}
							
							int equippedBowIndex = playerDataManager.GetEquippedBowIndex();
							if(equippedBowIndex == thisSelectedBowIndex){
								GUI.Box(
									sub_1,
									"Equipped"
								);
							}else{
								if(GUI.Button(
									sub_1,
									"Equip"
								))
									playerDataManager.SetEquippedBow(thisSelectedBowIndex);
							}
						}else{
							GUI.Box(
								rect,
								"bow control\n" + 
								"playerData not ready"
							);
						}
					}
				}
				// void DrawEquippedBowSwitch(Rect rect){
				// 	if(thisSystemIsReady){
				// 		IPlayerDataManager playerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
				// 		if(playerDataManager.PlayerDataIsLoaded()){
				// 			int equippedBowIndex = playerDataManager.GetEquippedBowIndex();
				// 			string[] texts = new string[]{
				// 				"bow 0",
				// 				"bow 1",
				// 				"bow 2"
				// 			};

				// 			int newEquippedBowIndex = GUI.SelectionGrid(
				// 				rect,
				// 				equippedBowIndex,
				// 				texts,
				// 				playerDataManager.GetBowCount()
				// 			);

				// 			if(newEquippedBowIndex != equippedBowIndex){
				// 				playerDataManager.SetEquippedBow(newEquippedBowIndex);
				// 				CalculateShootingData();
				// 			}
				// 		}else{
				// 			GUI.Box(
				// 				rect,
				// 				"equippedBow \n" +
				// 				"playerData not ready"
				// 			);
				// 		}
				// 	}
				// }

				int thisSelectedAttributeIndex = 0;
				void DrawAttributeSwitch(Rect rect){
					thisSelectedAttributeIndex = GUI.SelectionGrid(
						rect,
						thisSelectedAttributeIndex,
						new string[]{
							"STR",
							"QCK",
							"CRT"
						},
						3
					);
				}
				void DrawAttributeControl(Rect rect){
					if(thisSystemIsReady){
						IPlayerDataManager playerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
						if(playerDataManager.PlayerDataIsLoaded()){
							Rect sub_0 = GetHorizontalSubRect(rect, 0, 3);
							Rect sub_1 = GetHorizontalSubRect(rect, 1, 3);
							Rect sub_2 = GetHorizontalSubRect(rect, 2, 3);
							if(GUI.Button(
								sub_0,
								"Up"
							)){
								int equippedBowIndex = playerDataManager.GetEquippedBowIndex();
								IncreaseAttributeLevel(
									thisSelectedAttributeIndex
								);
								CalculateShootingData();
							}
							if(GUI.Button(
								sub_1,
								"Down"
							)){
								int equippedBowIndex = playerDataManager.GetEquippedBowIndex();
								DecreaseAttributeLevel(
									thisSelectedAttributeIndex
								);
								CalculateShootingData();
							}
							if(GUI.Button(
								sub_2,
								"Clear All"
							)){
								ClearAllBowConfigData();
							}
						}else{
							GUI.Box(
								rect,
								"attributeControl" + "\n" +
								"playerData not ready"
							);
						}
					}
				}
				void IncreaseAttributeLevel(
					int attributeIndex
				){
					IPlayerDataManager playerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
					playerDataManager.IncreaseAttributeLevel(attributeIndex);
				}
				void DecreaseAttributeLevel(
					int attributeIndex
				){
					IPlayerDataManager playerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
					playerDataManager.DecreaseAttributeLevel(attributeIndex);

				}
				// void IncrementBowLevelAt(int attributeIndex){
				// 	IPlayerDataManager dataManager = playerDataManagerAdaptor.GetPlayerDataManager();
				// 	int equippedBowIndex = dataManager.GetEquippedBowIndex();
				// 	dataManager.IncrementBowLevel(equippedBowIndex, attributeIndex);
				// 	CalculateShootingData();
				// }
				void ClearAllBowConfigData(){
					IPlayerDataManager dataManager = playerDataManagerAdaptor.GetPlayerDataManager();
					IBowConfigData[] configData = dataManager.GetBowConfigDataArray();
					int arrayLength = configData.Length;
					for(int i = 0; i < arrayLength; i ++){
						dataManager.ClearBowConfigData(i);
					}
					CalculateShootingData();
				}
				public ShootingDataManagerAdaptor shootingDataManagerAdaptor;
				void CalculateShootingData(){
					IShootingDataManager shootingDataManager = shootingDataManagerAdaptor.GetShootingDataManager();
					shootingDataManager.CalculateShootingData();
				}
			/* Context two */
				void DrawContextTwo(){
					DrawAddHeatButton(sTL_2);
					DrawTargetTierControl(sTL_3);
					DrawCurrencyControl(sTL_4);
				}
				void DrawAddHeatButton(Rect rect){
					if(GUI.Button(
						rect,
						"add heat"
					))
						AddHeat();
				}
				int thisTargetTier;
				void DrawTargetTierControl(Rect rect){
					string[] texts = new string[]{"tier0", "tier1", "tier2"};
					int prevTier = thisTargetTier;
					thisTargetTier = GUI.SelectionGrid(rect, thisTargetTier, texts, 3);
					if(thisTargetTier != prevTier)
						SetTierOnAllTargetReserves(thisTargetTier);
					
				}
				public int currency;
				void DrawCurrencyControl(Rect rect){
					if(thisSystemIsReady){

						Rect sub_0 = GetHorizontalSubRect(rect, 0, 2);//set
						Rect sub_1 = GetHorizontalSubRect(rect, 1, 2);//add

						IPlayerDataManager playerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
						if(GUI.Button(
							sub_0,
							"Set Currency"
						))
							playerDataManager.SetCurrency(currency);
						if(GUI.Button(
							sub_1,
							"Add 100"
						)){
							int currency = playerDataManager.GetCurrency();
							int newCurrency = currency + 100;
							playerDataManager.SetCurrency(newCurrency);
						}

					}
				}
		/*  */
			void DrawArrowsState(Rect rect){
				if(thisSystemIsReady){
					IArrowReserve arrowReserve = arrowReserveAdaptor.GetArrowReserve();
					IArrow[] arrows = arrowReserve.GetArrows();
					string result = "";
					foreach(IArrow arrow in arrows){
						result += 
							"id: " + arrow.GetIndex() + ", " +
							"state: " + GetArrowStateString(arrow) + ", " +
							"position: " + arrow.GetPosition().ToString() + ", " +
							"parent : " + arrow.GetParentName() + "\n";
					}
					GUI.Label(
						rect,
						result
					);
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
		/* Context Three */
			void DrawContextThree(){
				DrawGameplayControl(sTL_2);
				DrawColorSchemeControl(sTL_3);
				DrawADTestControl(sTL_4);
				DrawFileClear(sTL_5);
			}

			void DrawGameplayControl(Rect rect){
				Rect sub_0 = GetHorizontalSubRect(rect, 0, 2);
				Rect sub_1 = GetHorizontalSubRect(rect, 1, 2);
				DrawCountDownToggle(sub_0);
			}
			void DrawCountDownToggle(Rect rect){
				IHeatManager heatManager = heatManagerAdaptor.GetHeatManager();
				bool currentlyCountingDown = heatManager.IsCountingDown();
				string toggleText = currentlyCountingDown? "Heat Stop" : "Heat Start";
				bool countsDown = GUI.Toggle(rect, currentlyCountingDown, toggleText);
				if(currentlyCountingDown != countsDown)
					if(countsDown)
						heatManager.StartCountingDown();
					else
						heatManager.StopCountingDown();

			}
			void DrawColorSchemeControl(Rect rect){
				Rect sub_0 = GetHorizontalSubRect(rect, 0, 3);
				Rect sub_1 = GetHorizontalSubRect(rect, 1, 3);
				Rect sub_2 = GetHorizontalSubRect(rect, 2, 3);

				if(GUI.Button(
					sub_0,
					"0"
				))
					ChangeColorScheme(0);
				if(GUI.Button(
					sub_1,
					"1"
				))
					ChangeColorScheme(1);
				if(GUI.Button(
					sub_2,
					"2"
				))
					ChangeColorScheme(2);
			}
			void ChangeColorScheme(int index){
				IColorSchemeManager manager = colorSchemeManagerAdaptor.GetColorSchemeManager();
				manager.ChangeColorScheme(index, 10f);
			}
			public ColorSchemeManagerAdaptor colorSchemeManagerAdaptor;

			void DrawADTestControl(Rect rect){
				Rect sub_0 = GetHorizontalSubRect(rect, 0, 1);
				IADManager doubleEarnedCrystalsADmanager = doubleEarnedCrystalsADManagerAdaptor.GetADManager();
				if(doubleEarnedCrystalsADmanager != null){
					if(GUI.Button(
						sub_0,
						"AD Ready"
					))
						doubleEarnedCrystalsADmanager.TestToggleADReady(true);
				}

			}
			public ADManagerAdaptor doubleEarnedCrystalsADManagerAdaptor;

			void DrawFileClear(Rect rect){
				IPlayerDataManager playerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
				if(playerDataManager != null){
					if(GUI.Button(
						rect,
						"Clear Bow Config Data"
					))
						playerDataManager.ClearAllBowConfigData();
				}
			}


		/* Bottom Left */
			void DrawBottomLeft(){
				// DrawArrowsState(bottomLeftRect);
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
			// public GlidingTargetSpawnPointGroupAdaptor glidingTargetSpawnPointGroupAdaptor;
			// void CalcCurveOnGliderSpawnPointGroup(){
			// 	IGlidingTargetSpawnPointGroup group = glidingTargetSpawnPointGroupAdaptor.GetGlidingTargetSpawnPointGroup();
			// 	IGlidingTargetSpawnPoint[] spawnPoints = group.GetGlidingTargetSpawnPoints();
			// 	foreach(IGlidingTargetSpawnPoint point in spawnPoints){
			// 		IGlidingTargetWaypointCurve curve = point.GetGlidingTargetWaypointCurve();
			// 		curve.CalculateCurve();
			// 	}
			// }
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
			bool thisCurvesAreReady = false;
			bool thisGameIsReady{
				get{
					return gameManager.GameIsReady();
				}
			}
			void DrawWaypointEvents(Rect rect){
				if(thisGameIsReady){
					string result = "";
					IWaypointsFollower follower = waypointsFollowerAdaptor.GetWaypointsFollower();
					float normalizedPosOnCurve = follower.GetNormalizedPositionInCurve();
					result += "normPos: " + normalizedPosOnCurve.ToString("N2") + "\n";
					
					IWaypointCurveCycleManager curveManager = curveCycleManagerAdaptor.GetWaypointsManager();
					int currentCurveIndex = gameManager.GetCurrentWaypointGroupIndex();
					IWaypointCurve currentCurve = curveManager.GetAllWaypointCurves()[currentCurveIndex];
					IWaypointEvent[] evnets = currentCurve.GetWaypointEvents();
					result += "curve id: " + currentCurve.GetIndex().ToString() + "\n";
					foreach(IWaypointEvent wpEvent in evnets){
						string thisEventString = "\n";
						thisEventString += wpEvent.GetName() + ": " + wpEvent.GetEventPoint().ToString("N2");
						TargetType thisType = TargetType.Flyer;
						bool isRare = false;
						if(wpEvent is IShootingTargetSpawnWaypointEvent){
							IShootingTargetSpawnWaypointEvent spawnEvent = (IShootingTargetSpawnWaypointEvent)wpEvent;
							isRare = spawnEvent.IsRare();
							thisType = spawnEvent.GetTargetType();
							IShootingTargetSpawnPoint spawnPoint = spawnEvent.GetSpawnPoint();
							thisEventString += ", sp: " + spawnPoint.GetName();
							IShootingTarget target = spawnPoint.GetSpawnedTarget();
							if(target != null)
								thisEventString += ", tar: " + target.GetName();
							else
								thisEventString += ", tar: null";

						}
						Color col = GetStringColorForType(thisType, isRare);
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
			Color GetStringColorForType(TargetType targetType, bool isRare){
				if(isRare)
					return Color.yellow;
				else
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
			void DrawBottomRight(){
				// Rect sub_0 = GetHorizontalSubRect(bottomRightRect, 0, 2);
				// Rect sub_1 = GetHorizontalSubRect(bottomRightRect, 1, 2);
				// DrawPlayerData(sub_0);
				// DrawBowData(sub_1);
				// DrawLandedArrows(bottomRightRect);
				
				DrawWaypointEvents(bottomRightRect);
				// DrawInterstitialADManager(bottomRightRect);
			}
			void DrawPlayerData(Rect rect){
				if(thisSystemIsReady){
					IPlayerDataManager manager = playerDataManagerAdaptor.GetPlayerDataManager();
					GUI.Label(
						rect,
						manager.GetDebugString()
					);
				}
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
			public LandedArrowReserveAdaptor landedArrowReserveAdaptor;
			void DrawLandedArrows(Rect rect){
				if(thisSystemIsReady){
					ILandedArrowReserve reserve = landedArrowReserveAdaptor.GetLandedArrowReserve();
					ILandedArrow[] arrows = reserve.GetLandedArrows();
					string result = "";
					foreach(ILandedArrow arrow in arrows){
						int index = arrow.GetIndex();
						result += index.ToString() + ": ";
						if(arrow.IsActivated()){
							result += DKUtility.DebugHelper.StringInColor(" activated ", Color.green);
							result += "\n\t" + "detector: " + arrow.GetHitDetector().GetName();
						}
						else
							result += "deactivated ";
						Vector3 localScale = arrow.GetLocalScale();
						result += "locScale: " + localScale.ToString();
						result += "\n";
					}
					GUI.Label(
						rect,
						result
					);
				}
			}
			void DrawInterstitialADManager(Rect rect){
				IInterstitialADManager interstitialADManager = interstitialADManagerAdaptor.GetInterstitialADManager();
				string debugString;
				if(interstitialADManager != null){
					debugString = interstitialADManager.GetDebugString();
				}else
					debugString = "interstitialADManager not ready";
				GUI.Label(
					rect,
					debugString
				);
			}	
			public InterstitialADManagerAdaptor interstitialADManagerAdaptor;
		/*  */
	}
}
