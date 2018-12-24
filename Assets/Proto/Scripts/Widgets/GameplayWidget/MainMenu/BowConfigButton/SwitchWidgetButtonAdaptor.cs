using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface ISwitchWidgetButtonAdaptor: IUIAdaptor{
		ISwitchWidgetButton GetSwitchWidgetButton();
		int GetWidgetIndex();
	}
	public class SwitchWidgetButtonAdaptor: UIAdaptor, ISwitchWidgetButtonAdaptor{
		protected override IUIElement CreateUIElement(){
			SwitchWidgetButton.IConstArg arg = new SwitchWidgetButton.ConstArg(
				this,
				activationMode
			);
			return new SwitchWidgetButton(arg);
		}
		ISwitchWidgetButton thisButton{
			get{
				return (ISwitchWidgetButton)thisUIElement;
			}
		}
		public ISwitchWidgetButton GetSwitchWidgetButton(){
			return thisButton;
		}
		public int widgetIndex;
		public int GetWidgetIndex(){
			return widgetIndex;
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IUIElementGroupScroller rootScroller = (IUIElementGroupScroller)rootScrollerAdaptor.GetUIElement();
			thisButton.SetRootScroller(rootScroller);
		}
		public UIElementGroupScrollerAdaptor rootScrollerAdaptor;
	}
}

