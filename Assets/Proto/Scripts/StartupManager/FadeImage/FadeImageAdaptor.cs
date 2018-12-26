using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AppleShooterProto{
	public interface IFadeImageAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IFadeImage GetFadeImage();
		void SetAlpha(float alpha);
	}
	public class FadeImageAdaptor: AppleShooterMonoBehaviourAdaptor, IFadeImageAdaptor{
		public override void SetUp(){
			FadeImage.IConstArg arg = new FadeImage.ConstArg(
				this
			);
			thisFadeImage = new FadeImage(arg);
		}
		IFadeImage thisFadeImage;
		public IFadeImage GetFadeImage(){
			return thisFadeImage;
		}
		public CanvasGroup canvasGroupToTurn;

		public void SetAlpha(float alpha){
			canvasGroupToTurn.alpha = alpha;
		}
		// public override void FinalizeSetUp(){

		// }
	}
}

