using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface ITutorialDoneButton: IValidatableUIElement{
		void SetTutorialPane(ITutorialPane pane);
	}
	public class TutorialDoneButton: ValidatableUIElement, ITutorialDoneButton{
		public TutorialDoneButton(IConstArg arg): base(arg){}

		protected override void OnTapImple(int tapCount){
			base.OnTapImple(tapCount);
			thisTutorialPane.EndTutorial();
		}
		ITutorialPane thisTutorialPane;
		public void SetTutorialPane(ITutorialPane pane){
			thisTutorialPane = pane;
		}
	}
}


