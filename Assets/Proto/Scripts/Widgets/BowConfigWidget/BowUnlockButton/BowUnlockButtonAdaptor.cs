using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IBowUnlockButtonAdaptor: IHoldButtonAdaptor{
		IBowUnlockButton GetBowUnlockButton();
		int GetPanelIndex();
		void SetLabelText(string text);
		void SetCostText(string text);
	}
	public class BowUnlockButtonAdaptor: HoldButtonAdaptor, IBowUnlockButtonAdaptor{
		protected override IUIElement CreateUIElement(){
			BowUnlockButton.IConstArg arg = new BowUnlockButton.ConstArg(
				this,
				activationMode
			);
			return new BowUnlockButton(arg);
		}
		IBowUnlockButton thisButton{
			get{
				return (IBowUnlockButton)thisUIElement;
			}
		}
		public IBowUnlockButton GetBowUnlockButton(){
			return thisButton;
		}
		// public BowConfigWidgetAdaptor bowConfigWdigetAdaptor;
		// public override void SetUpReference(){
		// 	base.SetUpReference();
		// 	IBowConfigWidget widget = bowConfigWdigetAdaptor.GetBowConfigWidget();
		// 	thisButton.SetBowConfigWidget(widget);
		// }
		public int panelIndex;
		public int GetPanelIndex(){
			return panelIndex;
		}
		public UnityEngine.UI.Text textComp;
		public void SetLabelText(string text){
			textComp.text = text;
		}
		public UnityEngine.UI.Text costTextComp;
		public void SetCostText(string text){
			costTextComp.text = text;
		}
	}
}

