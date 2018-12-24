using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IBowLockPane: IUIElement{
		void ActivateThruBackdoor(bool instantly);
		void DeactivateThruBackdoor(bool instantly);
	}
	public class BowLockPane: UIElement, IBowLockPane{
		public BowLockPane(IConstArg arg): base(arg){}
		public override void ActivateRecursively(bool instantly){
			return;
		}
		public void ActivateThruBackdoor(bool instantly){
			ActivateSelf(instantly);
			ActivateAllChildren(instantly);
		}
		public override void DeactivateRecursively(bool instantly){
			return;
		}
		public void DeactivateThruBackdoor(bool instantly){
			DeactivateSelf(instantly);
			DeactivateAllChildren(instantly);
		}
		
	}
}

