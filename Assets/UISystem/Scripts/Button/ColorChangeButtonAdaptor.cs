using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IColorChangeButtonAdaptor: IUIAdaptor{
		IColorChangeButton GetButton();
	}
	public class ColorChangeButtonAdaptor : UIAdaptor, IColorChangeButtonAdaptor {

		public UIAdaptor targetUIElementAdaptor;
		public Color targetColor;
		public UnityEngine.UI.Text targetText;
		IColorChangeButton thisButton{
			get{
				return (IColorChangeButton)thisUIElement;
			}
		}
		public IColorChangeButton GetButton(){
			return thisButton;
		}
		protected override IUIElement CreateUIElement(){
			ColorChangeButton.IConstArg arg = new ColorChangeButton.ConstArg(
				this,
				targetColor,
				targetText
			);
			return new ColorChangeButton(arg);
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IUIElement targetUIElement = targetUIElementAdaptor.GetUIElement();
			thisButton.SetTargetUIElement(targetUIElement);
		}
	}
}
