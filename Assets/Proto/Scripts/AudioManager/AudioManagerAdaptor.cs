﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IAudioManagerAdaptor: IAppleShooterMonoBehaviourAdaptor{

		IAudioManager GetAudioManager();
		void SetSFXVolume(float volume);
		void SetBGMVolume(float volume);
	}
	public class AudioManagerAdaptor: AppleShooterMonoBehaviourAdaptor, IAudioManagerAdaptor{
		
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
		public AudioSource BGMAudioSource;
		public float minBGMVolume;
		public float maxBGMVolume; 
		public void SetBGMVolume(float volume){
			float newVolume = Mathf.Lerp(
				minBGMVolume,
				maxBGMVolume,
				volume
			);
			BGMAudioSource.volume = newVolume;
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

