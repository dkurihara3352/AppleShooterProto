using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityBase;

namespace UnityBase{
	public interface IPopUI: ISceneUI{
		void SetPopUIReserve(IPopUIReserve reserve);
		void SetText(string text);
		void Pop();
		void ActivateAt(
			ISceneObject targetObj,
			string text
		);

		/* used by process */
		void SetChildGraphicColor(Color color);
		void SetChildGraphicLocalScale(Vector3 scale);
		void SetChildGraphicLocalPosition(Vector3 position);
		ISceneObject GetSceneObject();
	
	}
	public class PopUI: AbsSceneUI, IPopUI{
		public PopUI(
			IConstArg arg
		): base(arg){
			thisPopMode = arg.popMode;
			thisGlideTime = arg.glideTime;
			thisMinGlideDistance = arg.minGlideDistance;
			thisMaxGlideDistance = arg.maxGlideDistance;
			thisNormalizedDistanceCurve = arg.normalizedDistanceCurve;
			thisAlphaCurve = arg.alphaCurve;
			thisScaleCurve = arg.scaleCurve;
			thisGraphicOriginalLocalPosition = arg.graphicOriginalLocalPosition;
			thisGraphicOriginalColor = arg.graphicOriginalColor;
		}
		IPopUIAdaptor thisTypedAdaptor{
			get{
				return (IPopUIAdaptor)thisAdaptor;
			}
		}
		public void ActivateAt(
			ISceneObject obj,
			string text
		){
			Deactivate();
			thisTargetObject = obj;
			SetTargetSceneObject(obj);
			SetText(text);
			Activate();
		}
		public override void ActivateImple(){
			base.ActivateImple();
			Pop();
		}
		public override void DeactivateImple(){
			base.DeactivateImple();
			thisTargetObject = null;
			StopMark();
			StopGlide();

		}
		protected ISceneObject thisTargetObject;
		public ISceneObject GetSceneObject(){
			return thisTargetObject;
		}
		public void Pop(){
			UpdateUI();
			StartMark();
			StartGlide();
		}
		IMarkerUIMarkProcess thisMarkProcess;
		void StartMark(){
			StopMark();
			thisMarkProcess = thisProcessFactory.CreateMarkerUIMarkProcess(
				this
			);
			thisMarkProcess.Run();
		}
		void StopMark(){
			if(thisMarkProcess != null && thisMarkProcess.IsRunning())
				thisMarkProcess.Stop();
			thisMarkProcess = null;
		}
		IPopUIGlideProcess thisGlideProcess;
		readonly PopUIAdaptor.PopMode thisPopMode;
		readonly float thisGlideTime;
		// readonly float thisGlideDistance;
		readonly float thisMinGlideDistance;
		readonly float thisMaxGlideDistance;
		readonly AnimationCurve thisNormalizedDistanceCurve;
		readonly AnimationCurve thisAlphaCurve;
		readonly AnimationCurve thisScaleCurve;
		readonly Vector2 thisGraphicOriginalLocalPosition;
		readonly Color thisGraphicOriginalColor;

		void StartGlide(){
			StopGlide();
			thisGlideProcess = thisProcessFactory.CreatePopUIGlideProcess(
				this,
				thisPopMode,
				thisGlideTime,
				thisMinGlideDistance,
				thisMaxGlideDistance,
				thisNormalizedDistanceCurve,
				thisAlphaCurve,
				thisScaleCurve,
				thisGraphicOriginalLocalPosition,
				thisGraphicOriginalColor
			);
			thisGlideProcess.Run();
		}
		void StopGlide(){
			if(thisGlideProcess != null && thisGlideProcess.IsRunning())
				thisGlideProcess.Stop();
			thisGlideProcess = null;
		}
		
		public void SetText(string text){
			thisTypedAdaptor.SetText(text);
		}
		IPopUIReserve thisReserve;
		public void SetPopUIReserve(IPopUIReserve reserve){
			thisReserve = reserve;
		}
		protected override void Reserve(){
			thisReserve.Reserve(this);
		}

		/* Process */
			public void SetChildGraphicColor(Color color){
				thisTypedAdaptor.SetChildGraphicColor(color);
			}
			public void SetChildGraphicLocalScale(Vector3 scale){
				thisTypedAdaptor.SetChildGraphicScale(scale);
			}
			public void SetChildGraphicLocalPosition(Vector3 position){
				thisTypedAdaptor.SetChildGraphicLocalPosition(position);
			}
		/* Const */
			public new interface IConstArg: AbsSceneUI.IConstArg{
				PopUIAdaptor.PopMode popMode{get;}
				float glideTime{get;}
				// float glideDistance{get;}
				float minGlideDistance{get;}
				float maxGlideDistance{get;}
				AnimationCurve normalizedDistanceCurve{get;}
				AnimationCurve alphaCurve{get;}
				AnimationCurve scaleCurve{get;}
				Vector2 graphicOriginalLocalPosition{get;}
				Color graphicOriginalColor{get;}
			}
			public new class ConstArg: AbsSceneUI.ConstArg, IConstArg{
				public ConstArg(
					IPopUIAdaptor adaptor,
					Camera uiCamera,
					Vector2 minUISize,
					Vector2 maxUISize,
					float nearUIDistance,
					float farUIDistance,
					int index,

					PopUIAdaptor.PopMode popMode,
					float glideTime,
					// float glideDistance,
					float minGlideDistance,
					float maxGlideDistance,
					AnimationCurve normalizedDistanceCurve,
					AnimationCurve alphaCurve,
					AnimationCurve scaleCurve,
					Vector2 graphicOriginalLocalPosition,
					Color graphicOriginalColor
				): base(
					adaptor,
					uiCamera,
					minUISize,
					maxUISize,
					nearUIDistance,
					farUIDistance,
					index
				){
					thisPopMode = popMode;
					thisGlideTime = glideTime;
					// thisGlideDistance = glideDistance;
					thisMinGlideDistance = minGlideDistance;
					thisMaxGlideDistance = maxGlideDistance;
					thisNormalizedDistanceCurve = normalizedDistanceCurve;
					thisAlphaCurve = alphaCurve;
					thisScaleCurve = scaleCurve;
					thisGraphicOriginalLocalPosition = graphicOriginalLocalPosition;
					thisGraphicOriginalColor = graphicOriginalColor;
				}
				readonly PopUIAdaptor.PopMode thisPopMode;
				public PopUIAdaptor.PopMode popMode{get{return thisPopMode;}}
				readonly float thisGlideTime;
				public float glideTime{get{return thisGlideTime;}}
				// readonly float thisGlideDistance;
				// public float glideDistance{get{return thisGlideDistance;}}
				readonly float thisMinGlideDistance;
				public float minGlideDistance{get{return thisMinGlideDistance;}}
				readonly float thisMaxGlideDistance;
				public float maxGlideDistance{get{return thisMaxGlideDistance;}}
				readonly AnimationCurve thisNormalizedDistanceCurve;
				public AnimationCurve normalizedDistanceCurve{get{return thisNormalizedDistanceCurve;}}
				readonly AnimationCurve thisAlphaCurve;
				public AnimationCurve alphaCurve{get{return thisAlphaCurve;}}
				readonly AnimationCurve thisScaleCurve;
				public AnimationCurve scaleCurve{get{return thisScaleCurve;}}
				readonly Vector2 thisGraphicOriginalLocalPosition;
				public Vector2 graphicOriginalLocalPosition{get{return thisGraphicOriginalLocalPosition;}}
				readonly Color thisGraphicOriginalColor;
				public Color graphicOriginalColor{get{return thisGraphicOriginalColor;}}
			}
		/*  */
	}
}

