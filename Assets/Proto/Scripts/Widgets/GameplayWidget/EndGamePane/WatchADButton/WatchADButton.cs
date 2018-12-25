using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IWatchADButton: IValidatableUIElement{
		void UpdateShowness(float normalizedTime);
		void ResetWatchADButton();
		void EnableInput();
	}
	public class WatchADButton: ValidatableUIElement, IWatchADButton{
		public WatchADButton(IConstArg arg): base(arg){}
		IWatchADButtonAdaptor thisWatchADButtonAdaptor{
			get{
				return (IWatchADButtonAdaptor)thisUIAdaptor;
			}
		}
		public void UpdateShowness(float normalizedTime){
			AnimationCurve alphaCurve = thisWatchADButtonAdaptor.GetAlphaCurve();
			float newAlpha = alphaCurve.Evaluate(normalizedTime);
			thisWatchADButtonAdaptor.SetAlpha(newAlpha);
		}
		
		public void ResetWatchADButton(){
			UpdateShowness(0f);
			DisableInputRecursively();
		}
		public void EnableInput(){
			EnableInputRecursively();
		}
		protected override void OnTapImple(int tapCount){
			base.OnTapImple(tapCount);
			Debug.Log("making money!");
		}

	}
}


