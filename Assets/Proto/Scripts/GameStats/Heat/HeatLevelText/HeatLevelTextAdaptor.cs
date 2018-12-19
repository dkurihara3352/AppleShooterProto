using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AppleShooterProto{
	public interface IHeatLevelTextAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IHeatLevelText GetHeatLevelText();
		void SetText(string text);
		AnimationCurve GetScaleValueCurve();
		float GetLevelUpProcessTime();
		void SetScale(float scaleValue);
		float GetTextSwapPoint();
	}
	public class HeatLevelTextAdaptor: AppleShooterMonoBehaviourAdaptor, IHeatLevelTextAdaptor{
		public override void SetUp(){
			thisHeatLevelText = CreateHeatLevelText();
		}
		IHeatLevelText thisHeatLevelText;
		public IHeatLevelText GetHeatLevelText(){
			return thisHeatLevelText;
		}
		IHeatLevelText CreateHeatLevelText(){
			HeatLevelText.IConstArg arg = new HeatLevelText.ConstArg(
				this
			);
			return new HeatLevelText(arg);
		}
		public Text thisHeatLevelTextComp;
		public void SetText(string text){
			thisHeatLevelTextComp.text = text;
		}
		public AnimationCurve scaleValueCurve;
		public AnimationCurve GetScaleValueCurve(){
			return scaleValueCurve;
		}
		public float levelUpProcessTime = 1f;
		public float GetLevelUpProcessTime(){
			return levelUpProcessTime;
		}
		public Transform thisScaleTransform;
		public void SetScale(float scaleValue){
			Vector3 newScale = Vector3.one * scaleValue;
			thisScaleTransform.localScale = newScale;
		}
		public float textSwapPoint;
		public float GetTextSwapPoint(){
			return textSwapPoint;
		}
	} 
}
