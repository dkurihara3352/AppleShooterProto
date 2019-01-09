using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface IBGMPlayer: ISlickBowShootingSceneObject{
		void PlayBGM();
	}
	public class BGMPlayer: SlickBowShootingSceneObject, IBGMPlayer{
		public BGMPlayer(IConstArg arg): base(arg){
		}
		IBGMPlayerAdaptor thisBGMPlayerAdaptor{
			get{
				return (IBGMPlayerAdaptor)thisAdaptor;
			}
		}
		public void PlayBGM(){
			thisBGMPlayerAdaptor.PlayIntroAudioSource();
			// thisBGMPlayerAdaptor.PlayLoopAudioSourceDelayed();
		}
	}
}

