using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IGameConfigWidgetUIElement: IUIElement{
		void SetGameConfigWidget(IGameConfigWidget widget);
	}
	public class GameConfigWidgetUIElement: UIElement, IGameConfigWidgetUIElement{
		public GameConfigWidgetUIElement(IConstArg arg): base(arg){}
		IGameConfigWidget thisGameConfigWidget;
		public void SetGameConfigWidget(IGameConfigWidget widget){
			thisGameConfigWidget = widget;
		}
		public override void OnScrollerFocus(){
			base.OnScrollerFocus();
			thisGameConfigWidget.Activate();
		}
		public override void OnScrollerDefocus(){
			base.OnScrollerDefocus();
			thisGameConfigWidget.Deactivate();
		}
	}
}
