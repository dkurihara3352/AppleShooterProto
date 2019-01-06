using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
using UISystem;

namespace AppleShooterProto{
	public interface IStartupManager: IAppleShooterSceneObject, IProcessHandler{
		void SetFadeImage(IFadeImage image);
		void SetMainMenuUIElement(IMainMenuUIElement mainMenuUIElement);
		void SetRootScroller(IUIElementGroupScroller scroller);
		void SetColorSchemeManager(IColorSchemeManager manager);
		void StartStartupSequence();
		void OnStartUpMainMenuShowComplete();
	}
	public class StartupManager: AppleShooterSceneObject, IStartupManager{
		public StartupManager(IConstArg arg): base(arg){
			thisWaitForWarmupReadyProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisStartupManagerAdaptor.GetWaitForWarmupReadyTime()
			);
			thisFadeInProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisStartupManagerAdaptor.GetFadeInTime()
			);
		}
		IStartupManagerAdaptor thisStartupManagerAdaptor{
			get{
				return (IStartupManagerAdaptor)thisAdaptor;
			}
		}
		public void StartStartupSequence(){
			thisRootScroller.DisableInputSelf();
			thisMainMenuUIElement.DeactivateRecursively(true);
			thisColorSchemeManager.ChangeColorScheme(0, 0f);
			thisWaitForWarmupReadyProcessSuite.Start();
		}
		IMainMenuUIElement thisMainMenuUIElement;
		public void SetMainMenuUIElement(IMainMenuUIElement mainMenuUIElement){
			thisMainMenuUIElement = mainMenuUIElement;
		}
		IUIElementGroupScroller thisRootScroller;
		public void SetRootScroller(IUIElementGroupScroller rootScroller){
			thisRootScroller = rootScroller;
		}
		IProcessSuite thisWaitForWarmupReadyProcessSuite;
		public void OnProcessRun(IProcessSuite suite){}
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisFadeInProcessSuite){
				UpdateFadeInProcess(normalizedTime);
			}
		}
		public void OnProcessExpire(IProcessSuite suite){
			if(suite == thisWaitForWarmupReadyProcessSuite){
				StartFadeInProcess();
				
			}else if(suite == thisFadeInProcessSuite){
				thisMainMenuUIElement.ActivateRecursively(true);
				thisMainMenuUIElement.DisableInputRecursively();
				thisMainMenuUIElement.EvaluateScrollerFocusRecursively();
				thisMainMenuUIElement.Hide(true);
				thisMainMenuUIElement.ShowForStartup();
			}
		}
		public void OnStartUpMainMenuShowComplete(){
			thisRootScroller.EnableInputSelf();
			thisMainMenuUIElement.EnableInputRecursively();
		}
		void StartFadeInProcess(){
			thisFadeInProcessSuite.Start();
		}
		IProcessSuite thisFadeInProcessSuite;
		float thisInitialFade = 0f;
		float thisTargetFade = 1f;
		void UpdateFadeInProcess(float normalizedTime){
			AnimationCurve fadeInProcessCurve = thisStartupManagerAdaptor.GetFadeInProcessCurve();
			float processValue = fadeInProcessCurve.Evaluate(normalizedTime);
			float newFadeness = Mathf.Lerp(
				thisInitialFade,
				thisTargetFade,
				processValue
			);
			thisFadeImage.SetFadeness(newFadeness);
		}
		IFadeImage thisFadeImage;
		public void SetFadeImage(IFadeImage image){
			thisFadeImage = image;
		}

		IColorSchemeManager thisColorSchemeManager;
		public void SetColorSchemeManager(IColorSchemeManager manager){
			thisColorSchemeManager = manager;
		}

	}
}


