using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IPopUpButtonAdaptor: IUIAdaptor{
		IPopUpButton GetPopUpButton();
	}
	public class PopUpButtonAdaptor : UIAdaptor, IPopUpButtonAdaptor {
		public PopUpAdaptor targetPopUpAdaptor;
		protected override IUIElement CreateUIElement(){
			PopUpButton.IConstArg arg = new PopUpButton.ConstArg(
				this,
				activationMode
			);
			return new PopUpButton(arg);
		}
		IPopUpButton thisPopUpButton{
			get{
				return (IPopUpButton)thisUIElement;
			}
		}
		public IPopUpButton GetPopUpButton(){
			return thisPopUpButton;
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IPopUp targetPopUp = targetPopUpAdaptor.GetPopUp();
			thisPopUpButton.SetTargetPopUp(targetPopUp);
		}
	}
}
