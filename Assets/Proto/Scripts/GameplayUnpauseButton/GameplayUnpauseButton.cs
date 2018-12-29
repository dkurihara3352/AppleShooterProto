using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	
	public interface IGameplayUnpauseButton: IUIElement{
		void SetGameplayPause(IGameplayPause pause);
	}
	public class GameplayUnpauseButton: UIElement, IGameplayUnpauseButton{
		public GameplayUnpauseButton(IConstArg arg): base(arg){}
		
		protected override void OnTapImple(int tapCount){
			thisGameplayPause.Unpause();
		}
		IGameplayPause thisGameplayPause;
		public void SetGameplayPause(IGameplayPause pause){
			thisGameplayPause = pause;
		}
	}
}


