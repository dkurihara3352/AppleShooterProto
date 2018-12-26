using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IGameplayRootUIElement: IUIElement{}
	public class GameplayRootUIElement : UIElement, IGameplayRootUIElement {
		public GameplayRootUIElement(
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
				IGameplayRootUIAdaptor adaptor,
				ActivationMode activationMode
			): base(
				adaptor,
				activationMode
			){
			}
		}
	}
}
