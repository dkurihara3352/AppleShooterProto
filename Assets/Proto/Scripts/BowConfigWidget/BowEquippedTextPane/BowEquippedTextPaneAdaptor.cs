using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IBowEquippedTextPaneAdaptor: IUIAdaptor{
		IBowEquippedTextPane GetBowEquippedTextPane();
		void SetScale(float scaleValue);
		void SetAlpha(float alhpa);

		float GetProcessTime();
		AnimationCurve GetShowScaleCurve();
		AnimationCurve GetShowAlphaCurve();
		AnimationCurve GetHideScaleCurve();
		AnimationCurve GetHideAlphaCurve();
	}
	[RequireComponent(typeof(CanvasGroup))]
	public class BowEquippedTextPaneAdaptor: UIAdaptor, IBowEquippedTextPaneAdaptor{

		public override void SetUp(){
			base.SetUp();
			thisCanvasGroup = CollectCanvasGroup();
		}
		protected override IUIElement CreateUIElement(){
			BowEquippedTextPane.IConstArg arg = new BowEquippedTextPane.ConstArg(
				this,
				activationMode
			);
			return new BowEquippedTextPane(arg);
		}
		IBowEquippedTextPane thisBowEquippedTextPane{
			get{
				return (IBowEquippedTextPane)thisUIElement;
			}
		}
		public IBowEquippedTextPane GetBowEquippedTextPane(){
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

