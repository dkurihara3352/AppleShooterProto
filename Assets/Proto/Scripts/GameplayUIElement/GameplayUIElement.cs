using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IGameplayUIElement: IUIElement{
		void ActivateThruBackdoor(bool instantly);
	}
	public class GameplayUIElement: UIElement, IGameplayUIElement{
		public GameplayUIElement(IConstArg arg): base(arg){}
		public override void ActivateRecursively(bool instantly){
			return;
		}
		public void ActivateThruBackdoor(bool instantly){
			ActivateSelf(instantly);
			ActivateAllChildren(instantly);
		}
	}
}


