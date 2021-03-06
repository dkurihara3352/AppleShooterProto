﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
using DKUtility;
using UISystem;

namespace SlickBowShooting{
	public interface IGameplayWidget: ISlickBowShootingSceneObject, IActivationStateHandler, IActivationStateImplementor, IProcessHandler{
		void SetGameplayUIElement(IGameplayUIElement uiElement);
		void SetPlayerCharacterWaypointsFollower(IPlayerCharacterWaypointsFollower follower);
		void SetGameStatsTracker(IGameStatsTracker tracker);
		void SetHeadUpDisplay(IHeadUpDisplay hud);
		void SetRootScroller(IUIElementGroupScroller scroller);
		void SetRootElementFrostGlass(IFrostGlass glass);
		void SetResourcePanel(IResourcePanel panel);
		void SetMainMenuUIElement(IMainMenuUIElement mainMenuUIElement);
		void SetEndGamePane(IEndGamePane pane);
		void SetTitlePane(ITitlePane pane);
		void SetPlayerDataManager(IPlayerDataManager manager);
		void SetShootingDataManager(IShootingDataManager manager);
		void SetScoreManager(IScoreManager manager);
		void SetCurrencyManager(ICurrencyManager manager);
		void SetHeatManager(IHeatManager manager);
		void SetCoreGameplayInputScroller(ICoreGameplayInputScroller scroller);
		void SetGameplayPause(IGameplayPause pause);
		void SetPlayerInputManager(IPlayerInputManager manager);
		void SetFrostManager(IFrostManager manager);
		void SetTutorialPane(ITutorialPane pane);
		void SetColorSchemeManager(IColorSchemeManager manager);
		void SetInterstitialADManager(IInterstitialADManager manager);
		
		void StartGameplay();
		void EndGameplay();
		void StartDelayedGameplay();

		void ActivateMainMenu();
		void DeactivateMainMenu();
		void ToggleMainMenu();
	}
	public class GameplayWidget: SlickBowShootingSceneObject, IGameplayWidget{
		public GameplayWidget(IConstArg arg): base(arg){
			thisActivationEngine = new ActivationStateEngine(this);
			thisWaitAndStartProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisStartWaitTime
			);
		}
		IGameplayWidgetAdaptor thisGameplayWidgetAdaptor{
			get{
				return (IGameplayWidgetAdaptor)thisAdaptor;
			}
		}
		float thisStartWaitTime{
			get{
				return thisGameplayWidgetAdaptor.GetStartWaitTime();
			}
		}
		IActivationStateEngine thisActivationEngine;
		public void Activate(){
			thisActivationEngine.Activate();
		}
		public void Deactivate(){
			thisActivationEngine.Deactivate();
		}
		public void ActivateImple(){
			
			HideResourcePanel();
			ShowTitle();
		}
		void DeactivateEndGamePane(){
			thisEndGamePane.DeactivateRecursively(false);
		}
		IEndGamePane thisEndGamePane;
		public  void SetEndGamePane(IEndGamePane pane){
			thisEndGamePane = pane;
		}
		void HideResourcePanel(){
			if(thisResourcePanel.IsShown())
				thisResourcePanel.Hide();
		}
		IResourcePanel thisResourcePanel;
		public void SetResourcePanel(IResourcePanel panel){
			thisResourcePanel = panel;
		}
		void ShowTitle(){
			thisTitlePane.Show(false);
		}
		ITitlePane thisTitlePane;
		public void SetTitlePane(ITitlePane pane){
			thisTitlePane = pane;
		}
		public void DeactivateImple(){
			DeactivateEndGamePane();

		}
		public bool IsActivated(){
			return thisActivationEngine.IsActivated();
		}
		/* StartGameplay */
			public void StartGameplay(){
				if(!TutorialIsDone())
					StartGameplayWithTutorial();
				else{
					DeactivateMainMenu();
					SetUpShootingData();
					ActivateGameplayUI();
					StartWaitAndStartGameplay();
					DisableRootScroller();
					Defrost();
					ChangeColorSchemeToZero();
					// DefrostRootElement();
				}
			}
			void SetUpShootingData(){
				/*  
				*/
				thisPlayerDataManager.SetFileIndex(0);
				thisPlayerDataManager.Load();
				thisShootingDataManager.CalculateShootingData();
				thisPlayerDataManager.Save();
			}
			IPlayerDataManager thisPlayerDataManager;
			public void SetPlayerDataManager(IPlayerDataManager manager){
				thisPlayerDataManager = manager;
			}
			IShootingDataManager thisShootingDataManager;
			public void SetShootingDataManager(IShootingDataManager manager){
				thisShootingDataManager = manager;
			}

			public void ActivateGameplayUI(){
				// thisGameplayUIElement.ActivateRecursively(instantly: false);
				thisGameplayUIElement.ActivateThruBackdoor(instantly: false);
				thisGameplayUIElement.EvaluateScrollerFocusRecursively();
			}
			IGameplayUIElement thisGameplayUIElement;
			public void SetGameplayUIElement(IGameplayUIElement uiElement){
				thisGameplayUIElement = uiElement;
			}
			public void StartWaitAndStartGameplay(){
				thisWaitAndStartProcessSuite.Start();
			}
			IProcessSuite thisWaitAndStartProcessSuite;
			public void OnProcessRun(IProcessSuite suite){}
			public void OnProcessExpire(IProcessSuite suite){
				if(suite == thisWaitAndStartProcessSuite)
					StartDelayedGameplay();
			}
			public void StartDelayedGameplay(){
				StartTargetSpawn();
				ResetStats();
				LoadAndSetHighScore();
				// ActivateGameplayUI();
				ActivateHUD();
				ActivatePauseButton();
				StartHeatCountDown();

				StartInterstitialADManagerCountDown();
			}
			public void StartTargetSpawn(){
				thisPlayerCharacterWaypointsFollower.StartExecutingSpawnEvents();
			}
			IPlayerCharacterWaypointsFollower thisPlayerCharacterWaypointsFollower;
			public void SetPlayerCharacterWaypointsFollower(IPlayerCharacterWaypointsFollower follower){
				thisPlayerCharacterWaypointsFollower = follower;
			}
			void ResetStats(){
				thisGameStatsTracker.ResetStats();
			}
			IGameStatsTracker thisGameStatsTracker;
			public void SetGameStatsTracker(IGameStatsTracker tracker){
				thisGameStatsTracker = tracker;
			}
			void LoadAndSetHighScore(){
				thisPlayerDataManager.SetFileIndex(0);
				if(!thisPlayerDataManager.PlayerDataIsLoaded())
					thisPlayerDataManager.Load();
				int highScore = thisPlayerDataManager.GetHighScore();

				thisScoreManager.SetHighScore(highScore);

				thisPlayerDataManager.Save();
			}
			void ActivateHUD(){
				thisHUD.Activate();
			}
			IHeadUpDisplay thisHUD;
			public void SetHeadUpDisplay(IHeadUpDisplay hud){
				thisHUD = hud;
			}
			void ActivatePauseButton(){
				thisGameplayPause.ActivatePauseButton();
			}
			void StartHeatCountDown(){
				thisHeatManager.StartCountingDown();
			}



			public void OnProcessUpdate(
				float deltaTime,
				float normalizedTime,
				IProcessSuite suite
			){
			}
			void DisableRootScroller(){
				thisRootScroller.DisableInputSelf();
			}
			IUIElementGroupScroller thisRootScroller;
			public void SetRootScroller(IUIElementGroupScroller scroller){
				thisRootScroller = scroller;
			}
			void DefrostRootElement(){
				thisRootElementFrostGlass.Defrost();
			}
			IFrostGlass thisRootElementFrostGlass;
			public void SetRootElementFrostGlass(IFrostGlass glass){
				thisRootElementFrostGlass = glass;
			}
			void ChangeColorSchemeToZero(){
				thisColorSchemeManager.ChangeColorScheme(0, 10f);
			}
			IColorSchemeManager thisColorSchemeManager;
			public void SetColorSchemeManager(IColorSchemeManager manager){
				thisColorSchemeManager = manager;
			}
			
			void StartInterstitialADManagerCountDown(){
				if(!thisInterstitialADManager.IsCountingDown())
					thisInterstitialADManager.StartCounting();
			}
			IInterstitialADManager thisInterstitialADManager;
			public void SetInterstitialADManager(IInterstitialADManager manager){
				thisInterstitialADManager = manager;
			}

		/* Tutorial */
			bool TutorialIsDone(){
				if(!thisPlayerDataManager.PlayerDataIsLoaded())
					thisPlayerDataManager.Load();
				return thisPlayerDataManager.GetTutorialIsDone();
			}
			void StartGameplayWithTutorial(){
				DeactivateMainMenu();
				SetUpShootingData();
				ActivateGameplayUI();
				// StartWaitAndStartGameplay();
				DisableRootScroller();
				Defrost();
				ActivateTutorialPane();
				ChangeColorSchemeToZero();
			}
			void ActivateTutorialPane(){
				thisTutorialPane.ActivateThruBackdoor();
			}
			ITutorialPane thisTutorialPane;
			public void SetTutorialPane(ITutorialPane pane){
				thisTutorialPane = pane;
			}
		/* End gameplay */
			public void EndGameplay(){
				RaisePointerOnInputScroller();
				ExpireUnpauseProcess();
				DeactivatePauseButton();
				StopCountDownHeat();
				MarkGameStatsTrackerGameplayEnded();
				DeactivateGameplayUI();
				DeactivateHUD();
				StopTargetSpawn();
				// EnableRootScroller();
				// FrostRootElement();

				GameResultStats stats = CreateGameResultStats();
				MightWannaSaveDataHere(stats);

				ActivateMainMenu();
				HideTitle();
				ActivateEndGamePane();
				ResetEndGamePane();

				FeedEndGamePane(stats);
				StartEndSequence();

				Frost();

				IncrementInterstitialADManagerSessionCount();
			}
			void RaisePointerOnInputScroller(){
				if(thisPlayerInputMnanager.IsDrawing() || thisPlayerInputMnanager.IsLookingAround())
					thisInputScroller.ClearInput();
			}
			IPlayerInputManager thisPlayerInputMnanager;
			public void SetPlayerInputManager(IPlayerInputManager manager){
				thisPlayerInputMnanager = manager;
			}
			ICoreGameplayInputScroller thisInputScroller;
			public void SetCoreGameplayInputScroller(ICoreGameplayInputScroller scroller){
				thisInputScroller = scroller;
			}
			void ExpireUnpauseProcess(){
				thisGameplayPause.ExpireUnpauseProcess();
			}
			IGameplayPause thisGameplayPause;
			void DeactivatePauseButton(){
				thisGameplayPause.DeactivatePauseButton();
			}
			public void SetGameplayPause(IGameplayPause pause){
				thisGameplayPause = pause;
			}
			void StopCountDownHeat(){
				thisHeatManager.StopProcessAndCountDown();
				// thisHeatManager.StopCountingDown();
			}
			void MarkGameStatsTrackerGameplayEnded(){
				thisGameStatsTracker.MarkGameplayEnded();
			}
			IHeatManager thisHeatManager;
			public void SetHeatManager(IHeatManager manager){
				thisHeatManager = manager;
			}
			public void DeactivateGameplayUI(){
				thisGameplayUIElement.DeactivateRecursively(false);
			}
			void DeactivateHUD(){
				thisHUD.Deactivate();
			}
			public void StopTargetSpawn(){
				thisPlayerCharacterWaypointsFollower.StopExecutingSpawnEvents();
			}
			// void EnableRootScroller(){
			// 	thisRootScroller.EnableInputSelf();
			// }
			void FrostRootElement(){
				thisRootElementFrostGlass.Frost();
			}
			void MightWannaSaveDataHere(GameResultStats stats){
				thisPlayerDataManager.SetFileIndex(0);
				if(!thisPlayerDataManager.PlayerDataIsLoaded())
					thisPlayerDataManager.Load();
				
				int currency = thisPlayerDataManager.GetCurrency();
				int newCurrency = currency + stats.gainedCurrency + stats.scoreCurrencyBonus;

				thisPlayerDataManager.SetCurrency(newCurrency);

				int score = stats.score;
				int highScore = thisPlayerDataManager.GetHighScore();
				
				if(score > highScore)
					thisPlayerDataManager.SetHighScore(score);
				
				thisPlayerDataManager.Save();
			}
			struct GameResultStats{
				public int score;
				public int highScore;
				public int gainedCurrency;
				public int scoreCurrencyBonus;
			}
			GameResultStats CreateGameResultStats(){
				int score = thisScoreManager.GetScore();
				int highScore = thisScoreManager.GetHighScore();
				int gainedCurrency = thisCurrencyManager.GetGainedCurrency();
				GameResultStats stats = new GameResultStats();
				stats.score = score;
				stats.highScore = highScore;
				stats.gainedCurrency = gainedCurrency;
				stats.scoreCurrencyBonus = CalculateScoreCurrencyBonus(score);

				return stats;
			}
			IScoreManager thisScoreManager;
			public void SetScoreManager(IScoreManager manager){
				thisScoreManager = manager;
			}
			ICurrencyManager thisCurrencyManager;
			public void SetCurrencyManager(ICurrencyManager manager){
				thisCurrencyManager = manager;
			}
			void HideTitle(){
				thisTitlePane.Hide(true);
			}
			void ActivateEndGamePane(){
				thisEndGamePane.ActivateThruBackdoor(false);
			}
			void ResetEndGamePane(){
				thisEndGamePane.ResetEndGamePane();
			}
			void FeedEndGamePane(GameResultStats stats){
				thisEndGamePane.FeedStats(
					stats.score,
					stats.highScore,
					stats.gainedCurrency,
					stats.scoreCurrencyBonus
				);
			}
			int CalculateScoreCurrencyBonus(int score){
				if(score >= 100){
					return score / 100;
				}else
					return 0;
			}
			void StartEndSequence(){
				thisEndGamePane.StartSequence();
			}
			float frostTime = 1f;
			void Defrost(){
				thisFrostManager.Defrost(frostTime);
			}
			void Frost(){
				thisFrostManager.Frost(frostTime);
			}
			IFrostManager thisFrostManager;
			public void SetFrostManager(IFrostManager manager){
				thisFrostManager = manager;
			}

			void IncrementInterstitialADManagerSessionCount(){
				thisInterstitialADManager.IncrementSessionCountPlayed();
			}
		/*  */
		public void ToggleMainMenu(){
			if(thisMainMenuUIElement.IsActivated())
				DeactivateMainMenu();
			else
				ActivateMainMenu();
		}
		public void ActivateMainMenu(){
			thisMainMenuUIElement.ActivateRecursively(false);
			thisMainMenuUIElement.EvaluateScrollerFocusRecursively();
		}
		public void DeactivateMainMenu(){
			thisMainMenuUIElement.DeactivateRecursively(false);
		}
		IMainMenuUIElement thisMainMenuUIElement;
		public void SetMainMenuUIElement(IMainMenuUIElement uiElement){
			thisMainMenuUIElement = uiElement;
		}
	}
}
