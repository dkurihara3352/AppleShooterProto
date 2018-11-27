using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IPopUpHideButton: IUIElement{
		void SetPopUp(IPopUp popUp);
		void HidePopUp();
	}
	public class PopUpHideButton : UIElement, IPopUpHideButton {
		public PopUpHideButton(IConstArg arg):base(arg){}
		IPopUp thisPopUp;
		public void SetPopUp(IPopUp popUp){
			thisPopUp = popUp;
		}
		public void HidePopUp(){
			thisPopUp.Hide(false);
		}
		public new interface IConstArg: UIElement.IConstArg{
		}
		public new class ConstArg: UIElement.ConstArg, IConstArg{
			public ConstArg(
				IPopUpHideButtonAdaptor adaptor,
				ActivationMode activationMode
			): base(
				adaptor,
				activationMode
			){}
		}
		protected override void OnTapImple(int tapCount){
			HidePopUp();
		}
	}
}
