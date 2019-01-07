using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IGameConfigWidgetUIAdaptor: IUIAdaptor{
		IGameConfigWidgetUIElement GetGameConfigWidgetUIElement();
	}
	public class GameConfigWidgetUIAdaptor: UIAdaptor, IGameConfigWidgetUIAdaptor{
		protected override IUIElement CreateUIElement(){
			GameConfigWidgetUIElement.IConstArg arg = new GameConfigWidgetUIElement.ConstArg(
				this,
				activationMode
			);
			return new GameConfigWidgetUIElement(arg);
		}
		IGameConfigWidgetUIElement thisGameConfigWidgetUIElement{
			get{
				return (IGameConfigWidgetUIElement)thisUIElement;
			}
		}
		public IGameConfigWidgetUIElement GetGameConfigWidgetUIElement(){
			return thisGameConfigWidgetUIElement;
		}
		public override void SetUpReference(){
			base.SetUpReference();

			IGameConfigWidget gameConfigWidget = CollectGameConfigWidget();
			thisGameConfigWidgetUIElement.SetGameConfigWidget(gameConfigWidget);
		}
		public GameConfigWidgetAdaptor gameConfigWidgetAdaptor;
		IGameConfigWidget CollectGameConfigWidget(){
			return gameConfigWidgetAdaptor.GetGameConfigWidget();
		}
	}

}


