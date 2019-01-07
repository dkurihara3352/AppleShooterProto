using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface ITutorialPaneInvertAxisButton: IValidatableUIElement{
		void SetTutorialPane(ITutorialPane pane);
		void SetToggleText(bool toggled);
	}
	public class TutorialPaneInvertAxisButton: ValidatableUIElement, ITutorialPaneInvertAxisButton{
		public TutorialPaneInvertAxisButton(IConstArg arg): base(arg){
		}
		ITutorialPaneInvertAxisButtonAdaptor thisTutorialPaneInvertAxisButtonAdaptor{
			get{
				return (ITutorialPaneInvertAxisButtonAdaptor)thisUIAdaptor;
			}
		}
		protected override void OnTapImple(int tapCount){
			base.OnTapImple(tapCount);
			int axis = thisTutorialPaneInvertAxisButtonAdaptor.GetAxis();
			thisTutorialPane.ToggleInvertAxis(axis);
		}
		ITutorialPane thisTutorialPane;
		public void SetTutorialPane(ITutorialPane pane){
			thisTutorialPane = pane;
		}
		public void SetToggleText(bool toggled){
			if(toggled)
				thisTutorialPaneInvertAxisButtonAdaptor.SetText("ON");
			else
				thisTutorialPaneInvertAxisButtonAdaptor.SetText("OFF");
		}
	}
}

