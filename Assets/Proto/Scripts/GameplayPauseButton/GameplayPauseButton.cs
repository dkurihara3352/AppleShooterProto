using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
namespace SlickBowShooting{
	public interface IGameplayPauseButton: IUIElement{
		void SetGameplayPause(IGameplayPause pause);

		void Execute();
		void ActivateThruBackdoor();
	}
	public class GameplayPauseButton : UIElement, IGameplayPauseButton {
		public GameplayPauseButton(IConstArg arg): base(arg){
		}
		IGameplayPause thisPause;
		public void SetGameplayPause(IGameplayPause pause){
			thisPause = pause;
		}	
		public void Execute(){
			thisPause.Pause();
		}
		protected override void OnTapImple(int tapCount){
			Execute();
		}
		public override void ActivateRecursively(bool instantly){
			return;
		}
		public void ActivateThruBackdoor(){
			ActivateSelf(false);
			ActivateAllChildren(false);
			EvaluateScrollerFocusRecursively();
		}
	}
}
