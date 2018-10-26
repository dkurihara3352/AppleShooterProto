using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
using UnityEngine.UI;

namespace AppleShooterProto{
	public interface IPopUIGlideProcess: IProcess{}
	public class PopUIGlideProcess: AbsConstrainedProcess, IPopUIGlideProcess{
		public PopUIGlideProcess(
			IConstArg arg
		): base(
			arg
		){
			thisAdaptor = arg.adaptor;
			thisPopMode = arg.popMode;
			thisGlideDistance = arg.glideDistance;
			thisNormalizedDistanceCurve = arg.normalizedDistanceCurve;
			thisAlphaCurve = arg.alphaCurve;
			thisScaleCurve = arg.scaleCurve;
			
			thisChildGraphic = thisAdaptor.GetChildGraphic();

			thisInitialPosition = thisAdaptor.GetChildGraphicOriginalLocalPosition();
			thisChildGraphicOriginalColor = thisAdaptor.GetChildGraphicOriginalColor();
			thisChildGraphicOriginalAlpha = thisChildGraphicOriginalColor.a;
		}
		readonly  IPopUIAdaptor thisAdaptor;
		readonly PopUIAdaptor.PopMode thisPopMode;
		readonly float thisGlideDistance;
		readonly AnimationCurve thisNormalizedDistanceCurve;
		readonly AnimationCurve thisAlphaCurve;
		readonly AnimationCurve thisScaleCurve;
		readonly Vector2 thisInitialPosition;
		readonly Graphic thisChildGraphic;
		readonly Color thisChildGraphicOriginalColor;
		readonly float thisChildGraphicOriginalAlpha;
		Vector2 thisGlideDirection;
		protected override void RunImple(){
			if(thisPopMode == PopUIAdaptor.PopMode.GlideUp)
				thisGlideDirection = Vector2.up;
			else if( thisPopMode == PopUIAdaptor.PopMode.GlideRandom)
				thisGlideDirection = CalcRandomDirection();
			
		}
		protected override void UpdateProcessImple(float deltaT){
			UpdateUIPosition();
			UpdateUIAlpha();
			UpdateUIScale();
		}
		void UpdateUIPosition(){
			if(thisPopMode == PopUIAdaptor.PopMode.PopStatic)
				return;
						
			float normalizedDistance = thisNormalizedDistanceCurve.Evaluate(thisNormalizedTime);
			Vector2 displacement = normalizedDistance * thisGlideDirection * thisGlideDistance;

			Vector2 newPosition = thisInitialPosition + displacement;
			thisChildGraphic.transform.localPosition = newPosition;
		}
		Vector2 CalcRandomDirection(){
			float normalizedAngle = Random.Range(0f, 1f);
			float angle = 360f * normalizedAngle;
			float angleInRad = angle * Mathf.Deg2Rad;
			float cosine = Mathf.Cos(angleInRad);
			float sine = Mathf.Sin(angleInRad);
			return new Vector2(cosine, sine);
		}
		void UpdateUIAlpha(){
			float newAlpha =  thisAlphaCurve.Evaluate(thisNormalizedTime);
			// thisAdaptor.SetUIAlpha(newAlpha);
			SetAlphaOnChildUIGraphic(newAlpha);
		}
		void SetAlphaOnChildUIGraphic(float alpha){
			float newAlpha = Mathf.Lerp(
				0f,
				thisChildGraphicOriginalAlpha,
				alpha
			);
			Color newColor = new Color(
				thisChildGraphicOriginalColor.r,
				thisChildGraphicOriginalColor.g,
				thisChildGraphicOriginalColor.b,
				newAlpha
			);
			thisChildGraphic.color = newColor;
		}
		void UpdateUIScale(){
			float newScale = thisScaleCurve.Evaluate(thisNormalizedTime);
			SetScaleOnChildUIGraphic(newScale);
		}
		void SetScaleOnChildUIGraphic(float newScale){
			Vector3 newScaleV3 = Vector3.one * newScale;
			thisChildGraphic.transform.localScale = newScaleV3;
		}
		protected override void StopImple(){
			thisAdaptor.DeactivateUI();
			// thisAdaptor.TriggerDeactivateOnAnimator();
		}
		/* Const */
			public interface IConstArg: IConstrainedProcessConstArg{
				IPopUIAdaptor adaptor{get;}
				PopUIAdaptor.PopMode popMode{get;}
				float glideDistance{get;}
				AnimationCurve normalizedDistanceCurve{get;}
				AnimationCurve alphaCurve{get;}
				AnimationCurve scaleCurve{get;}
			}
			public class ConstArg: ConstrainedProcessConstArg, IConstArg{
				public ConstArg(
					IPopUIAdaptor adaptor,
					PopUIAdaptor.PopMode popMode,
					float glideDistance,
					AnimationCurve normalizedDistanceCurve,
					AnimationCurve alhpaCurve,
					AnimationCurve scaleCurve,

					IProcessManager processManager,
					float glideTime

				): base(
					processManager,
					ProcessConstraint.ExpireTime,
					glideTime
				){
					thisAdaptor = adaptor;
					thisPopMode = popMode;
					thisGlideDistance = glideDistance;
					thisNormalizedDistanceCurve = normalizedDistanceCurve;
					thisAlphaCurve = alhpaCurve;
					thisScaleCurve = scaleCurve;
				}
				readonly IPopUIAdaptor thisAdaptor;
				public IPopUIAdaptor adaptor{
					get{
						return thisAdaptor;
					}
				}
				readonly PopUIAdaptor.PopMode thisPopMode;
				public PopUIAdaptor.PopMode popMode{
					get{
						return thisPopMode;
					}
				}
				readonly float thisGlideDistance;
				public float glideDistance{
					get{
						return thisGlideDistance;
					}
				}
				readonly AnimationCurve thisNormalizedDistanceCurve;
				public AnimationCurve normalizedDistanceCurve{
					get{
						return thisNormalizedDistanceCurve;
					}
				}
				readonly AnimationCurve thisAlphaCurve;
				public AnimationCurve alphaCurve{
					get{
						return thisAlphaCurve;
					}
				}
				readonly AnimationCurve thisScaleCurve;
				public AnimationCurve scaleCurve{get{return thisScaleCurve;}}
			}
		/*  */
	}
}
