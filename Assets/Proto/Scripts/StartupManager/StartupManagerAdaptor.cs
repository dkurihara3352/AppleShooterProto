using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IStartupManagerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IStartupManager GetStartupManager();
		float GetWaitForWarmupReadyTime();
		float GetFadeInTime();
		AnimationCurve GetFadeInProcessCurve();
	}
	public class StartupManagerAdaptor: AppleShooterMonoBehaviourAdaptor, IStartupManagerAdaptor{
		public override void SetUp(){
			StartupManager.IConstArg arg = new StartupManager.ConstArg(
				this
			);
			thisStartupManager = new StartupManager(arg);
		}
		IStartupManager thisStartupManager;
		public IStartupManager GetStartupManager(){
			return thisStartupManager;
		}
		public float waitForWarmupReadyTime = 2f;
		public float GetWaitForWarmupReadyTime(){
			return waitForWarmupReadyTime;
		}
		public float fadeInTime = 2f;
		public float GetFadeInTime(){
			return fadeInTime;
		}
		public AnimationCurve fadeInProcessCurve;
		public AnimationCurve GetFadeInProcessCurve(){
			return fadeInProcessCurve;
		}

		public override void SetUpReference(){
			IFadeImage fadeImage = fadeImageAdaptor.GetFadeImage();
			thisStartupManager.SetFadeImage(fadeImage);

			IMainMenuUIElement mainMenuUIElement = mainMenuUIAdaptor.GetMainMenuUIElement();
			thisStartupManager.SetMainMenuUIElement(mainMenuUIElement);

			IUIElementGroupScroller rootScroller = (IUIElementGroupScroller)rootScrollerAdaptor.GetUIElement();
			thisStartupManager.SetRootScroller(rootScroller);
		}
		public FadeImageAdaptor fadeImageAdaptor;
		public MainMenuUIAdaptor mainMenuUIAdaptor;
		public UIElementGroupScrollerAdaptor rootScrollerAdaptor;
	}
}

