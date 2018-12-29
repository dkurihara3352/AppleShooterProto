using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IGameplayPauseAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IGameplayPause GetGameplayPause();
		void SetTimeScale(float scale);
		float GetTimeScale();
		float GetFrostTime();

	}
	public class GameplayPauseAdaptor : AppleShooterMonoBehaviourAdaptor, IGameplayPauseAdaptor {
		
		public float unpauseTime;
		IGameplayPause thisPause;
		public override void SetUp(){
			GameplayPause.IConstArg arg = new GameplayPause.ConstArg(
				this,
				unpauseTime
			);
			thisPause =  new GameplayPause(arg);
		}
		public IGameplayPause GetGameplayPause(){
			return thisPause;
		}
		public CoreGameplayInputScrollerAdaptor inputScrollerAdaptor;
		public GameplayPauseButtonAdaptor pauseButtonAdaptor;
		public PopUpAdaptor pauseMenuPopUpAdaptor;
		public FrostManagerAdaptor frostManagerAdaptor;

		public override void SetUpReference(){
			ICoreGameplayInputScroller scroller = inputScrollerAdaptor.GetInputScroller();
			thisPause.SetInputScroller(scroller);
			IGameplayPauseButton button = pauseButtonAdaptor.GetPauseButton();
			thisPause.SetGameplayPauseButton(button);

			IPopUp pauseMenuPopUp = pauseMenuPopUpAdaptor.GetPopUp();
			thisPause.SetPauseMenuPopUp(pauseMenuPopUp);

			IFrostManager frostManager = frostManagerAdaptor.GetFrostManager();
			thisPause.SetFrostManager(frostManager);
		}

		public void SetTimeScale(float scale){
			Time.timeScale = scale;
		}
		public float GetTimeScale(){
			return Time.timeScale;
		}
		public float frostTime = .1f;
		public float GetFrostTime(){
			return frostTime;
		}

	}
}
