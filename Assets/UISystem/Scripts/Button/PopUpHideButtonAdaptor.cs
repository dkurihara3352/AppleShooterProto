using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IPopUpHideButtonAdaptor: IUIAdaptor{
		IPopUpHideButton GetPopUpHideButton();
	}
	public class PopUpHideButtonAdaptor : UIAdaptor, IPopUpHideButtonAdaptor {

		IPopUpHideButton thisButton{
			get{
				return (IPopUpHideButton)thisUIElement;
			}
		}
		public IPopUpHideButton GetPopUpHideButton(){
			return thisButton;
		}
		protected override IUIElement CreateUIElement(){
			PopUpHideButton.IConstArg arg = new PopUpHideButton.ConstArg(
				this,
				activationMode
			);
			return new PopUpHideButton(arg);
		}
		public override void SetUpReference(){
			IPopUp targetPopUp = CollectTargetPopUp();
			base.SetUpReference();
			thisButton.SetPopUp(targetPopUp);
		}
		public PopUpAdaptor targetPopUpAdaptor;
		IPopUp CollectTargetPopUp(){
			return targetPopUpAdaptor.GetPopUp();
		}
	}
}
