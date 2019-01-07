using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using DKUtility;

namespace SlickBowShooting{
	public interface IPopText: IUIElement, IProcessHandler{
		void Pop(
			string text,
			bool instantly
		);
		void Unpop(
			bool instantly
		);
	}
	public class PopText: UIElement, IPopText{

		public PopText(IConstArg arg): base(arg){
			thisShowProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisPopTextAdaptor.GetProcessTime()
			);
			thisHideProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisPopTextAdaptor.GetProcessTime()
			);
		}
		IProcessSuite thisShowProcessSuite;
		IProcessSuite thisHideProcessSuite;
		IPopTextAdaptor thisPopTextAdaptor{
			get{
				return (IPopTextAdaptor)thisUIAdaptor;
			}
		}
		public void Pop(
			string text,
			bool instantly
		){
			thisPopTextAdaptor.SetText(text);
			if(instantly){
				ShowText();
			}else{
				StartShowTextProcess();
			}
		}
		public void Unpop(
			bool instantly
		){
			if(instantly){
				HideText();
			}else{
				StartHideTextProcess();
			}
		}
		void ShowText(){
			StopAllProcess();
			thisPopTextAdaptor.SetAlpha(1f);
			thisPopTextAdaptor.SetScale(1f);
		}
		void HideText(){
			StopAllProcess();
			thisPopTextAdaptor.SetAlpha(0f);
		}
		void StartShowTextProcess(){
			thisHideProcessSuite.Stop();
			thisShowProcessSuite.Start();
		}
		void StartHideTextProcess(){
			thisShowProcessSuite.Stop();
			thisHideProcessSuite.Start();
		}
		public void OnProcessRun(IProcessSuite suite){

		}
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisShowProcessSuite){
				AnimationCurve alphaCurve = thisPopTextAdaptor.GetShowAlphaCurve();
				float newAlpha = alphaCurve.Evaluate(normalizedTime);
				thisPopTextAdaptor.SetAlpha(newAlpha);

				AnimationCurve scaleCurve = thisPopTextAdaptor.GetShowScaleCurve();
				float newScale = scaleCurve.Evaluate(normalizedTime);
				thisPopTextAdaptor.SetScale(newScale);
			}else if(suite == thisHideProcessSuite){
				AnimationCurve alphaCurve = thisPopTextAdaptor.GetHideAlphaCurve();
				float newAlpha = alphaCurve.Evaluate(normalizedTime);
				thisPopTextAdaptor.SetAlpha(newAlpha);

				AnimationCurve scaleCurve = thisPopTextAdaptor.GetHideScaleCurve();
				float newScale = scaleCurve.Evaluate(normalizedTime);
				thisPopTextAdaptor.SetScale(newScale);
			}
		}
		public void OnProcessExpire(IProcessSuite suite){
			if(suite == thisShowProcessSuite)
				ShowText();
			else if(suite == thisHideProcessSuite)
				HideText();
		}
		void StopAllProcess(){
			thisShowProcessSuite.Stop();
			thisHideProcessSuite.Stop();
		}

	}
}

