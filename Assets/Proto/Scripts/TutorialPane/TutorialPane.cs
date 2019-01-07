using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface ITutorialPane: IUIElement{
		void SetTutorialLabelPopText(IPopText popText);
		void SetCoreGameplayInputScroller(ICoreGameplayInputScroller scroller);
		void SetTutorialPaneInvertAxisButtons(ITutorialPaneInvertAxisButton[] buttons);
		void SetPlayerDataManager(IPlayerDataManager manager);
		void SetGameplayWidget(IGameplayWidget widget);

		void ActivateThruBackdoor();
		void EndTutorial();

		void ToggleInvertAxis(int axis);
	}
	public class TutorialPane: UIElement, ITutorialPane{
		public TutorialPane(IConstArg arg): base(arg){
		}
		ITutorialPaneAdaptor thisTutorialPaneAdaptor{
			get{
				return (ITutorialPaneAdaptor)thisUIAdaptor;
			}
		}
		public override void ActivateRecursively(bool instantly){
			return;
		}
		public void ActivateThruBackdoor(){
			ActivateSelf(false);
			ActivateAllChildren(false);
			EvaluateScrollerFocusRecursively();
			thisTutorialLabelPopText.Pop(
				"Tutorial",
				false
			);
			SetTutorialPaneInvertAxisButtonToggleTexts();
		}
		protected override void OnUIActivate(){
			thisTutorialPaneAdaptor.ToggleRaycastBlock(true);
		}
		protected override void OnUIDeactivate(){
			thisTutorialPaneAdaptor.ToggleRaycastBlock(false);
		}
		IPopText thisTutorialLabelPopText;
		public void SetTutorialLabelPopText(IPopText popText){
			thisTutorialLabelPopText = popText;
		}

		public void EndTutorial(){
			SetPlayerDataTutorialIsDone();
			DeactivateRecursively(false);
			SaveAxisInversion();
			thisGameplayWidget.StartDelayedGameplay();
		}
		IGameplayWidget thisGameplayWidget;
		public void SetGameplayWidget(IGameplayWidget widget){
			thisGameplayWidget = widget;
		}

		public void ToggleInvertAxis(int axis){
			bool toggled = thisInputScroller.GetAxisInversion(axis);
			thisInputScroller.SetAxisInversion(axis, !toggled);
			thisTutorialPaneInvertAxisButtons[axis].SetToggleText(!toggled);
		}
		ICoreGameplayInputScroller thisInputScroller;
		public void SetCoreGameplayInputScroller(ICoreGameplayInputScroller scroller){
			thisInputScroller = scroller;
		}
		ITutorialPaneInvertAxisButton[] thisTutorialPaneInvertAxisButtons;
		public void SetTutorialPaneInvertAxisButtons(ITutorialPaneInvertAxisButton[] buttons){
			thisTutorialPaneInvertAxisButtons = buttons;
		}
		void SetTutorialPaneInvertAxisButtonToggleTexts(){
			for(int i = 0; i < 2; i ++){
				bool toggled = thisInputScroller.GetAxisInversion(i);
				thisTutorialPaneInvertAxisButtons[i].SetToggleText(toggled);
			}
		}
		void SaveAxisInversion(){
			if(!thisPlayerDataManager.PlayerDataIsLoaded())
				thisPlayerDataManager.Load();
			for(int i = 0; i < 2; i++){
				bool toggled = thisInputScroller.GetAxisInversion(i);
				thisPlayerDataManager.SetAxisInversion(i, toggled);
			}
			thisPlayerDataManager.Save();
		}
		IPlayerDataManager thisPlayerDataManager;
		public void SetPlayerDataManager(IPlayerDataManager manager){
			thisPlayerDataManager = manager;
		}
		void SetPlayerDataTutorialIsDone(){
			if(!thisPlayerDataManager.PlayerDataIsLoaded())
				thisPlayerDataManager.Load();
			thisPlayerDataManager.SetTutorialIsDone();
		}
	}
}

