using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface ITutorialDoneButtonAdaptor: IUIAdaptor{
		ITutorialDoneButton GetTutorialDoneButton();
	}
	public class TutorialDoneButtonAdaptor: UIAdaptor, ITutorialDoneButtonAdaptor{
		protected override IUIElement CreateUIElement(){
			return CreateTutorialDoneButton();
		}
		ITutorialDoneButton CreateTutorialDoneButton(){
			TutorialDoneButton.IConstArg arg = new TutorialDoneButton.ConstArg(
				this,
				activationMode
			);
			return new TutorialDoneButton(arg);
		}
		ITutorialDoneButton thisTutorialDoneButton{
			get{
				return (ITutorialDoneButton)thisUIElement;
			}
		}
		public ITutorialDoneButton GetTutorialDoneButton(){
			return thisTutorialDoneButton;
		}
		public override void SetUpReference(){
			base.SetUpReference();
			ITutorialPane pane = tutorialPaneAdaptor.GetTutorialPane();
			thisTutorialDoneButton.SetTutorialPane(pane);
		}
		public TutorialPaneAdaptor tutorialPaneAdaptor;
	}	
}


