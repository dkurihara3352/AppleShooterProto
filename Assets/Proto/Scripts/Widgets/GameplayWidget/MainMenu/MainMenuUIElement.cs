using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using DKUtility;

namespace SlickBowShooting{
	public interface IMainMenuUIElement: IAlphaVisibilityTogglableUIElement{
		void SetStartupManager(IStartupManager manager);
		void ShowForStartup();
	}
	public class MainMenuUIElement: AlphaVisibilityTogglableUIElement, IMainMenuUIElement{
		public MainMenuUIElement(IConstArg arg): base(arg){
			thisShowForStartupProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisMainMenuUIAdaptor.GetProcessTime()
			);
		}
		IMainMenuUIAdaptor thisMainMenuUIAdaptor{
			get{
				return (IMainMenuUIAdaptor)thisUIAdaptor;
			}
		}
		IStartupManager thisStartupManager;
		public void SetStartupManager(IStartupManager manager){
			thisStartupManager = manager;
		}
		public void ShowForStartup(){
			ClearFields();
			thisIsShown = true;
			thisShows = true;
			thisShowForStartupProcessSuite.Start();
		}
		IProcessSuite thisShowForStartupProcessSuite;

		public override void OnProcessUpdate(float deltaTime, float normalizedTime, IProcessSuite suite){
			base.OnProcessUpdate(deltaTime, normalizedTime, suite);
			if(suite == thisShowForStartupProcessSuite){
				UpdateShowness(true, normalizedTime);
			}
		}
		public override void OnProcessExpire(IProcessSuite suite){
			base.OnProcessExpire(suite);
			if(suite == thisShowForStartupProcessSuite){
				thisStartupManager.OnStartUpMainMenuShowComplete();
				Debug.Log("showCompleted!");
			}
		}
	}
}

