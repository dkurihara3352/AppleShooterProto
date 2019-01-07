using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using UnityEngine.UI;
namespace SlickBowShooting{
	public interface IBowAttributeLevelUpHoldButtonAdaptor: IHoldButtonAdaptor{
		IBowAttributeLevelUpHoldButton GetBowAttributeLevelUpHoldButton();
		int GetAttributeIndex();
		void SetLabelText(string text);
		void SetCostText(string text);
	}
	public class BowAttributeLevelUpHoldButtonAdaptor: HoldButtonAdaptor, IBowAttributeLevelUpHoldButtonAdaptor{
		protected override IUIElement CreateUIElement(){
			BowAttributeLevelUpHoldButton.IConstArg arg = new BowAttributeLevelUpHoldButton.ConstArg(
				this,
				activationMode
			);
			return new BowAttributeLevelUpHoldButton(arg);
		}
		IBowAttributeLevelUpHoldButton thisButton{
			get{
				return (IBowAttributeLevelUpHoldButton)thisUIElement;
			}
		}
		public IBowAttributeLevelUpHoldButton GetBowAttributeLevelUpHoldButton(){
			return thisButton;
		}
		public int attributeIndex;
		public int GetAttributeIndex(){
			return attributeIndex;
		}
		public BowConfigWidgetAdaptor bowConfigWidgetAdaptor;
		public override void SetUpReference(){
			base.SetUpReference();
			IBowConfigWidget widget = bowConfigWidgetAdaptor.GetBowConfigWidget();
			thisButton.SetBowConfigWidget(widget);
		}
		public Text labelText;
		public void SetLabelText(string text){
			labelText.text = text;
		}
		public Text costText;
		public void SetCostText(string text){
			costText.text = text;
		}
	}
}

