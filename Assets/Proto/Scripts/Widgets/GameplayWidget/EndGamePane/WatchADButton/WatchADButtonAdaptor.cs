﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IWatchADButtonAdaptor: IAlphaVisibilityTogglableUIAdaptor{
		IWatchADButton GetWatchADButton();
		void SetLabelText(string text);
	}
	public class WatchADButtonAdaptor: AlphaVisibilityTogglableUIAdaptor, IWatchADButtonAdaptor{
		protected override IUIElement CreateUIElement(){
			WatchADButton.IConstArg arg = new WatchADButton.ConstArg(
				this,
				activationMode
			);
			return new WatchADButton(arg);
		}
		IWatchADButton thisButton{
			get{
				return (IWatchADButton)thisUIElement;
			}
		}
		public IWatchADButton GetWatchADButton(){
			return thisButton;
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IEndGamePane pane = endGamePaneAdaptor.GetEndGamePane();
			thisButton.SetEndGamePane(pane);
			IDoubleEarnedCrystalsADManager adManager = adManagerAdaptor.GetADManager();
			thisButton.SetDoubleEarnedCrystalsADManager(adManager);
		}
		public EndGamePaneAdaptor endGamePaneAdaptor;
		public UnityEngine.UI.Text labelTextComp;
		public void SetLabelText(string text){
			labelTextComp.text = text;
		}
		public DoubleEarnedCrystalsADManagerAdaptor adManagerAdaptor;
	}
}


