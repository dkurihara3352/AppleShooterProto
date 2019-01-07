using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IMainMenuButtonClusterAdaptor: IAlphaVisibilityTogglableUIAdaptor{
		IMainMenuButtonCluster GetMainMenuButtonCluster();
	}
	public class MainMenuButtonClusterAdaptor: AlphaVisibilityTogglableUIAdaptor, IMainMenuButtonClusterAdaptor{
		protected override IUIElement CreateUIElement(){
			MainMenuButtonCluster.IConstArg arg = new MainMenuButtonCluster.ConstArg(
				this,
				activationMode
			);
			return new MainMenuButtonCluster(arg);
		}
		IMainMenuButtonCluster thisButtonCluster{
			get{
				return (IMainMenuButtonCluster)thisUIElement;
			}
		}
		public IMainMenuButtonCluster GetMainMenuButtonCluster(){
			return thisButtonCluster;
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IEndGamePane pane = endGamePaneAdaptor.GetEndGamePane();
			thisButtonCluster.SetEndGamePane(pane);
		}
		public EndGamePaneAdaptor endGamePaneAdaptor;
	}
}
