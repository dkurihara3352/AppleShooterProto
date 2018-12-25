using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IAlphaVisibilityTogglableUIAdaptor: IUIAdaptor{
		IAlphaVisibilityTogglableUIElement GetAlphaVisibilityTogglableUIElement();
		AnimationCurve GetAlphaCurve();
		void SetAlpha(float alpha);
	}
	[RequireComponent(typeof(CanvasGroup))]
	public class AlphaVisibilityTogglableUIAdaptor: UIAdaptor, IAlphaVisibilityTogglableUIAdaptor{
		public override void SetUp(){
			base.SetUp();
			thisCanvasGroup = GetComponent<CanvasGroup>();
		}
		protected override IUIElement CreateUIElement(){
			AlphaVisibilityTogglableUIElement.IConstArg arg = new AlphaVisibilityTogglableUIElement.ConstArg(
				this,
				activationMode
			);
			return new AlphaVisibilityTogglableUIElement(arg);
		}
		IAlphaVisibilityTogglableUIElement thisTogglableUIElement{
			get{
				return (IAlphaVisibilityTogglableUIElement)thisUIElement;
			}
		}
		public IAlphaVisibilityTogglableUIElement GetAlphaVisibilityTogglableUIElement(){
			return thisTogglableUIElement;
		}
		public AnimationCurve alphaCurve;
		public AnimationCurve GetAlphaCurve(){
			return alphaCurve;
		}
		public void SetAlpha(float alpha){
			thisCanvasGroup.alpha = alpha;
		}
	}
}


