using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IMainMenuUIAdaptor: IAlphaVisibilityTogglableUIAdaptor{
		IMainMenuUIElement GetMainMenuUIElement();
	}
	public class MainMenuUIAdaptor: AlphaVisibilityTogglableUIAdaptor, IMainMenuUIAdaptor{
		protected override IUIElement CreateUIElement(){
			MainMenuUIElement.IConstArg arg = new MainMenuUIElement.ConstArg(
				this,
				activationMode
			);
			return new MainMenuUIElement(arg);
		}
		IMainMenuUIElement thisMainMenuUIElement{
			get{
				return (IMainMenuUIElement)thisUIElement;
			}
		}
		public IMainMenuUIElement GetMainMenuUIElement(){
			return thisMainMenuUIElement;
		}
	}
}
