using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IBowLockPane: IUIElement{

	}
	public class BowLockPane: UIElement, IBowLockPane{
		public BowLockPane(IConstArg arg): base(arg){}
	}
}

