using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IGameplayWidgetUIElement: IUIElement{
		void SetGameplayWidget(IGameplayWidget widget);
	}
	public class GameplayWidgetUIElement: UIElement, IGameplayWidgetUIElement{
		public GameplayWidgetUIElement(IConstArg arg): base(arg){}
		public void SetGameplayWidget(IGameplayWidget widget){
			thisWidget = widget;
		}
		IGameplayWidget thisWidget;
		public override void OnScrollerFocus(){
			base.OnScrollerFocus();
			thisWidget.Activate();
		}
		public override void OnScrollerDefocus(){
			base.OnScrollerDefocus();
			thisWidget.Deactivate();
		}
	}
}


