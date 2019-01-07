using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface ITutorialManagerAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		ITutorialManager GetTutorialManager();
	}
	public class TutorialManagerAdaptor: SlickBowShootingMonoBehaviourAdaptor, ITutorialManagerAdaptor{
		public override void SetUp(){
			thisTutorialManager = CreateTutorialManager();
		}
		ITutorialManager thisTutorialManager;
		ITutorialManager CreateTutorialManager(){
			TutorialManager.IConstArg arg = new TutorialManager.ConstArg(
				this
			);
			return new TutorialManager(arg);
		}
		public ITutorialManager GetTutorialManager(){
			return thisTutorialManager;
		}
		public override void SetUpReference(){
			IMainMenuUIElement mainMenuUIElement = mainMenuUIAdaptor.GetMainMenuUIElement();
			thisTutorialManager.SetMainMenuUIElement(mainMenuUIElement);
			IGameplayUIElement gameplayUIElement = gameplayUIAdaptor.GetGameplayUIElement();
			thisTutorialManager.SetGameplayUIElement(gameplayUIElement);
			IUIElementGroupScroller rootScroller = (IUIElementGroupScroller)rootScrollerAdaptor.GetUIElement();
			thisTutorialManager.SetRootScroller(rootScroller);
			IFrostManager frostManager = frostManagerAdaptor.GetFrostManager();
			thisTutorialManager.SetFrostManager(frostManager);
			ITutorialTitle tutorialTitle = tutorialTitleAdaptor.GetTutorialTitle();
			thisTutorialManager.SetTutorialTitle(tutorialTitle);
			ITutorialBaseUIElement tutorialBaseUIElement = tutorialBaseUIAdaptor.GetTutorialBaseUIElement();
			thisTutorialManager.SetTutorialBaseUIElement(tutorialBaseUIElement);
		}
		public MainMenuUIAdaptor mainMenuUIAdaptor;
		public GameplayUIAdaptor gameplayUIAdaptor;
		public UIElementGroupScrollerAdaptor rootScrollerAdaptor;
		public FrostManagerAdaptor frostManagerAdaptor;
		public TutorialTitleAdaptor tutorialTitleAdaptor;
		public TutorialBaseUIAdaptor tutorialBaseUIAdaptor;
	}
}

