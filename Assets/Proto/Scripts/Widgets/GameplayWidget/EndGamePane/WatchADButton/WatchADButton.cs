using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IWatchADButton: IValidatableUIElement{
		void SetEndGamePane(IEndGamePane pane);
		void UpdateShowness(float normalizedTime);
		void ResetWatchADButton();
		void EnableInput();
		void InvalidateForADWatchDone();
	}
	public class WatchADButton: ValidatableUIElement, IWatchADButton{
		public WatchADButton(IConstArg arg): base(arg){}
		IWatchADButtonAdaptor thisWatchADButtonAdaptor{
			get{
				return (IWatchADButtonAdaptor)thisUIAdaptor;
			}
		}
		public void UpdateShowness(float normalizedTime){
			AnimationCurve alphaCurve = thisWatchADButtonAdaptor.GetProcessCurve();
			float newAlpha = alphaCurve.Evaluate(normalizedTime);
			thisWatchADButtonAdaptor.SetAlpha(newAlpha);
		}
		
		public void ResetWatchADButton(){
			UpdateShowness(0f);
			DisableInputRecursively();
			Validate();
			thisWatchADButtonAdaptor.SetLabelText("Watch AD");
		}
		public void EnableInput(){
			EnableInputRecursively();
		}
		public override void BecomeSelectableImple(){
			base.BecomeSelectableImple();
			// Debug.Log(DKUtility.DebugHelper.StringInColor("become selectable, isValid: " + thisIsValid.ToString(), Color.blue));
		}
		public override void BecomeUnselectableImple(){
			base.BecomeUnselectableImple();
			// Debug.Log(DKUtility.DebugHelper.StringInColor("become unselectable", Color.red));
		}
		protected override void OnTapImple(int tapCount){
			base.OnTapImple(tapCount);
			// Debug.Log("making money!");
			//below is temp
				thisEndGamePane.OnWatchADComplete();
		}
		IEndGamePane thisEndGamePane;
		public void SetEndGamePane(IEndGamePane pane){
			thisEndGamePane = pane;
		}
		public void InvalidateForADWatchDone(){
			Invalidate();
			thisWatchADButtonAdaptor.SetLabelText("watched");
		}
	}
}


