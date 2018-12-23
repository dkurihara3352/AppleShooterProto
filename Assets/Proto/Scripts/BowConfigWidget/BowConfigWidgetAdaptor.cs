using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IBowConfigWidgetAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IBowConfigWidget GetBowConfigWidget();
	}
	public class BowConfigWidgetAdaptor: AppleShooterMonoBehaviourAdaptor, IBowConfigWidgetAdaptor{
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
		public override void SetUpReference(){
			IUIElementGroupScroller bowPanelScroller = (IUIElementGroupScroller)bowPanelGroupScrollerAdaptor.GetUIElement();
			thisWidget.SetBowPanelGroupScroller(bowPanelScroller);

			IBowPanel[] bowPanels = CollectBowPanels();
			thisWidget.SetBowPanels(bowPanels);
			foreach(IBowPanel bowPanel in bowPanels)
				bowPanel.SetBowConfigWidget(thisWidget);

			IPlayerDataManager playerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
			thisWidget.SetPlayerDataManager(playerDataManager);

			IResourcePanel resourcePanel = resourcePanelAdaptor.GetResourcePanel();
			thisWidget.SetResourcePanel(resourcePanel);
		}
	}
}

