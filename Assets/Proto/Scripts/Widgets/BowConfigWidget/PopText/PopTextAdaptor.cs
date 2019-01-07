using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IPopTextAdaptor: IUIAdaptor{
		IPopText GetPopText();
		void SetScale(float scaleValue);
		void SetAlpha(float alhpa);
		void SetText(string text);

		float GetProcessTime();
		AnimationCurve GetShowScaleCurve();
		AnimationCurve GetShowAlphaCurve();
		AnimationCurve GetHideScaleCurve();
		AnimationCurve GetHideAlphaCurve();
	}
	[RequireComponent(typeof(CanvasGroup))]
	public class PopTextAdaptor: UIAdaptor, IPopTextAdaptor{

		public override void SetUp(){
			base.SetUp();
			thisCanvasGroup = CollectCanvasGroup();
		}
		protected override IUIElement CreateUIElement(){
			PopText.IConstArg arg = new PopText.ConstArg(
				this,
				activationMode
			);
			return new PopText(arg);
		}
		IPopText thisBowEquippedTextPane{
			get{
				return (IPopText)thisUIElement;
			}
		}
		public IPopText GetPopText(){
			return thisBowEquippedTextPane;
		}

		public void SetScale(float scaleValue){
			Vector3 newScale = Vector3.one * scaleValue;
			this.transform.localScale = newScale;
		}

		CanvasGroup CollectCanvasGroup(){
			return GetComponent<CanvasGroup>();
		}
		public void SetAlpha(float alpha){
			thisCanvasGroup.alpha = alpha;
		}
		public float processTime = .5f;
		public float GetProcessTime(){
			return processTime;
		}

		public UnityEngine.UI.Text textComp;
		public void SetText(string text){
			textComp.text = text;
		}
		public AnimationCurve showScaleCurve;
		public AnimationCurve GetShowScaleCurve(){
			return showScaleCurve;
		}
		public AnimationCurve showAlphaCurve;
		public AnimationCurve GetShowAlphaCurve(){
			return showAlphaCurve;
		}
		public AnimationCurve hideScaleCurve;
		public AnimationCurve GetHideScaleCurve(){
			return hideScaleCurve;
		}
		public AnimationCurve hideAlphaCurve;
		public AnimationCurve GetHideAlphaCurve(){
			return hideAlphaCurve;
		}
	}
}

