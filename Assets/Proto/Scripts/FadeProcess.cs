using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IFadeProcess: IProcess{

	}
	public class FadeProcess : AbsInterpolatorProcess<FadeProcess.IFadableInterpolator>, IFadeProcess{

		public FadeProcess(
			IConstArg arg
		): base(arg){
			thisFadable = arg.fadable;
			thisFadesIn = arg.fadesIn;
		}
		readonly IFadable thisFadable;
		readonly bool thisFadesIn;
		protected override IFadableInterpolator CreateInterpolator(){
			float initialAlpha = thisFadable.GetAlpha();
			float targetAlpha;
			if(thisFadesIn)
				targetAlpha = 1f;
			else
				targetAlpha = 0f;
			FadableInterpolator.IConstArg arg = new FadableInterpolator.ConstArg(
				initialAlpha,
				targetAlpha,
				thisFadable
			);
			return new FadableInterpolator(arg);
		}
		/* Const */
			public interface IConstArg: IInterpolatorProcesssConstArg{
				IFadable fadable{get;}
				bool fadesIn{get;}
				float fadeTime{get;}
			}
			public class ConstArg: InterpolatorProcessConstArg, IConstArg{
				public ConstArg(
					IFadable fadable,
					bool fadesIn,
					float fadeTime
					,
					IProcessManager processManager
				): base(
					processManager,
					ProcessConstraint.ExpireTime,
					fadeTime,
					false
				){
					thisFadable = fadable;
					thisFadesIn = fadesIn;
					thisFadeTime = fadeTime;
				}
				readonly IFadable thisFadable;
				public IFadable fadable{get{return thisFadable;}}
				readonly bool thisFadesIn;
				public bool fadesIn{get{return thisFadesIn;}}
				readonly float thisFadeTime;
				public float fadeTime{get{return thisFadeTime;}}
			}
		/* interpolator */
			public interface IFadableInterpolator: IInterpolator{
			}
			public class FadableInterpolator: AbsInterpolator,IFadableInterpolator{
				public FadableInterpolator(
					IConstArg arg
				){
					thisInitialAlpha = arg.initialAlpha;
					thisTargetAlpha = arg.targetAlpha;
					thisFadable = arg.fadable;
				}
				readonly float thisInitialAlpha;
				readonly float thisTargetAlpha;
				readonly IFadable thisFadable;
				protected override void InterpolateImple(float normalizedT){
					float newAlpha = Mathf.Lerp(
						thisInitialAlpha,
						thisTargetAlpha,
						normalizedT
					);
					thisFadable.SetAlpha(newAlpha);
				}
				public override void Terminate(){
					thisFadable.SetAlpha(thisTargetAlpha);
				}
				public interface IConstArg{
					float initialAlpha{get;}
					float targetAlpha{get;}
					IFadable fadable{get;}
				}
				public struct ConstArg: IConstArg{
					public ConstArg(
						float initialAlpha,
						float targetAlpha,
						IFadable fadable
					){
						thisInitialAlpha = initialAlpha;
						thisTargetAlpha = targetAlpha;
						thisFadable = fadable;
					}
					readonly float thisInitialAlpha;
					public float initialAlpha{get{return thisInitialAlpha;}}
					readonly float thisTargetAlpha;
					public float targetAlpha{get{return thisTargetAlpha;}}
					readonly IFadable thisFadable;
					public IFadable fadable{get{return thisFadable;}}
				}
			}
		/*  */
	}
}
