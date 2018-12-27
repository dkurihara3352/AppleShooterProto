using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using DKUtility;

namespace AppleShooterProto{
	public interface IAlphaVisibilityTogglableUIElement: IUIElement, IProcessHandler{
		void UpdateShowness(
			bool shows,
			float normalizedTime
		);
		void ClearFields();
		void Show(bool instantly);
		void Hide(bool instantly);
		bool IsShown();
		void Toggle(bool instantly);
	}
	public class AlphaVisibilityTogglableUIElement: UIElement, IAlphaVisibilityTogglableUIElement{
		public AlphaVisibilityTogglableUIElement(IConstArg arg): base(arg){
			thisProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisAlphaVisibilityTogglableUIAdaptor.GetProcessTime()
			);
		}
		IAlphaVisibilityTogglableUIAdaptor thisAlphaVisibilityTogglableUIAdaptor{
			get{
				return(IAlphaVisibilityTogglableUIAdaptor)thisUIAdaptor;
			}
		}
		public void UpdateShowness(
			bool shows,
			float normalizedTime
		){
			if(!thisUpdateIsStarted){
				thisInitialAlpha = thisAlphaVisibilityTogglableUIAdaptor.GetAlpha();
				thisTargetAlpha = shows? 1f: 0f;
				thisUpdateIsStarted = true;
			}
			AnimationCurve processCurve = thisAlphaVisibilityTogglableUIAdaptor.GetProcessCurve();
			float processValue = processCurve.Evaluate(normalizedTime);
			float newAlpha = Mathf.Lerp(
				thisInitialAlpha,
				thisTargetAlpha,
				processValue
			);
			thisAlphaVisibilityTogglableUIAdaptor.SetAlpha(newAlpha);
		}
		bool thisUpdateIsStarted = false;
		IProcessSuite thisProcessSuite;
		public void Show(bool instantly){
			ClearFields();
			thisIsShown = true;
			if(instantly)
				UpdateShowness(true, 1f);
			else
				StartShowProcess();
		}
		void StartShowProcess(){
			ClearFields();
			thisShows = true;
			thisProcessSuite.Start();
		}
		protected bool thisShows;
		public void ClearFields(){
			thisUpdateIsStarted = false;
			thisShows = false;
			thisInitialAlpha = 0f;
			thisTargetAlpha = 0f;
		}
		float thisInitialAlpha;
		float thisTargetAlpha;

		public void Hide(bool instantly){
			ClearFields();
			thisIsShown = false;
			if(instantly)
				UpdateShowness(false, 1f);
			else
				StartHideProcess();
		}
		void StartHideProcess(){
			ClearFields();
			thisShows = false;
			thisProcessSuite.Start();
		}
		public bool IsShown(){
			return thisIsShown;
		}
		protected bool thisIsShown;
		public void Toggle(bool instantly){
			if(thisIsShown)
				Hide(instantly);
			else
				Show(instantly);
		}

		public void OnProcessRun(IProcessSuite suite){}
		public virtual void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisProcessSuite){
				UpdateShowness(
					thisShows,
					normalizedTime
				);
			}
		}
		public virtual void OnProcessExpire(IProcessSuite suite){
			if(suite == thisProcessSuite){
				UpdateShowness(thisShows, 1f);
				OnShowComplete();
			}
		}
		protected virtual void OnShowComplete(){

		}

	}
}


