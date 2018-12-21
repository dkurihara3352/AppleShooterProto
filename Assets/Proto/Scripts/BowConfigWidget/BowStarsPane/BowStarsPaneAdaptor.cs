using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using UnityEngine.UI;

namespace AppleShooterProto{
	public interface IBowStarsPaneAdaptor: IUIAdaptor{
		IBowStarsPane GetBowStarsPane();
		float GetProcessTime();
		AnimationCurve GetFillCurve();
		void SetFill(float fill);
		float GetFill();
		int GetStepCount();
	}
	public class BowStarsPaneAdaptor: UIAdaptor, IBowStarsPaneAdaptor{
		protected override IUIElement CreateUIElement(){
			BowStarsPane.IConstArg arg = new BowStarsPane.ConstArg(
				this,
				activationMode
			);
			return new BowStarsPane(arg);
		}
		IBowStarsPane thisBowLevelPane{
			get{
				return (IBowStarsPane)thisUIElement;
			}
		}
		public IBowStarsPane GetBowStarsPane(){
			return thisBowLevelPane;
		}
		public float processTime = 1f;
		public float GetProcessTime(){
			return processTime;
		}
		public AnimationCurve fillCurve;
		public AnimationCurve GetFillCurve(){
			return fillCurve;
		}
		public Image[] thisStarImages;
		public void SetFill(float fill){
			ClearAllFill();
			int quotient = Mathf.FloorToInt(fill);
			for(int i = 0; i < quotient; i ++){
				Image starImage = thisStarImages[i];
				starImage.fillAmount = 1f;
			}
			float modulo = fill - quotient;
			if(quotient < thisStarImages.Length){
				Image nextStarImage = thisStarImages[quotient];
				nextStarImage.fillAmount = modulo;
			}
		}
		void ClearAllFill(){
			foreach(Image starImage in thisStarImages)
				starImage.fillAmount = 0f;
		}

		public float GetFill(){
			float result = 0f;
			foreach(Image starImage in thisStarImages){
				result += starImage.fillAmount;
			}
			return result;
		}
		public int stepCount = 5;
		public int GetStepCount(){
			return stepCount;
		}
	}
}


