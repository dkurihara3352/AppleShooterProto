using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IGameplayWidgetUIAdaptor: IUIAdaptor{
		IGameplayWidgetUIElement GetGameplayWidgetUIElement();
	}
	public class GameplayWidgetUIAdaptor: UIAdaptor, IGameplayWidgetUIAdaptor{
		protected override IUIElement CreateUIElement(){
			GameplayWidgetUIElement.IConstArg arg = new GameplayWidgetUIElement.ConstArg(
				this,
				activationMode
			);
			return new GameplayWidgetUIElement(arg);
		}
		IGameplayWidgetUIElement thisWidgetUIElement{
			get{
				return (IGameplayWidgetUIElement)thisUIElement;
			}
		}
		public IGameplayWidgetUIElement GetGameplayWidgetUIElement(){
			return thisWidgetUIElement;
		}

		public override void SetUpReference(){
			base.SetUpReference();
			IGameplayWidget widget = gameplayWidgetAdaptor.GetGameplayWidget();
			thisWidgetUIElement.SetGameplayWidget(widget);
		}
		public GameplayWidgetAdaptor gameplayWidgetAdaptor;
	}
}


