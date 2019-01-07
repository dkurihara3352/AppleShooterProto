using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IBowConfigWidgetUIElement: IUIElement{
		void SetBowConfigWidget(IBowConfigWidget widget);
	}
	public class BowConfigWidgetUIElement: UIElement, IBowConfigWidgetUIElement{
		public BowConfigWidgetUIElement(IConstArg arg): base(arg){}
		IBowConfigWidget thisBowConfigWidget;
		public void SetBowConfigWidget(IBowConfigWidget widget){
			thisBowConfigWidget = widget;
		}
		public override void OnScrollerFocus(){
			base.OnScrollerFocus();
			thisBowConfigWidget.Activate();
		}
		public override void OnScrollerDefocus(){
			base.OnScrollerDefocus();
			thisBowConfigWidget.Deactivate();
		}
	}
}

