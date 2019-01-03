using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
using UISystem;

namespace AppleShooterProto{
	public interface IGameConfigWidget: IAppleShooterSceneObject, IActivationStateImplementor, IActivationStateHandler{
		void SetPlayerDataManager(IPlayerDataManager manager);
		void SetInputScroller(ICoreGameplayInputScroller scorller);
		void SetGameConfigPopText(IPopText popText);
		void SetAxisInversionToggleButtons(IAxisInversionToggleButton[] buttons);
		void SetAudioManager(IAudioManager audioManager);
		void SetBGMVolumeControlScroller(IVolumeControlScroller scroller);
		void SetSFXVolumeControlScroller(IVolumeControlScroller scroller);

		bool PlayerDataIsLoaded();
		void SavePlayerData();
		void ToggleAxisInversion(int axis);
		bool GetAxisInversion(int axis);

		void SetBGMVolume(float volume);
		void SetSFXVolume(float volume);
	}

	public class GameConfigWidget: AppleShooterSceneObject, IGameConfigWidget{
		public GameConfigWidget(IConstArg arg): base(arg){
			thisActivationStateEngine = new ActivationStateEngine(this);
		}
		IActivationStateEngine thisActivationStateEngine;
		public void Activate(){
			thisActivationStateEngine.Activate();
		}
		public void Deactivate(){
			thisActivationStateEngine.Deactivate();
		}
		public bool IsActivated(){
			return thisActivationStateEngine.IsActivated();
		}
		public void ActivateImple(){
			LoadAndSetControls();
			
			thisGameConfigPopText.Pop("Game Config", false);
		}
		void LoadAndSetControls(){
			if(!PlayerDataIsLoaded())
				thisPlayerDataManager.Load();
			SetAxisInversionControl();
			SetVolumeControlScrollerVolume();
		}
		void SetAxisInversionControl(){
			for(int i = 0; i < 2; i ++){
				bool inverts = GetAxisInversion(i);
				thisAxisInversionToggleButtons[i].SetStatus(inverts);
			}
		}
		IAxisInversionToggleButton[] thisAxisInversionToggleButtons;
		public void SetAxisInversionToggleButtons(IAxisInversionToggleButton[] buttons){
			thisAxisInversionToggleButtons = buttons;
		}
		public void DeactivateImple(){
			if(PlayerDataIsLoaded()){
				SetVolumeOnPlayerData();
				SavePlayerData();
			}
			thisGameConfigPopText.Unpop(false);
		}
		IPopText thisGameConfigPopText;
		public void SetGameConfigPopText(IPopText popText){
			thisGameConfigPopText = popText;
		}
		IPlayerDataManager thisPlayerDataManager;
		public void SetPlayerDataManager(IPlayerDataManager manager){
			thisPlayerDataManager = manager;
		}
		public bool PlayerDataIsLoaded(){
			return thisPlayerDataManager.PlayerDataIsLoaded();
		}
		public void SavePlayerData(){
			thisPlayerDataManager.Save();
		}
		public bool GetAxisInversion(int axis){
			return thisPlayerDataManager.GetAxisInversion(axis);
		}
		public void ToggleAxisInversion(int axis){
			bool currentInversion = GetAxisInversion(axis);
			bool newInversion = !currentInversion;
			thisPlayerDataManager.SetAxisInversion(axis, newInversion);
			
			thisInputScroller.SetAxisInversion(axis, newInversion);
			
			thisAxisInversionToggleButtons[axis].SetStatus(newInversion);
		}
		ICoreGameplayInputScroller thisInputScroller;
		public void SetInputScroller(ICoreGameplayInputScroller scroller){
			thisInputScroller = scroller;
		}

		public void SetBGMVolume(float volume){
			thisAudioManager.SetBGMVolume(volume);
		}
		public void SetSFXVolume(float volume){
			thisAudioManager.SetSFXVolume(volume);
		}
		IAudioManager thisAudioManager;
		public void SetAudioManager(IAudioManager manager){
			thisAudioManager = manager;
		}
		void SetVolumeControlScrollerVolume(){
			if(!thisPlayerDataManager.PlayerDataIsLoaded())
				thisPlayerDataManager.Load();
			float bgmVolume = thisPlayerDataManager.GetBGMVolume();
			thisBGMVolumeControlScroller.SetVolumeVisual(bgmVolume);

			float sfxVolume = thisPlayerDataManager.GetSFXVolume();
			thisSFXVolumeControlScroller.SetVolumeVisual(sfxVolume);
		}
		IVolumeControlScroller thisBGMVolumeControlScroller;
		public void SetBGMVolumeControlScroller(IVolumeControlScroller scroller){
			thisBGMVolumeControlScroller = scroller;
		}
		IVolumeControlScroller thisSFXVolumeControlScroller;
		public void SetSFXVolumeControlScroller(IVolumeControlScroller scroller){
			thisSFXVolumeControlScroller = scroller;
		}
		public void SetVolumeOnPlayerData(){
			float bgmVolume = thisAudioManager.GetBGMVolume();
			float sfxVolume = thisAudioManager.GetSFXVolume();
			thisPlayerDataManager.SetBGMVolume(bgmVolume);
			thisPlayerDataManager.SetSFXVolume(sfxVolume);
		}
	}
}

