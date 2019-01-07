using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IGameplayStartButton: IValidatableUIElement{
		void SetGameplayWidget(IGameplayWidget widget);

		void SetInterstitialADManager(IInterstitialADManager manager);
	}
	public class GameplayStartButton: ValidatableUIElement, IGameplayStartButton{
		public GameplayStartButton(IConstArg arg): base(arg){}
		IGameplayStartButtonAdaptor thisGameplayStartButtonAdaptor{
			get{
				return (IGameplayStartButtonAdaptor)thisAdaptor;
			}
		}
		protected override void OnTapImple(int tapCount){
			base.OnTapImple(tapCount);
			// thisGameplayWidget.StartGameplay();
			if(thisInterstitialADManager.ADIsDue())
				thisInterstitialADManager.StartAD();
			else
				thisGameplayWidget.StartGameplay();
		}
		IGameplayWidget thisGameplayWidget;
		public void SetGameplayWidget(IGameplayWidget widget){
			thisGameplayWidget = widget;
		}
		IInterstitialADManager thisInterstitialADManager;
		public void SetInterstitialADManager(IInterstitialADManager manager){
			thisInterstitialADManager = manager;
		}
	}
}


