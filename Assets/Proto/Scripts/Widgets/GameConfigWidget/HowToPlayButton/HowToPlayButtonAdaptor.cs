using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IHowToPlayButtonAdaptor: IUIAdaptor{
		IHowToPlayButton GetHowToPlayButton();
	}
	public class HowToPlayButtonAdaptor: UIAdaptor, IHowToPlayButtonAdaptor{
		protected override IUIElement CreateUIElement(){
			return CreateHowToPlayButton();
		}
		IHowToPlayButton thisHowToPlayButton{
			get{
				return (IHowToPlayButton)thisUIElement;
			}
		}
		IHowToPlayButton CreateHowToPlayButton(){
			HowToPlayButton.IConstArg arg = new HowToPlayButton.ConstArg(
				this,
				activationMode
			);
			return new HowToPlayButton(arg);
		}
		public IHowToPlayButton GetHowToPlayButton(){
			return thisHowToPlayButton;
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IPopUp popUp = howToPlayPopUpAdaptor.GetPopUp();
			thisHowToPlayButton.SetHowToPlayPopUp(popUp);
			IUIElementGroupScroller scroller = (IUIElementGroupScroller)howToPlayScrollerAdaptor.GetUIElement();
			thisHowToPlayButton.SetHowToPlayScroller(scroller);
		}
		public PopUpAdaptor howToPlayPopUpAdaptor;
		public UIElementGroupScrollerAdaptor howToPlayScrollerAdaptor;
	}
}

