using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface ITitlePaneAdaptor: IAlphaVisibilityTogglableUIAdaptor{
		ITitlePane GetTitlePane();
	}
	public class TitlePaneAdaptor: AlphaVisibilityTogglableUIAdaptor, ITitlePaneAdaptor{
		protected override IUIElement CreateUIElement(){
			TitlePane.IConstArg arg = new TitlePane.ConstArg(
				this,
				activationMode
			);
			return new TitlePane(arg);
		}
		ITitlePane thisPane{
			get{
				return (ITitlePane)thisUIElement;
			}
		} 
		public ITitlePane GetTitlePane(){
			return thisPane;
		}
	}
}



