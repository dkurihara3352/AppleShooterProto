﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using UnityBase;

namespace AppleShooterProto{
	public interface IGameplayWidgetAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IGameplayWidget GetGameplayWidget();
		float GetStartWaitTime();
	}
	public class GameplayWidgetAdaptor: AppleShooterMonoBehaviourAdaptor, IGameplayWidgetAdaptor{
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

			IFrostGlass frostGlass = rootElementFrostGlassAdaptor.GetFrostGlass();
			thisWidget.SetRootElementFrostGlass(frostGlass);

			IResourcePanel resourcePanel = resourcePanelAdaptor.GetResourcePanel();
			thisWidget.SetResourcePanel(resourcePanel);

			IMainMenuUIElement mainMenuUIElement = mainMenuUIAdaptor.GetMainMenuUIElement();
			thisWidget.SetMainMenuUIElement(mainMenuUIElement);

			IEndGamePane endGamePane = endGamePaneAdaptor.GetEndGamePane();
			thisWidget.SetEndGamePane(endGamePane);

			ITitlePane titlePane = titlePaneAdaptor.GetTitlePane();
			thisWidget.SetTitlePane(titlePane);
		}
		public GameplayUIAdaptor gameplayUIElementAdaptor;
		public PlayerCharacterWaypointsFollowerAdaptor playerCharacterWaypointsFollowerAdaptor;
		public GameStatsTrackerAdaptor gameStatsTrackerAdaptor;
		public HeadUpDisplayAdaptor headUpDisplayAdaptor;
		public UIElementGroupScrollerAdaptor rootScrollerAdaptor;
		public FrostGlassAdaptor rootElementFrostGlassAdaptor;
		public ResourcePanelAdaptor resourcePanelAdaptor;
		public MainMenuUIAdaptor mainMenuUIAdaptor;
		public EndGamePaneAdaptor endGamePaneAdaptor;
		public TitlePaneAdaptor titlePaneAdaptor;
	}
}

