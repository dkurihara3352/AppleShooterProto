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
			thisPopUI = arg.popUI;
			thisPopMode = arg.popMode;
			thisMinGlideDistance = arg.minGlideDistance;
			thisMaxGlideDistance = arg.maxGlideDistance;
			thisNormalizedDistanceCurve = arg.normalizedDistanceCurve;
			thisAlphaCurve = arg.alphaCurve;
			thisScaleCurve = arg.scaleCurve;

			thisInitialPosition = arg.graphicOriginalLocalPosition;
			thisChildGraphicOriginalColor = arg.graphicOriginalColor;
			thisChildGraphicOriginalAlpha = thisChildGraphicOriginalColor.a;
		}
		readonly IPopUI thisPopUI;
		readonly PopUIAdaptor.PopMode thisPopMode;
		readonly float thisMaxGlideDistance;
		readonly float thisMinGlideDistance;
		readonly AnimationCurve thisNormalizedDistanceCurve;
		readonly AnimationCurve thisAlphaCurve;
		readonly AnimationCurve thisScaleCurve;
		readonly Vector2 thisInitialPosition;
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
			float normalizedSqrDistanceOfUIToCam = thisPopUI.GetNormalizedSqrDist();
			float glideDistance = Mathf.Lerp(
				thisMaxGlideDistance,
				thisMinGlideDistance,
				normalizedSqrDistanceOfUIToCam
			);
			Vector2 displacement = normalizedDistance * thisGlideDirection * glideDistance;

			Vector2 newPosition = thisInitialPosition + displacement;

			thisPopUI.SetChildGraphicLocalPosition(newPosition);
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
			thisPopUI.SetChildGraphicColor(newColor);
		}
		void UpdateUIScale(){
			float newScale = thisScaleCurve.Evaluate(thisNormalizedTime);
			SetScaleOnChildUIGraphic(newScale);
		}
		void SetScaleOnChildUIGraphic(float newScale){
			Vector3 newScaleV3 = Vector3.one * newScale;
			thisPopUI.SetChildGraphicLocalScale(newScaleV3);
		}
		protected override void StopImple(){
			thisPopUI.Deactivate();
		}
		/* Const */
			public interface IConstArg: IConstrainedProcessConstArg{
				IPopUI popUI{get;}
				PopUIAdaptor.PopMode popMode{get;}
				// float glideDistance{get;}
				float minGlideDistance{get;}
				float maxGlideDistance{get;}
				AnimationCurve normalizedDistanceCurve{get;}
				AnimationCurve alphaCurve{get;}
				AnimationCurve scaleCurve{get;}
				Vector2 graphicOriginalLocalPosition{get;}
				Color graphicOriginalColor{get;}
			}
			public class ConstArg: ConstrainedProcessConstArg, IConstArg{
				public ConstArg(
					IPopUI popUI,
					PopUIAdaptor.PopMode popMode,
					// float glideDistance,
					float minGlideDistance,
					float maxGlideDistance,
					AnimationCurve normalizedDistanceCurve,
					AnimationCurve alhpaCurve,
					AnimationCurve scaleCurve,
					Vector2 graphicOriginalLocalPosition,
					Color graphicOriginalColor,

					IProcessManager processManager,
					float glideTime

				): base(
					processManager,
					ProcessConstraint.ExpireTime,
					glideTime
				){
					thisPopUI = popUI;
					thisPopMode = popMode;
					// thisGlideDistance = glideDistance;
					thisMinGlideDistance = minGlideDistance;
					thisMaxGlideDistance = maxGlideDistance;
					thisNormalizedDistanceCurve = normalizedDistanceCurve;
					thisAlphaCurve = alhpaCurve;
					thisScaleCurve = scaleCurve;
					thisGraphicOrignalLocalPosition = graphicOriginalLocalPosition;
					thisGraphicOriginalColor = graphicOriginalColor;
				}
				readonly IPopUI thisPopUI;
				public IPopUI popUI{
					get{
						return thisPopUI;
					}
				}
				readonly PopUIAdaptor.PopMode thisPopMode;
				public PopUIAdaptor.PopMode popMode{
					get{
						return thisPopMode;
					}
				}
				readonly float thisMinGlideDistance;
				public float minGlideDistance{
					get{return thisMinGlideDistance;}
				}
				readonly float thisMaxGlideDistance;
				public float maxGlideDistance{
					get{
						return thisMaxGlideDistance;
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

				readonly Vector2 thisGraphicOrignalLocalPosition;
				public Vector2 graphicOriginalLocalPosition{get{return thisGraphicOrignalLocalPosition;}}
				readonly Color thisGraphicOriginalColor;
				public Color graphicOriginalColor{get{return thisGraphicOriginalColor;}}
			}
		/*  */
	}
}
