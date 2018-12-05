using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IMainMenuUIAdaptor: IUIAdaptor{
		IMainMenuUIElement GetMainMenuUIElement();
	}
	public class MainMenuUIAdaptor : UIAdaptor, IMainMenuUIAdaptor {

		protected override IUIElement CreateUIElement(){
			MainMenuUIElement.IConstArg arg = new MainMenuUIElement.ConstArg(
				this,
				activationMode
			);
			return new MainMenuUIElement(arg);
		}
		public IMainMenuUIElement GetMainMenuUIElement(){
			return (IMainMenuUIElement)thisUIElement;
		}
	}
}
