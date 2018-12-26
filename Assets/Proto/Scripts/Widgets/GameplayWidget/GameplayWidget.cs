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

		void StartGameplay();
		void EndGameplay();

		void ShowMainMenu();
		void HideMainMenu();
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
				// StartTargetSpawn();
				// HideMainMenu();
				ActivateGameplayUI();
				StartWaitAndStartGameplay();
				DisableRootScroller();
				DefrostRootElement();
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
				EnableRootScroller();
				FrostRootElement();
			}
			void DeactivateHUD(){
				thisHUD.Deactivate();
			}
			public void StopTargetSpawn(){
				thisPlayerCharacterWaypointsFollower.StopExecutingSpawnEvents();
			}
			void EnableRootScroller(){
				thisRootScroller.EnableInputSelf();
			}
			void FrostRootElement(){
				thisRootElementFrostGlass.Frost();
			}
			public void DeactivateGameplayUI(){
				thisGameplayUIElement.DeactivateRecursively(false);
			}
		/*  */
		public void ToggleMainMenu(){
			if(thisMainMenuUIElement.IsActivated())
				HideMainMenu();
			else
				ShowMainMenu();
		}
		public void ShowMainMenu(){
			thisMainMenuUIElement.ActivateRecursively(false);
			thisMainMenuUIElement.EvaluateScrollerFocusRecursively();
		}
		public void HideMainMenu(){
			thisMainMenuUIElement.DeactivateRecursively(false);
		}
		IMainMenuUIElement thisMainMenuUIElement;
		public void SetMainMenuUIElement(IMainMenuUIElement uiElement){
			thisMainMenuUIElement = uiElement;
		}
	}
}
