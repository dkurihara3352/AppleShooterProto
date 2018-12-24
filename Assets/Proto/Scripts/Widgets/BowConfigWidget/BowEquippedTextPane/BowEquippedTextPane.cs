using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using DKUtility;

namespace AppleShooterProto{
	public interface IBowEquippedTextPane: IUIElement, IProcessHandler{
		void ShowEquippedText();
		void StartShowTextProcess();
		void HideEquippedText();
		void StartHideTextProcess();
	}
	public class BowEquippedTextPane: UIElement, IBowEquippedTextPane{

		public BowEquippedTextPane(IConstArg arg): base(arg){
			thisShowProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisBowEquippedTextPaneAdaptor.GetProcessTime()
			);
			thisHideProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisBowEquippedTextPaneAdaptor.GetProcessTime()
			);
		}
		IProcessSuite thisShowProcessSuite;
		IProcessSuite thisHideProcessSuite;
		IBowEquippedTextPaneAdaptor thisBowEquippedTextPaneAdaptor{
			get{
				return (IBowEquippedTextPaneAdaptor)thisUIAdaptor;
			}
		}
		public void ShowEquippedText(){
			StopAllProcess();
			thisBowEquippedTextPaneAdaptor.SetAlpha(1f);
			thisBowEquippedTextPaneAdaptor.SetScale(1f);
		}
		public void HideEquippedText(){
			StopAllProcess();
			thisBowEquippedTextPaneAdaptor.SetAlpha(0f);
		}
		public void StartShowTextProcess(){
			thisHideProcessSuite.Stop();
			thisShowProcessSuite.Start();
		}
		public void StartHideTextProcess(){
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
				AnimationCurve alphaCurve = thisBowEquippedTextPaneAdaptor.GetShowAlphaCurve();
				float newAlpha = alphaCurve.Evaluate(normalizedTime);
				thisBowEquippedTextPaneAdaptor.SetAlpha(newAlpha);

				AnimationCurve scaleCurve = thisBowEquippedTextPaneAdaptor.GetShowScaleCurve();
				float newScale = scaleCurve.Evaluate(normalizedTime);
				thisBowEquippedTextPaneAdaptor.SetScale(newScale);
			}else if(suite == thisHideProcessSuite){
				AnimationCurve alphaCurve = thisBowEquippedTextPaneAdaptor.GetHideAlphaCurve();
				float newAlpha = alphaCurve.Evaluate(normalizedTime);
				thisBowEquippedTextPaneAdaptor.SetAlpha(newAlpha);

				AnimationCurve scaleCurve = thisBowEquippedTextPaneAdaptor.GetHideScaleCurve();
				float newScale = scaleCurve.Evaluate(normalizedTime);
				thisBowEquippedTextPaneAdaptor.SetScale(newScale);
			}
		}
		public void OnProcessExpire(IProcessSuite suite){
			if(suite == thisShowProcessSuite)
				ShowEquippedText();
			else if(suite == thisHideProcessSuite)
				HideEquippedText();
		}
		void StopAllProcess(){
			thisShowProcessSuite.Stop();
			thisHideProcessSuite.Stop();
		}

	}
}

