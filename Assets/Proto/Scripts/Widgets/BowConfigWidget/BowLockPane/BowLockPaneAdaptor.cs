using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IBowLockPaneAdaptor: IUIAdaptor{
		IBowLockPane GetBowLockPane();
	}
	public class BowLockPaneAdaptor: UIAdaptor, IBowLockPaneAdaptor{
		protected override IUIElement CreateUIElement(){
			BowLockPane.IConstArg arg = new BowLockPane.ConstArg(
				this,
				activationMode
			);
			return new BowLockPane(arg);
		}
		IBowLockPane thisBowLockPane{
			get{
				return (IBowLockPane)thisUIElement;
			}
		}
		public IBowLockPane GetBowLockPane(){
			return thisBowLockPane;
		}
	}
}


