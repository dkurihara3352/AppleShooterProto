using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IGameplayRootUIAdaptor: IUIAdaptor{
		IGameplayRootUIElement GetMainMenuUIElement();
	}
	public class GameplayRootUIAdaptor : UIAdaptor, IGameplayRootUIAdaptor {

		protected override IUIElement CreateUIElement(){
			GameplayRootUIElement.IConstArg arg = new GameplayRootUIElement.ConstArg(
				this,
				activationMode
			);
			return new GameplayRootUIElement(arg);
		}
		public IGameplayRootUIElement GetMainMenuUIElement(){
			return (IGameplayRootUIElement)thisUIElement;
		}
	}
}
