using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IHowToPlayCloseButton: IValidatableUIElement{
		void SetHowToPlayPopUp(IPopUp popUp);
	}
	public class HowToPlayCloseButton: ValidatableUIElement, IHowToPlayCloseButton{
		public HowToPlayCloseButton(IConstArg arg): base(arg){}

		public void SetHowToPlayPopUp(IPopUp popUp){
			thisHowToPlayPopUp = popUp;
		}
		IPopUp thisHowToPlayPopUp;
		protected override void OnTapImple(int tapCount){
			base.OnTapImple(tapCount);
			thisHowToPlayPopUp.Hide(false);
		}
	}
}


