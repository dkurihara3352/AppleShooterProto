using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface ITutorialPaneInvertAxisButtonAdaptor: IUIAdaptor{
		ITutorialPaneInvertAxisButton GetTutorialPaneInvertAxisButton();
		int GetAxis();
		void SetText(string text);
		
	}
	public class TutorialPaneInvertAxisButtonAdaptor: UIAdaptor, ITutorialPaneInvertAxisButtonAdaptor{
		protected override IUIElement CreateUIElement(){
			return CreateTutorialPaneInvertAxisButton();
		}
		ITutorialPaneInvertAxisButton CreateTutorialPaneInvertAxisButton(){
			TutorialPaneInvertAxisButton.IConstArg arg = new TutorialPaneInvertAxisButton.ConstArg(
				this,
				activationMode
			);
			return new TutorialPaneInvertAxisButton(arg);
		}
		ITutorialPaneInvertAxisButton thisTutorialPaneInvertAxisButton{
			get{
				return (ITutorialPaneInvertAxisButton)thisUIElement;
			}
		}
		public ITutorialPaneInvertAxisButton GetTutorialPaneInvertAxisButton(){
			return thisTutorialPaneInvertAxisButton;
		}
		public int axis;
		public int GetAxis(){
			return axis;
		}
		public void SetText(string text){
			textComp.text = text;
		}
		public UnityEngine.UI.Text textComp;

		public override void SetUpReference(){
			base.SetUpReference();
			ITutorialPane pane = tutorialPaneAdaptor.GetTutorialPane();
			thisTutorialPaneInvertAxisButton.SetTutorialPane(pane);
		}
		public TutorialPaneAdaptor tutorialPaneAdaptor;
	}
}


