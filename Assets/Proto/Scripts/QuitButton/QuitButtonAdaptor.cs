using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IQuitButtonAdaptor: IUIAdaptor{
		IQuitButton GetQuitButton();
	}
	public class QuitButtonAdaptor: UIAdaptor, IQuitButtonAdaptor{
		protected override IUIElement CreateUIElement(){
			QuitButton.IConstArg arg = new QuitButton.ConstArg(
				this,
				activationMode
			);
			return new QuitButton(arg);
		}
		public IQuitButton GetQuitButton(){
			return thisQuitButton;
		}
		IQuitButton thisQuitButton{
			get{
				return (IQuitButton)thisUIElement;
			}
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IGameplayWidget widget = gameplayWidgetAdaptor.GetGameplayWidget();
			thisQuitButton.SetGameplayWidget(widget);

			IPopUp popUp = pauseMenuPopUpAdaptor.GetPopUp();
			thisQuitButton.SetPauseMenuPopUp(popUp);
		}
		public GameplayWidgetAdaptor gameplayWidgetAdaptor;
		public PopUpAdaptor pauseMenuPopUpAdaptor;
	}
}


