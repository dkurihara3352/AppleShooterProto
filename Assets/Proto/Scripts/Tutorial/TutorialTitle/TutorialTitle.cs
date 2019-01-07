using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
using DKUtility;

namespace SlickBowShooting{
	public interface ITutorialTitle: ISlickBowShootingSceneObject, IActivationStateHandler, IActivationStateImplementor, ITutorialBaseTapListener, IProcessHandler{
		void SetTutorialBaseUIElement(ITutorialBaseUIElement baseUIElement);
		void SetTutorialManager(ITutorialManager manager);
	}
	public class TutorialTitle: SlickBowShootingSceneObject, ITutorialTitle{
		public TutorialTitle(IConstArg arg)	: base(arg){
			thisActivationStateEngine = new ActivationStateEngine(this);
			thisProcessSuite = new ProcessSuite(
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
			thisTutorialBaseUIElement.CheckAndClearTapListener(this);
			StartHiding();
		}
		ITutorialBaseUIElement thisTutorialBaseUIElement;
		public void SetTutorialBaseUIElement(ITutorialBaseUIElement uie){
			thisTutorialBaseUIElement = uie;
		}
		public void OnTutorialBaseTap(){
			if(thisIsShowing)
				thisProcessSuite.Expire();
			else if (thisIsShown)
				thisTutorialManager.StartLookAroundTutorial();
		}
		bool thisIsShowing = false;
		void StartShowing(){
			thisInitialShowness = 0f;
			thisTargetShowness = 1f;
			thisProcessSuite.Start();
		}
		float thisInitialShowness;
		float thisTargetShowness;
		IProcessSuite thisProcessSuite;
		public void OnProcessRun(IProcessSuite suite){
			if(suite == thisProcessSuite){
				thisIsShowing = true;
			}
		}
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisProcessSuite){
				UpdateShowness(normalizedTime);
			}
		}
		public void OnProcessExpire(IProcessSuite suite){
			if(suite == thisProcessSuite){
				UpdateShowness(1f);
				thisIsShowing = false;
				thisIsShown = true;
			}
		}
		bool thisIsShown = false;
		void UpdateShowness(float value){
			float newShowness = Mathf.Lerp(
				thisInitialShowness,
				thisTargetShowness,
				value
			);
			thisTutorialTitleAdaptor.SetAlpha(newShowness);
		}
		ITutorialManager thisTutorialManager;
		public void SetTutorialManager(ITutorialManager manager){
			thisTutorialManager = manager;
		}
		void StartHiding(){
			thisInitialShowness = 1f;
			thisTargetShowness = 0f;
			thisProcessSuite.Start();
		}
	}
}

