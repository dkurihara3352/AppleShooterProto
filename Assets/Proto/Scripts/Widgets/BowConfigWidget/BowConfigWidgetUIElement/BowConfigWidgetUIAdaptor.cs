using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IBowConfigWidgetUIAdaptor: IUIAdaptor{
		IBowConfigWidgetUIElement GetBowConfigWidgetUIElement();
	}
	public class BowConfigWidgetUIAdaptor: UIAdaptor, IBowConfigWidgetUIAdaptor{
		protected override IUIElement CreateUIElement(){
			BowConfigWidgetUIElement.IConstArg arg = new BowConfigWidgetUIElement.ConstArg(
				this,
				activationMode
			);
			return new BowConfigWidgetUIElement(arg);
		}
		IBowConfigWidgetUIElement thisBowConfigWidgetUIElement{
			get{
				return (IBowConfigWidgetUIElement)thisUIElement;
			}
		}
		public IBowConfigWidgetUIElement GetBowConfigWidgetUIElement(){
			return thisBowConfigWidgetUIElement;
		}
		
		public override void SetUpReference(){
			base.SetUpReference();

			IBowConfigWidget bowConfigWidget = CollectBowConfigWidget();
			thisBowConfigWidgetUIElement.SetBowConfigWidget(bowConfigWidget);
		}
		public BowConfigWidgetAdaptor bowConfigWidgetAdaptor;
		IBowConfigWidget CollectBowConfigWidget(){
			return bowConfigWidgetAdaptor.GetBowConfigWidget();
		}
	}
}


