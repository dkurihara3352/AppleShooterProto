using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IGameplayUIAdaptor: IUIAdaptor{
		IGameplayUIElement GetGameplayUIElement();
	}
	public class GameplayUIAdaptor: UIAdaptor, IGameplayUIAdaptor{
		protected override IUIElement CreateUIElement(){
			GameplayUIElement.IConstArg arg = new GameplayUIElement.ConstArg(
				this,
				activationMode
			);
			return new GameplayUIElement(arg);
		}
		IGameplayUIElement thisGameplayUIElement{
			get{
				return (IGameplayUIElement)thisUIElement;
			}
		}
		public IGameplayUIElement GetGameplayUIElement(){
			return thisGameplayUIElement;
		}
	}
}


