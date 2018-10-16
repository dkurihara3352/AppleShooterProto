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
			thisTypedAdaptor = thisAdaptor as ITestShootingTargetAdaptor;
			thisDefaultColor = arg.defaultColor;
			thisProcessFactory = arg.processFactory;
			thisFadeTime = arg.fadeTime;
			TestShootingTargetHitStateEngine.IConstArg engineConstArg = new TestShootingTargetHitStateEngine.ConstArg(this);
			thisHitStateEngine = new TestShootingTargetHitStateEngine(engineConstArg);
		}
		readonly ITestShootingTargetAdaptor thisTypedAdaptor;
		readonly Color thisDefaultColor;
		readonly ITestShootingTargetHitStateEngine thisHitStateEngine;
		readonly protected IAppleShooterProcessFactory thisProcessFactory;
		readonly float thisFadeTime;
		protected override void DestroyTarget(){
			StartFadingAway();
			thisTypedAdaptor.ToggleCollider(false);
			ReserveAllLandedArrow();
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
			float normalizedHealth = health/ thisOriginalHealth;
			Color newColor = Color.Lerp(
				Color.red,
				thisDefaultColor,
				normalizedHealth
			);
			thisTypedAdaptor.SetColor(newColor);
		}
		protected override void IndicateHit(float delta){
			thisHitStateEngine.Hit(delta);
		}
		public float GetAlpha(){
			return thisTypedAdaptor.GetAlpha();
		}
		public void SetAlpha(float alpha){
			thisTypedAdaptor.SetAlpha(alpha);
		}
		public void CheckAndStartNewHitAnimation(float magnitude){
			thisTypedAdaptor.CheckAndStartNewHitAnimation(magnitude);
		}
		public override void ResetTarget(){
			base.ResetTarget();
			thisTypedAdaptor.SetColor(thisDefaultColor);
			SetAlpha(1f);
			thisTypedAdaptor.ToggleCollider(true);
		}
		/* Const */
			public interface IConstArg: IAbsConstArg{
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
				public IShootingTargetAdaptor adaptor{get{return thisAdaptor;}}
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
