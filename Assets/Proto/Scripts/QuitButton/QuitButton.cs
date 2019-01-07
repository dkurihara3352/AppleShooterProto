using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IQuitButton: IUIElement{
		void SetGameplayWidget(IGameplayWidget widget);
		void SetPauseMenuPopUp(IPopUp popUp);
	}
	public class QuitButton: UIElement, IQuitButton{
		public QuitButton(IConstArg arg): base(arg){

		}
		IQuitButtonAdaptor thisQuitButtonAdaptor{
			get{
				return (IQuitButtonAdaptor)thisUIAdaptor;
			}
		}
		protected override void OnTapImple(int tapCount){
			Time.timeScale = 1f;
			thisPauseMenuPopUpPopUp.Hide(false);
			thisGameplayWidget.EndGameplay();
		}
		IPopUp thisPauseMenuPopUpPopUp;
		public void SetPauseMenuPopUp(IPopUp popUp){
			thisPauseMenuPopUpPopUp = popUp;
		}
		IGameplayWidget thisGameplayWidget;
		public void SetGameplayWidget(IGameplayWidget widget){
			thisGameplayWidget = widget;
		}
	}
}

