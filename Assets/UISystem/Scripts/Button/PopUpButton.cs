using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IPopUpButton: IUIElement{
		void SetTargetPopUp(IPopUp popUp);
	}
	public class PopUpButton : UIElement, IPopUpButton {
		public PopUpButton(
			IConstArg arg
		): base(
			arg
		){}
		IPopUp thisTargetPopUp = null;
		public void SetTargetPopUp(IPopUp popUp){
			thisTargetPopUp = popUp;
		}
		protected override void OnTapImple(int tapCount){
			ToggePopUp();
		}
		void ToggePopUp(){
			if(thisTargetPopUp.IsShown())
				thisTargetPopUp.Hide(false);
			else
				thisTargetPopUp.Show(false);
		}

		public new interface IConstArg: UIElement.IConstArg{

		}
		public new class ConstArg: UIElement.ConstArg, IConstArg{
			public ConstArg(
				IPopUpButtonAdaptor adaptor,
				ActivationMode activationMode
			): base(
				adaptor,
				activationMode
			){}
		}
	}
}
