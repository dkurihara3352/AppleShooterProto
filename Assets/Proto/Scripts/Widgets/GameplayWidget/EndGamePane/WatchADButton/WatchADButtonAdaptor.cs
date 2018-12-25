using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IWatchADButtonAdaptor: IAlphaVisibilityTogglableUIAdaptor{
		IWatchADButton GetWatchADButton();
	}
	public class WatchADButtonAdaptor: AlphaVisibilityTogglableUIAdaptor, IWatchADButtonAdaptor{
		protected override IUIElement CreateUIElement(){
			WatchADButton.IConstArg arg = new WatchADButton.ConstArg(
				this,
				activationMode
			);
			return new WatchADButton(arg);
		}
		IWatchADButton thisButton{
			get{
				return (IWatchADButton)thisUIElement;
			}
		}
		public IWatchADButton GetWatchADButton(){
			return thisButton;
		}
	}
}


