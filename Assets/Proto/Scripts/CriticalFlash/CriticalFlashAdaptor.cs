using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface ICriticalFlashAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		ICriticalFlash GetCriticalFlash();
		AnimationCurve GetFlashCurve();
		float GetFlashTime();
		void SetFlashValue(float value);
	}
	public class CriticalFlashAdaptor : SlickBowShootingMonoBehaviourAdaptor, ICriticalFlashAdaptor {
		ICriticalFlash thisFlash;
		public override void SetUp(){
			thisFlash = CreateFlash();
			thisOriginalFlashImageColor = flashImage.color;
		}
		public ICriticalFlash GetCriticalFlash(){
			return thisFlash;
		}
		ICriticalFlash CreateFlash(){
			CriticalFlash.IConstArg arg = new CriticalFlash.ConstArg(
				this
			);
			return new CriticalFlash(arg);
		}

		public AnimationCurve flashCurve;
		public AnimationCurve GetFlashCurve(){
			return flashCurve;
		}
		public float flashTime = .5f;
		public float GetFlashTime(){
			return flashTime;
		}
		public UnityEngine.UI.Image flashImage;
		public void SetFlashValue(float normalizedFlashValue){
			Color newColor = GetNewColor(normalizedFlashValue);
			flashImage.color = newColor;
		}
		Color thisOriginalFlashImageColor;
		Color GetNewColor(float normalizedFlashValue){
			float newA = Mathf.Lerp(
				0f,
				1f,
				normalizedFlashValue
			);
			Color result = thisOriginalFlashImageColor;
			result.a = newA;
			return result;
		}
	}
}
