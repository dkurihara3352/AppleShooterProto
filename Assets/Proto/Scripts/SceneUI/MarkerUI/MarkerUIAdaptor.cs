using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IMarkerUIAdaptor: ISceneUIAdaptor{
		IMarkerUI GetMarkerUI();
		void SetMarkerUIReserve(IMarkerUIReserve reserve);
		void TriggerActivationOnAnimator();
		void TriggerDeactivationOnAnimator();
		void SetIndex(int index);
	}
	[RequireComponent(typeof(Animator))]
	public class MarkerUIAdaptor : AbsSceneUIAdaptor, IMarkerUIAdaptor {

		protected override ISceneUI CreateSceneUI(){
			AbsSceneUI.IConstArg arg = new AbsSceneUI.ConstArg(
				this,
				thisCamera,
				minUISize,
				maxUISize,
				nearUIDistance,
				farUIDistance,
				thisIndex
			);
			return new MarkerUI(arg);
		}

		public override void SetUp(){
			base.SetUp();
			thisActivationHash = Animator.StringToHash(
				"Activate"
			);
			thisDeactivationHash = Animator.StringToHash(
				"Deactivate"
			);
			thisAnimator = CollectAnimator();
		}
		IMarkerUI thisMarkerUI{
			get{
				return (IMarkerUI)thisSceneUI;
			}
		}
		public IMarkerUI GetMarkerUI(){
			return thisMarkerUI;
		}
		public override void SetUpReference(){
			thisMarkerUI.SetMarkerUIReserve(thisReserve);
		}
		IMarkerUIReserve thisReserve;
		public void SetMarkerUIReserve(IMarkerUIReserve reserve){
			thisReserve = reserve;
		}
		public override void FinalizeSetUp(){
			thisSceneUI.Deactivate();
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
		int thisIndex;
		public void SetIndex(int index){
			thisIndex = index;
		}
	}
}
