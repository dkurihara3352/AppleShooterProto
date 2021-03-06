﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IBowConfigWidgetAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		IBowConfigWidget GetBowConfigWidget();
	}
	public class BowConfigWidgetAdaptor: SlickBowShootingMonoBehaviourAdaptor, IBowConfigWidgetAdaptor{
		public override void SetUp(){
			thisWidget = CreateBowConfigWidget();
		}
		IBowConfigWidget thisWidget;
		public IBowConfigWidget GetBowConfigWidget(){
			return thisWidget;
		}
		IBowConfigWidget CreateBowConfigWidget(){
			BowConfigWidget.IConstArg arg = new BowConfigWidget.ConstArg(
				this
			);
			return new BowConfigWidget(arg);
		}
		public UIElementGroupScrollerAdaptor bowPanelGroupScrollerAdaptor;
		public BowPanelAdaptor[] bowPanelAdaptors;
		IBowPanel[] CollectBowPanels(){
			List<IBowPanel> resultList = new List<IBowPanel>();
			foreach(BowPanelAdaptor adaptor in bowPanelAdaptors){
				resultList.Add(adaptor.GetBowPanel());
			}
			return resultList.ToArray();
		}
		public PlayerDataManagerAdaptor playerDataManagerAdaptor;
		public ResourcePanelAdaptor resourcePanelAdaptor;
		public CurrencyPaneAdaptor currencyPaneAdaptor;
		public BowUnlockButtonAdaptor[] bowUnlockButtonAdaptors;
		IBowUnlockButton[] CollectBowUnlockButtons(){
			List<IBowUnlockButton> resultList = new List<IBowUnlockButton>();
			foreach(IBowUnlockButtonAdaptor adaptor in bowUnlockButtonAdaptors){
				resultList.Add(adaptor.GetBowUnlockButton());
			}
			return resultList.ToArray();
		}
		public override void SetUpReference(){
			IUIElementGroupScroller bowPanelScroller = (IUIElementGroupScroller)bowPanelGroupScrollerAdaptor.GetUIElement();
			thisWidget.SetBowPanelGroupScroller(bowPanelScroller);

			IBowPanel[] bowPanels = CollectBowPanels();
			thisWidget.SetBowPanels(bowPanels);
			foreach(IBowPanel bowPanel in bowPanels)
				bowPanel.SetBowConfigWidget(thisWidget);

			IPlayerDataManager playerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
			thisWidget.SetPlayerDataManager(playerDataManager);

			IBowDataCalculator bowDataCalculator = new BowDataCalculator(playerDataManagerAdaptor);
			thisWidget.SetBowDataCalculator(bowDataCalculator);

			IResourcePanel resourcePanel = resourcePanelAdaptor.GetResourcePanel();
			thisWidget.SetResourcePanel(resourcePanel);

			ICurrencyPane currencyPane = currencyPaneAdaptor.GetCurrencyPane();
			thisWidget.SetCurrencyPane(currencyPane);

			IBowUnlockButton[] unlockButtons = CollectBowUnlockButtons();
			thisWidget.SetBowUnlockButtons(unlockButtons);
			foreach(IBowUnlockButton button in unlockButtons){
				button.SetBowConfigWidget(thisWidget);
			}
			IPopText popText = bowConfigLabelPopTextAdaptor.GetPopText();
			thisWidget.SetBowConfigLabelPopText(popText);
			
		}
		public PopTextAdaptor bowConfigLabelPopTextAdaptor;
	}
}

