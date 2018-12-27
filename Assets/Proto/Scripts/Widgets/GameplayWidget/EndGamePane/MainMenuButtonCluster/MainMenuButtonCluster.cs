using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IMainMenuButtonCluster: IAlphaVisibilityTogglableUIElement{
		void SetEndGamePane(IEndGamePane pane);
		void EnableInput();
		void ResetButtonCluster();
		void StartPassingTapToEndGamePane();
		void EndPassingTapToEndGamePane();
	}
	public class MainMenuButtonCluster: AlphaVisibilityTogglableUIElement, IMainMenuButtonCluster{
		public MainMenuButtonCluster(IConstArg arg): base(arg){
		}
		IMainMenuButtonClusterAdaptor thisMainMenuButtonClusterAdaptor{
			get{
				return (IMainMenuButtonClusterAdaptor)thisUIAdaptor;
			}
		}
		public void EnableInput(){
			EnableInputRecursively();
		}
		public void ResetButtonCluster(){
			// DisableInputRecursively();
			StartPassingTapToEndGamePane();
			Hide(true);
			ClearFields();
		}

		public void StartPassingTapToEndGamePane(){
			DisableInputRecursively();
			EnableInputSelf();
			thisPassesTapToEndGamePane = true;
		}
		public void EndPassingTapToEndGamePane(){
			EnableInputRecursively();
			thisPassesTapToEndGamePane = false;
		}
		bool thisPassesTapToEndGamePane = false;
		protected override void OnTapImple(int tapCount){
			thisEndGamePane.OnTap(tapCount);
		}
		IEndGamePane thisEndGamePane;
		public void SetEndGamePane(IEndGamePane pane){
			thisEndGamePane = pane;
		}
	}
}

