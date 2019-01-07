using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IAudioManager: ISlickBowShootingSceneObject{
		float GetSFXVolume();
		void SetSFXVolume(float volume);
		float GetBGMVolume();
		void SetBGMVolume(float volume);
	}
	public class AudioManager: SlickBowShootingSceneObject, IAudioManager{
		public AudioManager(IConstArg arg): base(arg){}
		public IAudioManagerAdaptor thisAudioManagerAdaptor{
			get{
				return (IAudioManagerAdaptor)thisAdaptor;
			}
		}
		float thisSFXVolume;
		public float GetSFXVolume(){
			return thisSFXVolume;
		}
		public void SetSFXVolume(float volume){
			thisSFXVolume = volume;
			thisAudioManagerAdaptor.SetSFXVolume(volume);
		}
		float thisBGMVolume;
		public float GetBGMVolume(){
			return thisBGMVolume;
		}
		public void SetBGMVolume(float volume){
			thisBGMVolume = volume;
			thisAudioManagerAdaptor.SetBGMVolume(volume);
		}
	}
}

