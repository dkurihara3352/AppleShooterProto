using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
using UnityEngine.UI;

namespace AppleShooterProto{
	public interface IPopUIAdaptor: ISceneUIAdaptor{
		void SetPopUIReserve(IPopUIReserve reserve);
		IPopUI GetPopUI();

		void SetUIAlpha(float alpha);

		void SetChildGraphicColor(Color color);
		void SetChildGraphicScale(Vector3 scale);
		void SetChildGraphicLocalPosition(Vector3 position);

		void SetText(string text);

		void SetIndex(int index);
	}
	
	public class PopUIAdaptor: AbsSceneUIAdaptor, IPopUIAdaptor{
		protected override ISceneUI CreateSceneUI(){
			PopUI.IConstArg arg = new PopUI.ConstArg(
				this,
				thisCamera,
				minUISize,
				maxUISize,
				nearUIDistance,
				farUIDistance,
				thisIndex,

				popMode,
				glideTime,
				glideDistance,
				normalizedDistanceCurve,
				alphaCurve,
				scaleCurve,
				thisChildGraphicOriginalLocalPosition,
				thisChildGraphicOriginalColor
			);
			return new PopUI(arg);
		}
		public override void SetUp(){
			thisGraphic = CollectGraphic();
			thisOriginalColor  = thisGraphic.color;
			thisChildGraphicOriginalColor = thisGraphic.color;
			thisChildGraphicOriginalLocalPosition = thisGraphic.transform.localPosition;
			base.SetUp();
		}
		IPopUIReserve thisPopUIReserve;
		public override void SetUpReference(){
			thisPopUI.SetPopUIReserve(thisPopUIReserve);
		}
		public void SetPopUIReserve(IPopUIReserve popUIReserve){
			thisPopUIReserve = popUIReserve;
		}
		public enum PopMode
		{
			PopStatic,
			GlideUp,
			GlideRandom	
		}
		public PopMode popMode;
		IPopUIGlideProcess thisGlideProcess;
		public float glideTime = .5f;
		public float glideDistance;
		public AnimationCurve normalizedDistanceCurve;
		public AnimationCurve alphaCurve;
		public AnimationCurve scaleCurve;

		Graphic thisGraphic;
		Color thisChildGraphicOriginalColor;
		Vector2 thisChildGraphicOriginalLocalPosition;
		public Vector2 GetChildGraphicOriginalLocalPosition(){
			return thisChildGraphicOriginalLocalPosition;
		}
		Graphic CollectGraphic(){
			return this.transform.GetComponentInChildren<Graphic>();
		}
		public void SetChildGraphicColor(Color color){
			thisGraphic.color = color;
		}
		public void SetChildGraphicScale(Vector3 scale){
			thisGraphic.transform.localScale = scale;
		}
		public void SetChildGraphicLocalPosition(Vector3 position){
			thisGraphic.transform.localPosition = position;
		}
		Color thisOriginalColor;
		public void SetUIAlpha(float alpha){
			float originalAlpha = thisOriginalColor.a;
			float targetAlpha = Mathf.Lerp(
				0f,
				originalAlpha,
				alpha
			);
			Color newColor = new Color(
				thisOriginalColor.r,
				thisOriginalColor.g,
				thisOriginalColor.b,
				targetAlpha
			);
			thisGraphic.color = newColor;
		}	
		public Color GetChildGraphicOriginalColor(){
			return thisChildGraphicOriginalColor;
		}
		protected IPopUI thisPopUI{
			get{
				return (IPopUI)thisSceneUI;
			}
		}
		public IPopUI GetPopUI(){
			return thisPopUI;
		}
		public void SetText(string text){
			if(thisGraphic is Text)
				((Text)thisGraphic).text = text;
		}
		int thisIndex;
		public void SetIndex(int index){
			thisIndex = index;
		}
	}
}
