using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IGameplayPauseButtonAdaptor: IUIAdaptor{
		IGameplayPauseButton GetPauseButton();
	}
	public class GameplayPauseButtonAdaptor : UIAdaptor, IGameplayPauseButtonAdaptor {
		public bool pauses = true;
		protected override IUIElement CreateUIElement(){
			GameplayPauseButton.IConstArg arg = new GameplayPauseButton.ConstArg(
				this,
				activationMode
			);
			return new GameplayPauseButton(arg);
		}
		public override void SetUpReference(){
			base.SetUpReference();

			IGameplayPause pause = CollectGameplayPause();
			thisButton.SetGameplayPause(pause);
		}
		IGameplayPauseButton thisButton{
			get{
				return (IGameplayPauseButton)thisUIElement;
			}
		}
		public IGameplayPauseButton GetPauseButton(){
			return thisButton;
		}

		public GameplayPauseAdaptor gameplayPauseAdaptor;
		IGameplayPause CollectGameplayPause(){
			return gameplayPauseAdaptor.GetGameplayPause();
		}
	}
}
