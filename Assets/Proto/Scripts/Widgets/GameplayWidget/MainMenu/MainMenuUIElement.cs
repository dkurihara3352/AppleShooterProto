using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IMainMenuUIElement: IAlphaVisibilityTogglableUIElement{
		void SetStartupManager(IStartupManager manager);
	}
	public class MainMenuUIElement: AlphaVisibilityTogglableUIElement, IMainMenuUIElement{
		public MainMenuUIElement(IConstArg arg): base(arg){}
		IStartupManager thisStartUpManager;
		public void SetStartupManager(IStartupManager manager){
			thisStartUpManager = manager;
		}
	}
}

