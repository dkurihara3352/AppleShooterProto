using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IHowToPlayCloseButtonAdaptor: IUIAdaptor{
		IHowToPlayCloseButton GetHowToPlayCloseButton();
	}
	public class HowToPlayCloseButtonAdaptor: UIAdaptor, IHowToPlayCloseButtonAdaptor{
		protected override IUIElement CreateUIElement(){
			return CreateHowToPlayCloseButton();
		}
		IHowToPlayCloseButton CreateHowToPlayCloseButton(){
			HowToPlayCloseButton.IConstArg arg = new HowToPlayCloseButton.ConstArg(
				this,
				activationMode
			);
			return new HowToPlayCloseButton(arg);
		}
		IHowToPlayCloseButton thisHowToPlayCloseButton{
			get{
				return (IHowToPlayCloseButton)thisUIElement;
			}
		}
		public IHowToPlayCloseButton GetHowToPlayCloseButton(){
			return thisHowToPlayCloseButton;
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IPopUp popUp = howToPlayPopUpAdaptor.GetPopUp();
			thisHowToPlayCloseButton.SetHowToPlayPopUp(popUp);
		}
		public PopUpAdaptor howToPlayPopUpAdaptor;
	}
}

