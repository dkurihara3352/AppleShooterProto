using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IMainMenuButtonCluster: IAlphaVisibilityTogglableUIElement{
		void EnableInput();
		void ResetButtonCluster();
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
			DisableInputRecursively();
			thisMainMenuButtonClusterAdaptor.SetAlpha(0f);
		}
	}
}

