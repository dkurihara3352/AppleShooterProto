using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace UISystem{
	public interface IHoldButton: IUIElement{
		void SetHoldIndicatorImage(IHoldIndicatorImage image);
	}
	public abstract class HoldButton: UIElement, IHoldButton, IProcessHandler{
		public HoldButton(IConstArg arg): base(arg){
			thisHoldProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisHoldButtonAdaptor.GetHoldTime()
			);
		}
		IHoldButtonAdaptor thisHoldButtonAdaptor{
			get{
				return (IHoldButtonAdaptor)thisUIAdaptor;
			}
		}
		IHoldIndicatorImage thisHoldIndicatorImage;
		IProcessSuite thisHoldProcessSuite;
		public void SetHoldIndicatorImage(IHoldIndicatorImage image){
			thisHoldIndicatorImage = image;
		}
		protected override void OnTouchImple(int touchCount){
			StartButtonHoldProcess();
		}
		protected override void OnReleaseImple(){
			StopButtonHoldProcess();
		}
		protected override void OnTapImple(int tapCount){
			StopButtonHoldProcess();
		}
		protected override void OnSwipeImple(ICustomEventData eventData){
			base.OnSwipeImple(eventData);
			StopButtonHoldProcess();
		}
		protected abstract void OnHoldButtonExecute();

		void StartButtonHoldProcess(){
			thisHoldIndicatorImage.SetHoldValue(0f);
			thisHoldProcessSuite.Start();
		}
		void StopButtonHoldProcess(){
			thisHoldProcessSuite.Stop();
			thisHoldIndicatorImage.SetHoldValue(0f);
			thisHoldIndicatorImage.DeactivateRecursively(false);
		}
		public void OnProcessRun(IProcessSuite suite){}
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisHoldProcessSuite){
				if(!thisHoldIndicatorImage.IsActivated()){
					if(normalizedTime > thisIndicatorActivationDelayNormalizedTime){
						thisHoldIndicatorImage.ActivateRecursively(false);
						thisHoldIndicatorImage.EvaluateScrollerFocusRecursively();
					}
				}else
					thisHoldIndicatorImage.SetHoldValue(normalizedTime);
			}
		}
		float thisIndicatorActivationDelayNormalizedTime{
			get{
				return thisHoldButtonAdaptor.GetIndicatorActivationDelayTime()/ thisHoldButtonAdaptor.GetHoldTime();
			}
		}
		public void OnProcessExpire(IProcessSuite suite){
			if(suite == thisHoldProcessSuite){
				// thisHoldIndicatorImage.SetHoldValue(1f);
				thisHoldIndicatorImage.DeactivateRecursively(false);
				this.OnHoldButtonExecute();
			}
		}
		public override void ActivateRecursively(bool instantly){
			ActivateSelf(instantly);
		}
	}
}

