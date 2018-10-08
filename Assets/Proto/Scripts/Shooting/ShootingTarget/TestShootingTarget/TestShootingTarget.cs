using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IFadable{
		void SetAlpha(float alpha);
		float GetAlpha();
	}
	public interface ITestShootingTarget: IShootingTarget, IFadable{
		void CheckAndStartNewHitAnimation(float magnitude);
	}
	public class TestShootingTarget : AbsShootingTarget, ITestShootingTarget {
		public TestShootingTarget(
			IConstArg arg
		):base(arg){
			thisAdaptor  = arg.adaptor;
			thisMaxHealth = thisHealth;
			thisDefaultColor = arg.defaultColor;
			thisProcessFactory = arg.processFactory;
			thisFadeTime = arg.fadeTime;
			TestShootingTargetHitStateEngine.IConstArg engineConstArg = new TestShootingTargetHitStateEngine.ConstArg(this);
			thisHitStateEngine = new TestShootingTargetHitStateEngine(engineConstArg);
		}
		readonly ITestShootingTargetAdaptor thisAdaptor;
		readonly float thisMaxHealth;
		readonly Color thisDefaultColor;
		readonly ITestShootingTargetHitStateEngine thisHitStateEngine;
		readonly IAppleShooterProcessFactory thisProcessFactory;
		readonly float thisFadeTime;
		protected override void DestroyTarget(){
			StartFadingAway();
			thisAdaptor.ToggleOffCollider();
		}
		void StartFadingAway(){
			thisFadeProcess = thisProcessFactory.CreateFadeProcess(
				fadable: this,
				fadeIn: false,
				fadeTime: thisFadeTime
			);
			thisFadeProcess.Run();
		}
		IFadeProcess thisFadeProcess;
		protected override void IndicateHealth(
			float health, 
			float delta
		){
			float normalizedHealth = health/ thisMaxHealth;
			Color newColor = Color.Lerp(
				Color.red,
				thisDefaultColor,
				normalizedHealth
			);
			thisAdaptor.SetColor(newColor);
		}
		protected override void IndicateHit(float delta){
			thisHitStateEngine.Hit(delta);
		}
		public void StartHitAnimation(float magnitude){
			thisAdaptor.StartHitAnimation(magnitude);
		}
		public void StopHitAnimation(){
			thisAdaptor.StopHitAnimation();
		}
		public float GetAlpha(){
			return thisAdaptor.GetAlpha();
		}
		public void SetAlpha(float alpha){
			thisAdaptor.SetAlpha(alpha);
		}
		public void CheckAndStartNewHitAnimation(float magnitude){
			thisAdaptor.CheckAndStartNewHitAnimation(magnitude);
		}
		/* Const */
			public interface IConstArg: IShootingTargetConstArg{
				ITestShootingTargetAdaptor adaptor{get;}
				Color defaultColor{get;}
				IAppleShooterProcessFactory processFactory{get;}
				float fadeTime{get;}
			}
			public struct ConstArg: IConstArg{
				public ConstArg(
					float health,
					ITestShootingTargetAdaptor adaptor,
					Color defaultColor,
					IAppleShooterProcessFactory processFactory,
					float fadeTime

				){
					thisHealth = health;
					thisAdaptor = adaptor;
					thisDefaultColor = defaultColor;
					thisProcessFactory =  processFactory;
					thisFadeTime = fadeTime;
				}
				readonly float thisHealth;
				public float health{get{return thisHealth;}}
				readonly ITestShootingTargetAdaptor thisAdaptor;
				public ITestShootingTargetAdaptor adaptor{get{return thisAdaptor;}}
				readonly Color thisDefaultColor;
				public Color defaultColor{get{return thisDefaultColor;}}
				readonly IAppleShooterProcessFactory thisProcessFactory;
				public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
				readonly float thisFadeTime;
				public float fadeTime{get{return thisFadeTime;}}
			}
		/*  */
	}

}
