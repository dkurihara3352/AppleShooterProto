using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface ITutorialBaseUIAdaptor: IUIAdaptor{
		ITutorialBaseUIElement GetTutorialBaseUIElement();
	}
	public class TutorialBaseUIAdaptor: UIAdaptor, ITutorialBaseUIAdaptor{
		protected override IUIElement CreateUIElement(){
			return CreateTutorialBaseUIElement();
		}
		ITutorialBaseUIElement thisTutorialBaseUIElement{
			get{
				return (ITutorialBaseUIElement)thisUIElement;
			}
		}
		ITutorialBaseUIElement CreateTutorialBaseUIElement(){
			TutorialBaseUIElement.IConstArg arg = new TutorialBaseUIElement.ConstArg(
				this,
				activationMode
			);
			return new TutorialBaseUIElement(arg);
		}
		public ITutorialBaseUIElement GetTutorialBaseUIElement(){
			return thisTutorialBaseUIElement;
		}
	}
}

