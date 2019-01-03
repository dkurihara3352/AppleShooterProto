using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IGameConfigWidgetAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IGameConfigWidget GetGameConfigWidget();
	}
	public class GameConfigWidgetAdaptor: AppleShooterMonoBehaviourAdaptor, IGameConfigWidgetAdaptor{
		public override void SetUp(){
			thisGameConfigWidget = CreateGameConfigWidget();
		}
		IGameConfigWidget thisGameConfigWidget;
		public IGameConfigWidget GetGameConfigWidget(){
			return thisGameConfigWidget;
		}
		IGameConfigWidget CreateGameConfigWidget(){
			GameConfigWidget.IConstArg arg = new GameConfigWidget.ConstArg(
				this
			);
			return new GameConfigWidget(arg);
		}
		void OnApplicationQuit(){
			Debug.Log("quit, saving");
			if(thisGameConfigWidget.PlayerDataIsLoaded())
				thisGameConfigWidget.SavePlayerData();
		}

		public override void SetUpReference(){
			IPopText gameConfigPopText = gameConfigPopTextAdaptor.GetPopText();
			thisGameConfigWidget.SetGameConfigPopText(gameConfigPopText);
			IPlayerDataManager playerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
			thisGameConfigWidget.SetPlayerDataManager(playerDataManager);
			IAxisInversionToggleButton[] axisInversionToggleButtons = CollectAxisInversionToggleButtons();
			thisGameConfigWidget.SetAxisInversionToggleButtons(axisInversionToggleButtons);

			ICoreGameplayInputScroller inputScroller = inputScrollerAdaptor.GetInputScroller();
			thisGameConfigWidget.SetInputScroller(inputScroller);

			IAudioManager audioManager = audioManagerAdaptor.GetAudioManager();
			thisGameConfigWidget.SetAudioManager(audioManager);

			IVolumeControlScroller bgmVolumeControlScroller = BGMVolumeControlScrollerAdaptor.GetVolumeControlScroller();
			thisGameConfigWidget.SetBGMVolumeControlScroller(bgmVolumeControlScroller);
			IVolumeControlScroller sfxVolumeControlScroller = SFXVolumeControlScrollerAdaptor.GetVolumeControlScroller();
			thisGameConfigWidget.SetSFXVolumeControlScroller(sfxVolumeControlScroller);
		}
		public PopTextAdaptor gameConfigPopTextAdaptor;
		public PlayerDataManagerAdaptor playerDataManagerAdaptor;
		public AxisInversionToggleButtonAdaptor[] axisInversionToggleButtonAdaptors;
		IAxisInversionToggleButton[] CollectAxisInversionToggleButtons(){
			IAxisInversionToggleButton[] result = new IAxisInversionToggleButton[2];
			for(int i = 0; i < 2; i ++){
				result[i] = axisInversionToggleButtonAdaptors[i].GetAxisInversionToggleButton();
			}
			return result;
		}

		public CoreGameplayInputScrollerAdaptor inputScrollerAdaptor;
		public AudioManagerAdaptor audioManagerAdaptor;
		public VolumeControlScrollerAdaptor BGMVolumeControlScrollerAdaptor;
		public VolumeControlScrollerAdaptor SFXVolumeControlScrollerAdaptor;
	}	
}
