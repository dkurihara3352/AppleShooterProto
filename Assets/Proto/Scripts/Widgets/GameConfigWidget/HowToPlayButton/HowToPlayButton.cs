using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IHowToPlayButton: IValidatableUIElement{
		void SetHowToPlayPopUp(IPopUp popUp);
		void SetHowToPlayScroller(IUIElementGroupScroller scroller);
	}
	public class HowToPlayButton: ValidatableUIElement, IHowToPlayButton{
		public HowToPlayButton(IConstArg arg): base(arg){
		}

		protected override void OnTapImple(int tapCount){
			base.OnTapImple(tapCount);
			Debug.Log("Hi");
			thisHowToPlayPopUp.Show(false);
			thisHowToPlayScroller.SnapToGroupElement(0);
		}

		IPopUp thisHowToPlayPopUp;
		public void SetHowToPlayPopUp(IPopUp popUp){
			thisHowToPlayPopUp = popUp;
		}
		IUIElementGroupScroller thisHowToPlayScroller;
		public void SetHowToPlayScroller(IUIElementGroupScroller scroller){
			thisHowToPlayScroller = scroller;
		}
	}
}

