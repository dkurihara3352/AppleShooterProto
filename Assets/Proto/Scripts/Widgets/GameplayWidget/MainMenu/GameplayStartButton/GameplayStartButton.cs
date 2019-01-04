using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IGameplayStartButton: IValidatableUIElement{
		void SetGameplayWidget(IGameplayWidget widget);
		void SetTutorialManager(ITutorialManager manager);
	}
	public class GameplayStartButton: ValidatableUIElement, IGameplayStartButton{
		public GameplayStartButton(IConstArg arg): base(arg){}
		protected override void OnTapImple(int tapCount){
			base.OnTapImple(tapCount);
			thisGameplayWidget.StartGameplay();
			// thisTutorialManager.StartTutorial();

		}
		IGameplayWidget thisGameplayWidget;
		public void SetGameplayWidget(IGameplayWidget widget){
			thisGameplayWidget = widget;
		}
		ITutorialManager thisTutorialManager;
		public void SetTutorialManager(ITutorialManager manager){
			thisTutorialManager = manager;
		}
	}
}


