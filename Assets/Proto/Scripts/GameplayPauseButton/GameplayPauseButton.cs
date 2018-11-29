using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
namespace AppleShooterProto{
	public interface IGameplayPauseButton: IUIElement{
		void SetGameplayPause(IGameplayPause pause);

		void Execute();
	}
	public class GameplayPauseButton : UIElement, IGameplayPauseButton {
		public GameplayPauseButton(IConstArg arg): base(arg){
			thisPauses = arg.pauses;
		}
		bool thisPauses;
		IGameplayPause thisPause;
		public void SetGameplayPause(IGameplayPause pause){
			thisPause = pause;
		}	
		public void Execute(){
			if(thisPauses)
				thisPause.Pause();
			else
				thisPause.Unpause();
		}
		protected override void OnTapImple(int tapCount){
			Execute();
		}


		public new interface IConstArg: UIElement.IConstArg{
			bool pauses{get;}
		}
		public new class ConstArg: UIElement.ConstArg, IConstArg{
			public ConstArg(
				IGameplayPauseButtonAdaptor adaptor,
				ActivationMode activationMode,

				bool pauses
			): base(
				adaptor,
				activationMode
			){
				thisPauses = pauses;
			}
			readonly bool thisPauses;
			public bool pauses{get{return thisPauses;}}
		}
	}
}
