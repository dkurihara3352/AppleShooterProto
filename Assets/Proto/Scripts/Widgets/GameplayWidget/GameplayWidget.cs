using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
using DKUtility;
using UISystem;

namespace AppleShooterProto{
	public interface IGameplayWidget: IAppleShooterSceneObject, IActivationStateHandler, IActivationStateImplementor, IProcessHandler{
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

		void StartGameplay();
		void EndGameplay();

		void ActivateMainMenu();
		void DeactivateMainMenu();
		void ToggleMainMenu();
	}
	public class GameplayWidget: AppleShooterSceneObject, IGameplayWidget{
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
				
				DeactivateMainMenu();
				SetUpShootingData();

				ActivateGameplayUI();
				StartWaitAndStartGameplay();
				DisableRootScroller();
				DefrostRootElement();
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
			void StartDelayedGameplay(){
				StartTargetSpawn();
				ResetStats();
				LoadAndSetHighScore();
				// ActivateGameplayUI();
				ActivateHUD();
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

		/* End gameplay */
			public void EndGameplay(){	
				DeactivateGameplayUI();
				DeactivateHUD();
				StopTargetSpawn();
				// EnableRootScroller();
				FrostRootElement();

				GameResultStats stats = CreateGameResultStats();
				MightWannaSaveDataHere(stats);

				ActivateMainMenu();
				HideTitle();
				ActivateEndGamePane();
				ResetEndGamePane();

				FeedEndGamePane(stats);
				StartEndSequence();
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
				int newCurrency = currency + stats.gainedCurrency;

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
			}
			GameResultStats CreateGameResultStats(){
				int score = thisScoreManager.GetScore();
				int highScore = thisScoreManager.GetHighScore();
				int gainedCurrency = thisCurrencyManager.GetGainedCurrency();
				GameResultStats stats = new GameResultStats();
				stats.score = score;
				stats.highScore = highScore;
				stats.gainedCurrency = gainedCurrency;

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
				int scoreCurrencyBonus = CalculateScoreCurrencyBonus(stats.score);
				thisEndGamePane.FeedStats(
					stats.score,
					stats.highScore,
					stats.gainedCurrency,
					scoreCurrencyBonus
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
