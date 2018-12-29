using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeTai.Asset.TranslucentImage;

namespace AppleShooterProto{
	public interface IFrostManagerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IFrostManager GetFrostManager();
		void SetFrostValue(float value);
		float GetFrostValue();

		float GetProcessTime();
		AnimationCurve GetProcessCurve();
	}
	public class FrostManagerAdaptor: AppleShooterMonoBehaviourAdaptor, IFrostManagerAdaptor{
		public override void SetUp(){
			FrostManager.IConstArg arg = new FrostManager.ConstArg(
				this
			);
			thisFrostManager = new FrostManager(arg);
			SetFrostValue(1f);
		}
		IFrostManager thisFrostManager;
		public IFrostManager GetFrostManager(){
			return thisFrostManager;
		}
		// public TranslucentImageSource translucentImageSource;
		public TranslucentImage translucentImage;
		// float targetBlurStrength = 200f;
		float targetImageAlpha = 1f;
		public float GetFrostValue(){
			return translucentImage.color.a;
		}
		public void SetFrostValue(float value){
			float newAlpha = Mathf.Lerp(
				0f,
				targetImageAlpha,
				value
			);
			Color newColor = translucentImage.color;
			newColor.a = newAlpha;
			translucentImage.color = newColor;
		}
		public float processTime = 1f;
		public float GetProcessTime(){
			return processTime;
		}
		public AnimationCurve processCurve;
		public AnimationCurve GetProcessCurve(){
			return processCurve;
		}

	}
}



