using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IGameplayStartButton: IValidatableUIElement{
		void SetGameplayWidget(IGameplayWidget widget);
		// void SetTutorialManager(ITutorialManager manager);
	}
	public class GameplayStartButton: ValidatableUIElement, IGameplayStartButton{
		public GameplayStartButton(IConstArg arg): base(arg){}
		IGameplayStartButtonAdaptor thisGameplayStartButtonAdaptor{
			get{
				return (IGameplayStartButtonAdaptor)thisAdaptor;
			}
		}
		protected override void OnTapImple(int tapCount){
			base.OnTapImple(tapCount);
			thisGameplayWidget.StartGameplay();
			// thisGameplayStartButtonAdaptor.ShowAD();

		}
		IGameplayWidget thisGameplayWidget;
		public void SetGameplayWidget(IGameplayWidget widget){
			thisGameplayWidget = widget;
		}
	}
}


