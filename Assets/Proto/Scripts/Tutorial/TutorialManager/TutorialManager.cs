using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface ITutorialManager: IAppleShooterSceneObject{
		void SetMainMenuUIElement(IMainMenuUIElement uie);
		void SetGameplayUIElement(IGameplayUIElement uie);
		void SetRootScroller(IUIElementGroupScroller scroller);
		void SetFrostManager(IFrostManager manager);
		void SetTutorialTitle(ITutorialTitle title);
		void SetTutorialBaseUIElement(ITutorialBaseUIElement uie);


		
		void StartTutorial();
		void StartLookAroundTutorial();
	}
	public class TutorialManager: AppleShooterSceneObject, ITutorialManager{
		public TutorialManager(IConstArg arg): base(arg){}
		ITutorialManagerAdaptor thisTutorialManagerAdaptor{
			get{
				return (ITutorialManagerAdaptor)thisAdaptor;
			}
		}
		public void StartTutorial(){
			DeactivateMainMenu();
			ActivateGameplayUI();
			DisableRootScroller();
			Defrost();
			ActivateTutorialTitle();
			ActivateTutorialBaseUIElement();
		}
		public void DeactivateMainMenu(){
			thisMainMenuUIElement.DeactivateRecursively(false);
		}
		IMainMenuUIElement thisMainMenuUIElement;
		public void SetMainMenuUIElement(IMainMenuUIElement uie){
			thisMainMenuUIElement = uie;
		}
		void ActivateGameplayUI(){
			thisGameplayUIElement.ActivateThruBackdoor(instantly: false);
			thisGameplayUIElement.EvaluateScrollerFocusRecursively();
		}
		IGameplayUIElement thisGameplayUIElement;
		public void SetGameplayUIElement(IGameplayUIElement uie){
			thisGameplayUIElement = uie;
		}
		void DisableRootScroller(){
			thisRootScroller.DisableInputSelf();
		}
		IUIElementGroupScroller thisRootScroller;
		public void SetRootScroller(IUIElementGroupScroller scroller){
			thisRootScroller = scroller;
		}
		float frostTime = 1f;
		void Defrost(){
			thisFrostManager.Defrost(frostTime);
		}
		IFrostManager thisFrostManager;
		public void SetFrostManager(IFrostManager manager){
			thisFrostManager = manager;
		}
		void ActivateTutorialTitle(){
			thisTutorialTitle.Activate();
		}
		ITutorialTitle thisTutorialTitle;
		public void SetTutorialTitle(ITutorialTitle title){
			thisTutorialTitle = title;
		}
		void ActivateTutorialBaseUIElement(){
			thisTutorialBaseUIElement.ActivateThruBackdoor(false);
		}
		ITutorialBaseUIElement thisTutorialBaseUIElement;
		public void SetTutorialBaseUIElement(ITutorialBaseUIElement uie){
			thisTutorialBaseUIElement = uie;
		}

		public void StartLookAroundTutorial(){
			thisTutorialTitle.Deactivate();
			thisTutorialBaseUIElement.DisableInputSelf();
		}
	}
}

