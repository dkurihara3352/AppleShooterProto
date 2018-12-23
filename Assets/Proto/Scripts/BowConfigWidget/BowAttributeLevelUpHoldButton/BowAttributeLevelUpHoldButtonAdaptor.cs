using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IBowAttributeLevelUpHoldButtonAdaptor: IHoldButtonAdaptor{
		IBowAttributeLevelUpHoldButton GetBowAttributeLevelUpHoldButton();
		int GetAttributeIndex();
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
	}
}

