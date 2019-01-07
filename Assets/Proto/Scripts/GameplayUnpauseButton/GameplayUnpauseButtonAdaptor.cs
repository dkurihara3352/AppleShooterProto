using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IGameplayUnpauseButtonAdaptor: IUIAdaptor{
		IGameplayUnpauseButton GetGameplayUnpauseButton();
	}
	public class GameplayUnpauseButtonAdaptor: UIAdaptor, IGameplayUnpauseButtonAdaptor{
		protected override IUIElement CreateUIElement(){
			GameplayUnpauseButton.IConstArg arg = new GameplayUnpauseButton.ConstArg(
				this,
				activationMode
			);
			return new GameplayUnpauseButton(arg);
		}
		public IGameplayUnpauseButton GetGameplayUnpauseButton(){
			return thisGameplayUnpauseButton;
		}
		IGameplayUnpauseButton thisGameplayUnpauseButton{
			get{
				return(IGameplayUnpauseButton)thisUIElement;
			}
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IGameplayPause pause = gameplayPauseAdaptor.GetGameplayPause();
			thisGameplayUnpauseButton.SetGameplayPause(pause);
		}
		public GameplayPauseAdaptor gameplayPauseAdaptor;
	}
}


