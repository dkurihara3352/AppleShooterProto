using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IAlphaVisibilityTogglableUIAdaptor: IUIAdaptor{
		IAlphaVisibilityTogglableUIElement GetAlphaVisibilityTogglableUIElement();
		AnimationCurve GetProcessCurve();
		void SetAlpha(float alpha);
		float GetAlpha();
		float GetProcessTime();
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
		public AnimationCurve processCurve;
		public AnimationCurve GetProcessCurve(){
			return processCurve;
		}
		public void SetAlpha(float alpha){
			thisCanvasGroup.alpha = alpha;
		}
		public float GetAlpha(){
			return thisCanvasGroup.alpha;
		}
		public float processTime = .5f;
		public float GetProcessTime(){
			return processTime;
		}
	}
}


