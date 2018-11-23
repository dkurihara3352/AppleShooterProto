using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBase{
	public interface IMarkerUI: ISceneUI{
		void SetMarkerUIReserve(IMarkerUIReserve reserve);
		void OnDeactivationAnimationEnd();
		void UpdateMarkerUI();
	}
	public class MarkerUI : AbsSceneUI, IMarkerUI {

		public MarkerUI(
			IConstArg arg
		): base(arg){
		}
		IMarkerUIAdaptor thisTypedAdaptor{
			get{
				return (IMarkerUIAdaptor)thisAdaptor;
			}
		}
		public override void DeactivateImple(){
			// base.DeactivateImple();
			thisTypedAdaptor.TriggerDeactivationOnAnimator();
		}
		public override void ActivateImple(){
			base.ActivateImple();
			thisTypedAdaptor.BecomeChildToCanvas();
			StartMark();
			thisTypedAdaptor.TriggerActivationOnAnimator();
		}
		IMarkerUIMarkProcess thisProcess;
		void StartMark(){
			StopMark();
			thisProcess = thisProcessFactory.CreateMarkerUIMarkProcess(
				this
			);
			thisProcess.Run();
		}
		void StopMark(){
			if(thisProcess != null && thisProcess.IsRunning())
				thisProcess.Stop();
			thisProcess = null;
		}
		public void OnDeactivationAnimationEnd(){
			if(thisReserve != null)
				Reserve();
			StopMark();
		}
		public void UpdateMarkerUI(){
			Vector3 targetPosition = thisTypedAdaptor.GetTargetWorldPosition();
			SetUIWorldPosition(targetPosition);
			UpdateUI();
		}
		IMarkerUIReserve thisReserve;
		public void SetMarkerUIReserve(IMarkerUIReserve reserve){
			thisReserve = reserve;
		}
		protected override void Reserve(){
			thisReserve.Reserve(this);
		}
	}
}
