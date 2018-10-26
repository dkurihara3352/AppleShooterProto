using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
using UnityEngine.UI;

namespace AppleShooterProto{
	public interface IPopUIAdaptor: ISceneUIAdaptor{
		void Pop();
		void SetUIAlpha(float alpha);
		void DeactivateUI();
		void StopMark();
		Graphic GetChildGraphic();
		Color GetChildGraphicOriginalColor();
		Vector2 GetChildGraphicOriginalLocalPosition();
	}
	
	public class PopUIAdaptor: AbsSceneUIAdaptor, IPopUIAdaptor{
		protected override ISceneUI CreateSceneUI(){
			AbsSceneUI.IConstArg arg = new AbsSceneUI.ConstArg(
				uiCamera,
				this,
				minUISize,
				maxUISize,
				nearUIDistance,
				farUIDistance
			);
			return new PopUI(arg);
		}
		public override void SetUp(){
			base.SetUp();
			thisGraphic = CollectGraphic();
			thisOriginalColor  = thisGraphic.color;
			thisChildGraphicOriginalColor = thisGraphic.color;
			thisChildGraphicOriginalLocalPosition = thisGraphic.transform.localPosition;

		}
		public enum PopMode
		{
			PopStatic,
			GlideUp,
			GlideRandom	
		}
		public PopMode popMode;
		IPopUIGlideProcess thisProcess;
		public void Pop(){
			thisSceneUI.UpdateUI();
			StartMark();
			StartGlide();
		}
		public float glideTime = .5f;
		public float glideDistance;
		public AnimationCurve normalizedDistanceCurve;
		public AnimationCurve alphaCurve;
		public AnimationCurve scaleCurve;
		IMarkerUIMarkProcess thisMarkProcess;
		void StartMark(){
			StopMark();
			thisMarkProcess = processFactory.CreateMarkerUIMarkProcess(
				thisSceneUI
			);
			thisMarkProcess.Run();
		}
		public void StopMark(){
			if(thisMarkProcess != null && thisMarkProcess.IsRunning())
				thisMarkProcess.Stop();
			thisMarkProcess = null;
		}
		void StartGlide(){
			StopGlide();
			thisProcess = processFactory.CreatePopUIGlideProcess(
				this,
				popMode,
				glideTime,
				glideDistance,
				normalizedDistanceCurve,
				alphaCurve,
				scaleCurve
			);
			thisProcess.Run();
		}
		void StopGlide(){
			if(thisProcess != null && thisProcess.IsRunning())
				thisProcess.Stop();
			thisProcess = null;
		}
		Graphic thisGraphic;
		Color thisChildGraphicOriginalColor;
		Vector2 thisChildGraphicOriginalLocalPosition;
		public Vector2 GetChildGraphicOriginalLocalPosition(){
			return thisChildGraphicOriginalLocalPosition;
		}
		Graphic CollectGraphic(){
			return this.transform.GetComponentInChildren<Graphic>();
		}
		public Graphic GetChildGraphic(){
			return thisGraphic;
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
		protected override void OnResetAtReserve(){
			return;
		}
		public void DeactivateUI(){
			thisSceneUI.Deactivate();
		}
		public Color GetChildGraphicOriginalColor(){
			return thisChildGraphicOriginalColor;
		}
	}
}
