using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
using DKUtility;

namespace AppleShooterProto{
	public interface ITutorialTitle: IAppleShooterSceneObject, IActivationStateHandler, IActivationStateImplementor, ITutorialBaseTapListener, IProcessHandler{
		void SetTutorialBaseUIElement(ITutorialBaseUIElement baseUIElement);
	}
	public class TutorialTitle: AppleShooterSceneObject, ITutorialTitle{
		public TutorialTitle(IConstArg arg)	: base(arg){
			thisActivationStateEngine = new ActivationStateEngine(this);
			thisShowProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisTutorialTitleAdaptor.GetShowProcessTime()
			);
		}
		ITutorialTitleAdaptor thisTutorialTitleAdaptor{
			get{
				return (ITutorialTitleAdaptor)thisAdaptor;
			}
		}
		IActivationStateEngine thisActivationStateEngine;
		public void Activate(){
			thisActivationStateEngine.Activate();
		}
		public void Deactivate(){
			thisActivationStateEngine.Deactivate();
		}
		public bool IsActivated(){
			return thisActivationStateEngine.IsActivated();
		}
		public void ActivateImple(){
			thisTutorialBaseUIElement.SetTutorialTapListener(this);
			StartShowing();
		}
		public void DeactivateImple(){
			
		}
		ITutorialBaseUIElement thisTutorialBaseUIElement;
		public void SetTutorialBaseUIElement(ITutorialBaseUIElement uie){
			thisTutorialBaseUIElement = uie;
		}
		public void OnTutorialBaseTap(){
			if(thisIsShowing)
				thisShowProcessSuite.Expire();
			// else if (thisIsShown)
			// 	thisTutorialManager.StartLookAroundTutorial()
		}
		bool thisIsShowing = false;
		void StartShowing(){
			thisShowProcessSuite.Start();
		}
		IProcessSuite thisShowProcessSuite;
		public void OnProcessRun(IProcessSuite suite){
			if(suite == thisShowProcessSuite){
				thisIsShowing = true;
			}
		}
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisShowProcessSuite){
				UpdateShowness(normalizedTime);
			}
		}
		public void OnProcessExpire(IProcessSuite suite){
			if(suite == thisShowProcessSuite){
				UpdateShowness(1f);
				thisIsShowing = false;
				thisIsShown = true;
			}
		}
		bool thisIsShown = false;
		void UpdateShowness(float showness){
			thisTutorialTitleAdaptor.SetAlpha(showness);
		}
	}
}

