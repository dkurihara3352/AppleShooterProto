using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IMainMenuUIElement: IUIElement{}
	public class MainMenuUIElement : UIElement, IMainMenuUIElement {
		public MainMenuUIElement(
			IConstArg arg
		): base(
			arg
		){
		}
		public override void ActivateRecursively(bool instantly){
			ActivateSelf(instantly);
		}
		public override void OnScrollerFocus(){
			base.OnScrollerFocus();
			ActivateRecursively(false);
		}
		public new interface IConstArg: UIElement.IConstArg{}
		public new class ConstArg: UIElement.ConstArg, IConstArg{
			public ConstArg(
				IMainMenuUIAdaptor adaptor,
				ActivationMode activationMode
			): base(
				adaptor,
				activationMode
			){
			}
		}
	}
}
