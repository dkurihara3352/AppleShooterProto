using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IAudioManagerAdaptor: ISlickBowShootingMonoBehaviourAdaptor{

		IAudioManager GetAudioManager();
		void SetSFXVolume(float volume);
		void SetBGMVolume(float volume);
	}
	public class AudioManagerAdaptor: SlickBowShootingMonoBehaviourAdaptor, IAudioManagerAdaptor{
		
		public override void SetUp(){
			thisAudioManager = CreateAudioManager();
		}
		IAudioManager thisAudioManager;
		IAudioManager CreateAudioManager(){
			AudioManager.IConstArg arg = new AudioManager.ConstArg(this);
			return new AudioManager(arg);
		}
		public IAudioManager GetAudioManager(){
			return thisAudioManager;
		}
		public AudioSource BGMIntroSource;
		public AudioSource BGMLoopSource;
		public float minBGMVolume;
		public float maxBGMVolume; 
		public void SetBGMVolume(float volume){
			float newVolume = Mathf.Lerp(
				minBGMVolume,
				maxBGMVolume,
				volume
			);
			BGMIntroSource.volume = newVolume;
			BGMLoopSource.volume = newVolume;
		}
		public AudioSourceMinMaxVolumeData[] SFXAudioSourceData;
		public void SetSFXVolume(float volume){
			foreach(AudioSourceMinMaxVolumeData data in SFXAudioSourceData){
				data.SetVolume(volume);
			}
		}
	}
	[System.Serializable]
	public class AudioSourceMinMaxVolumeData{
		public AudioSource SFXAudioSource;
		public float minVolume;
		public float maxVolume;
		public void SetVolume(float volume){
			float thisVolume = Mathf.Lerp(
				minVolume,
				maxVolume,
				volume
			);
			SFXAudioSource.volume = thisVolume;
		}
	}
}

