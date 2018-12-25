using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IAlphaVisibilityTogglableUIElement: IUIElement{
		void UpdateShowness(float normalizedTime);
	}
	public class AlphaVisibilityTogglableUIElement: UIElement, IAlphaVisibilityTogglableUIElement{
		public AlphaVisibilityTogglableUIElement(IConstArg arg): base(arg){}
		IAlphaVisibilityTogglableUIAdaptor thisAlphaVisibilityTogglableUIAdaptor{
			get{
				return(IAlphaVisibilityTogglableUIAdaptor)thisUIAdaptor;
			}
		}
		public void UpdateShowness(float normalizedTime){
			AnimationCurve alphaCurve = thisAlphaVisibilityTogglableUIAdaptor.GetAlphaCurve();
			float newAlpha = alphaCurve.Evaluate(normalizedTime);
			thisAlphaVisibilityTogglableUIAdaptor.SetAlpha(newAlpha);
		}
	}
}


