using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IMarkerUIAdaptor: ISceneUIAdaptor{
		void StartMark();
		void StopMark();
		void ResetAtReserve();
		void UpdateSceneUI();
		void TriggerActivationOnAnimator();
		void TriggerDeactivationOnAnimator();
		void BecomeChildToCanvas();
	}
	[RequireComponent(typeof(Animator))]
	public class MarkerUIAdaptor : AbsSceneUIAdaptor, IMarkerUIAdaptor {

		protected override ISceneUI CreateSceneUI(){
			AbsSceneUI.IConstArg arg = new AbsSceneUI.ConstArg(
				uiCamera,
				this,
				minUISize,
				maxUISize,
				nearUIDistance,
				farUIDistance
			);
			return new MarkerUI(arg);
		}
		public ProcessManager processManager;
		public override void SetUp(){
			base.SetUp();
			thisProcessFactory = new AppleShooterProcessFactory(
				processManager
			);
			thisCanvas = CollectCanvasFromParent();
			thisActivationHash = Animator.StringToHash(
				"Activate"
			);
			thisDeactivationHash = Animator.StringToHash(
				"Deactivate"
			);
			thisAnimator = CollectAnimator();
		}
		public override void FinalizeSetUp(){
			thisSceneUI.Deactivate();
		}

		IAppleShooterProcessFactory thisProcessFactory;
		IMarkerUIMarkProcess thisProcess;
		public void StartMark(){
			StopMark();
			thisProcess = thisProcessFactory.CreateMarkerUIMarkProcess(
				(IMarkerUI)thisSceneUI
			);
			thisProcess.Run();
		}
		public void StopMark(){
			if(thisProcess != null && thisProcess.IsRunning())
				thisProcess.Stop();
			thisProcess = null;
		}

		public RectTransform thisReserveRectTransform;
		public void ResetAtReserve(){
			this.transform.SetParent(thisReserveRectTransform);
			this.transform.localPosition = Vector3.zero;
			OnResetAtReserve();
		}
		void OnResetAtReserve(){
			StopMark();
		}
		public void UpdateSceneUI(){
			thisSceneUI.SetUIWorldPosition(targetTransform.position);
			thisSceneUI.UpdateUI();
		}
		int thisActivationHash;
		int thisDeactivationHash;
		Animator thisAnimator;
		Animator CollectAnimator(){
			return this.transform.GetComponent<Animator>() as Animator;
		}
		public void TriggerActivationOnAnimator(){
			thisAnimator.SetTrigger(
				thisActivationHash
			);
		}
		public void TriggerDeactivationOnAnimator(){
			thisAnimator.SetTrigger(
				thisDeactivationHash
			);
		}
		Canvas thisCanvas;
		Canvas CollectCanvasFromParent(){
			return this.GetComponentInParent<Canvas>() as Canvas;
		}
		public void BecomeChildToCanvas(){
			this.transform.SetParent(thisCanvas.transform);
		}
	}
}
