using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface ISwitchWidgetButton: IValidatableUIElement{
		void SetRootScroller(IUIElementGroupScroller scroller);
	}
	public class SwitchWidgetButton: ValidatableUIElement, ISwitchWidgetButton{
		public SwitchWidgetButton(IConstArg arg): base(arg){}
		protected override void OnTapImple(int tapCount){
			base.OnTapImple(tapCount);
			int widgetIndex = thisSwitchWidgetButtonAdaptor.GetWidgetIndex();
			thisRootScroller.SnapToGroupElement(widgetIndex);
		}
		ISwitchWidgetButtonAdaptor thisSwitchWidgetButtonAdaptor{
			get{
				return (ISwitchWidgetButtonAdaptor)thisUIAdaptor;
			}
		}
		IUIElementGroupScroller thisRootScroller;
		public void SetRootScroller(IUIElementGroupScroller scroller){
			thisRootScroller = scroller;
		}
	}
}


