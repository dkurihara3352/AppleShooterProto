using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using UnityBase;

namespace SlickBowShooting{
	public interface IGameplayWidgetAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		IGameplayWidget GetGameplayWidget();
		float GetStartWaitTime();
	}
	public class GameplayWidgetAdaptor: SlickBowShootingMonoBehaviourAdaptor, IGameplayWidgetAdaptor{
		public override void SetUp(){
			GameplayWidget.IConstArg arg = new GameplayWidget.ConstArg(
				this
			);
			thisWidget = new GameplayWidget(arg);
		}
		IGameplayWidget thisWidget;
		public IGameplayWidget GetGameplayWidget(){
			return thisWidget;
		}
		public float startWaitTime = 1f;
		public float GetStartWaitTime(){
			return startWaitTime;
		}

		public override void SetUpReference(){
			IGameplayUIElement gameplayUIElement = gameplayUIElementAdaptor.GetGameplayUIElement();
			thisWidget.SetGameplayUIElement(gameplayUIElement);

			IPlayerCharacterWaypointsFollower follower = playerCharacterWaypointsFollowerAdaptor.GetPlayerCharacterWaypointsFollower();
			thisWidget.SetPlayerCharacterWaypointsFollower(follower);

			IGameStatsTracker tracker = gameStatsTrackerAdaptor.GetTracker();
			thisWidget.SetGameStatsTracker(tracker);

			IHeadUpDisplay hud = headUpDisplayAdaptor.GetHeadUpDisplay();
			thisWidget.SetHeadUpDisplay(hud);

			IUIElementGroupScroller rootScroller = (IUIElementGroupScroller)rootScrollerAdaptor.GetUIElement();
			thisWidget.SetRootScroller(rootScroller);

			// IFrostGlass frostGlass = rootElementFrostGlassAdaptor.GetFrostGlass();
			// thisWidget.SetRootElementFrostGlass(frostGlass);

			IResourcePanel resourcePanel = resourcePanelAdaptor.GetResourcePanel();
			thisWidget.SetResourcePanel(resourcePanel);

			IMainMenuUIElement mainMenuUIElement = mainMenuUIAdaptor.GetMainMenuUIElement();
			thisWidget.SetMainMenuUIElement(mainMenuUIElement);

			IEndGamePane endGamePane = endGamePaneAdaptor.GetEndGamePane();
			thisWidget.SetEndGamePane(endGamePane);

			ITitlePane titlePane = titlePaneAdaptor.GetTitlePane();
			thisWidget.SetTitlePane(titlePane);

			IPlayerDataManager playerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
			thisWidget.SetPlayerDataManager(playerDataManager);

			IShootingDataManager shootingDataManager = shootingDataManagerAdaptor.GetShootingDataManager();
			thisWidget.SetShootingDataManager(shootingDataManager);

			IScoreManager scoreManager = scoreManagerAdaptor.GetScoreManager();
			thisWidget.SetScoreManager(scoreManager);

			ICurrencyManager currencyManager = currencyManagerAdaptor.GetCurrencyManager();
			thisWidget.SetCurrencyManager(currencyManager);

			IHeatManager heatManager = heatManagerAdaptor.GetHeatManager();
			thisWidget.SetHeatManager(heatManager);
			
			ICoreGameplayInputScroller inputScroller = inputScrollerAdaptor.GetInputScroller();
			thisWidget.SetCoreGameplayInputScroller(inputScroller);

			IGameplayPause gameplayPause = gameplayPauseAdaptor.GetGameplayPause();
			thisWidget.SetGameplayPause(gameplayPause);

			IPlayerInputManager playerInputManager = playerInputManagerAdaptor.GetInputManager();
			thisWidget.SetPlayerInputManager(playerInputManager);

			IFrostManager frostManager = frostManagerAdaptor.GetFrostManager();
			thisWidget.SetFrostManager(frostManager);

			ITutorialPane tutorialPane = tutorialPaneAdaptor.GetTutorialPane();
			thisWidget.SetTutorialPane(tutorialPane);

			IColorSchemeManager colorSchemeManager = colorSchemeManagerAdaptor.GetColorSchemeManager();
			thisWidget.SetColorSchemeManager(colorSchemeManager);

			IInterstitialADManager interstitialADManager = interstitialADManagerAdaptor.GetInterstitialADManager();
			thisWidget.SetInterstitialADManager(interstitialADManager);
		}
		public GameplayUIAdaptor gameplayUIElementAdaptor;
		public PlayerCharacterWaypointsFollowerAdaptor playerCharacterWaypointsFollowerAdaptor;
		public GameStatsTrackerAdaptor gameStatsTrackerAdaptor;
		public HeadUpDisplayAdaptor headUpDisplayAdaptor;
		public UIElementGroupScrollerAdaptor rootScrollerAdaptor;
		public ResourcePanelAdaptor resourcePanelAdaptor;
		public MainMenuUIAdaptor mainMenuUIAdaptor;
		public EndGamePaneAdaptor endGamePaneAdaptor;
		public TitlePaneAdaptor titlePaneAdaptor;
		public PlayerDataManagerAdaptor playerDataManagerAdaptor;
		public ShootingDataManagerAdaptor shootingDataManagerAdaptor;
		public ScoreManagerAdaptor scoreManagerAdaptor;
		public CurrencyManagerAdaptor currencyManagerAdaptor;
		public HeatManagerAdaptor heatManagerAdaptor;
		public CoreGameplayInputScrollerAdaptor inputScrollerAdaptor;
		public GameplayPauseAdaptor gameplayPauseAdaptor;
		public PlayerInputManagerAdaptor playerInputManagerAdaptor;
		public FrostManagerAdaptor frostManagerAdaptor;
		public TutorialPaneAdaptor tutorialPaneAdaptor;
		public ColorSchemeManagerAdaptor colorSchemeManagerAdaptor;
		public InterstitialADManagerAdaptor interstitialADManagerAdaptor;
	}
}

