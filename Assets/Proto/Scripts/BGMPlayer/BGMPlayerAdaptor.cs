using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IBGMPlayerAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		IBGMPlayer GetBGMPlayer();
		void PlayIntroAudioSource();
		void PlayLoopAudioSourceDelayed();
	}
	public class BGMPlayerAdaptor: SlickBowShootingMonoBehaviourAdaptor, IBGMPlayerAdaptor{
		public override void SetUp(){
			thisBGMPlayer = CreateBGMPlayer();
		}
		IBGMPlayer thisBGMPlayer;
		public IBGMPlayer GetBGMPlayer(){
			return thisBGMPlayer;
		}
		IBGMPlayer CreateBGMPlayer(){
			BGMPlayer.IConstArg arg = new BGMPlayer.ConstArg(
				this
			);
			return new BGMPlayer(arg);
		}
		public AudioSource thisIntroAudioSource;
		public AudioSource thisLoopAudioSource;
		public void PlayIntroAudioSource(){
			thisIntroAudioSource.Play();
		}
		public void PlayLoopAudioSourceDelayed(){
			float introClipLength = thisIntroAudioSource.clip.length;
			thisLoopAudioSource.PlayDelayed(introClipLength);
		}
	}
}

