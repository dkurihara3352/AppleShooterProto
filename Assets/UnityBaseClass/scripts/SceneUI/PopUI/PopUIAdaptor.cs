using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
using UnityEngine.UI;

namespace UnityBase{
	public interface IPopUIAdaptor: ISceneUIAdaptor{
		void SetPopUIReserve(IPopUIReserve reserve);
		IPopUI GetPopUI();

		void SetUIAlpha(float alpha);

		void SetChildGraphicColor(Color color);
		void SetChildGraphicScale(Vector3 scale);
		void SetChildGraphicLocalPosition(Vector3 position);

		void SetText(string text);
		string GetText();

		void SetIndex(int index);
		Rect GetGraphicRect();
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
				minGlideDistance,
				maxGlideDistance,
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
		public override bool IsCompletelyOutOfScreenBounds(){
			Vector3[] cornersArray = new Vector3[4];
			thisGraphic.rectTransform.GetWorldCorners(cornersArray);
			Vector3 min = cornersArray[0];
			Vector3 max = cornersArray[2];

			Vector2 screenSize = new Vector2(Screen.width, Screen.height);
			if(
				min.x > screenSize.x ||
				min.y > screenSize.y ||
				max.x < 0f ||
				max.y < 0f
			)
				return true;
			return false;
		}
		bool debugEnabled = false;
		void Update(){
			if(debugEnabled)
				if(thisPopUI != null && targetTransform != null)
					SetDepthText();
					// SetGraphicRectSizeText();
		}
		void SetDepthText(){
			Vector3 tarWorPos = GetTargetWorldPosition();
			string zText = thisCamera.WorldToScreenPoint(tarWorPos).z.ToString("N1");
			SetText(zText);
		}
		// void SetGraphicRectSizeText(){
		// 	SetText(thisGraphic.rectTransform.sizeDelta.ToString("N0"));
		// }
		public PopMode popMode;
		IPopUIGlideProcess thisGlideProcess;
		public float glideTime = .5f;
		// public float glideDistance;
		public float minGlideDistance;
		public float maxGlideDistance;
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
		public override void SetUISize(Vector2 size){
			thisGraphic.rectTransform.sizeDelta = size;
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
		public string GetText(){
			if(thisGraphic is Text){
				return ((Text)thisGraphic).text;
			}
			return "";
		}
		int thisIndex;
		public void SetIndex(int index){
			thisIndex = index;
		}
		public Rect GetGraphicRect(){
			return thisGraphic.rectTransform.rect;
		}
		public override void DisableGraphic(){
			thisGraphic.enabled = false;
		}
		public override void EnableGraphic(){
			thisGraphic.enabled = true;
		}
	}
}
