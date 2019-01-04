using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IGameplayStartButtonAdaptor: IUIAdaptor{
		IGameplayStartButton GetGameplayStartButton();
	}
	public class GameplayStartButtonAdaptor: UIAdaptor, IGameplayStartButtonAdaptor{
		protected override IUIElement CreateUIElement(){
			GameplayStartButton.IConstArg arg = new GameplayStartButton.ConstArg(
				this,
				activationMode
			);
			return new GameplayStartButton(arg);
		}
		IGameplayStartButton thisGameplayStartButton{
			get{
				return (IGameplayStartButton)thisUIElement;
			}
		}
		public IGameplayStartButton GetGameplayStartButton(){
			return thisGameplayStartButton;
		}

		public override void SetUpReference(){
			base.SetUpReference();
			IGameplayWidget gameplayWidget = gameplayWidgetAdaptor.GetGameplayWidget();
			thisGameplayStartButton.SetGameplayWidget(gameplayWidget);
			ITutorialManager tutorialManager = tutorialManagerAdaptor.GetTutorialManager();
			thisGameplayStartButton.SetTutorialManager(tutorialManager);
		}
		public GameplayWidgetAdaptor gameplayWidgetAdaptor;
		public TutorialManagerAdaptor tutorialManagerAdaptor;
	}
}
