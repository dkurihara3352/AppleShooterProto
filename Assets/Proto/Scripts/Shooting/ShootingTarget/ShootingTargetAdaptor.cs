using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetAdaptor: IMonoBehaviourAdaptor{
		IShootingTarget GetShootingTarget();
		void ResetTransformAtReserve();
		void PopText(string text);
	}
	public abstract class ShootingTargetAdaptor: MonoBehaviourAdaptor, IShootingTargetAdaptor{
		public override void SetUpReference(){
			if(popUIReserveAdaptor != null)
				thisPopUIReserve = popUIReserveAdaptor.GetPopUIReserve();
			if(destroyedTargetReserveAdaptor != null){
				IDestroyedTargetReserve reserve = destroyedTargetReserveAdaptor.GetDestroyedTargetReserve();
				thisShootingTarget.SetDestroyedTargetReserve(reserve);
			}
		}
		public override void FinalizeSetUp(){
			thisShootingTarget.Deactivate();
		}
		public IShootingTarget GetShootingTarget(){
			return thisShootingTarget;
		}
		protected IShootingTarget thisShootingTarget;
		public Transform reserveTransform;
		public void ResetTransformAtReserve(){
			SetParent(reserveTransform);
			ResetLocalTransform();
		}
		public PopUIReserveAdaptor popUIReserveAdaptor;
		public IPopUIReserve thisPopUIReserve;
		public DestroyedTargetReserveAdaptor destroyedTargetReserveAdaptor;
		public void PopText(string text){
			IPopUI popUI = thisPopUIReserve.GetNextPopUI();
			popUI.SetTargetTransform(this.GetTransform());
			popUI.SetText(text);
			popUI.Activate();
		}
	}
	public interface IInstatiableShootingTargetAdaptor: ITestShootingTargetAdaptor, IInstatiableMonoBehaviourAdaptor{}
	public abstract class InstatiableShootingTargetAdaptor: TestShootingTargetAdaptor, IInstatiableShootingTargetAdaptor{
		protected override sealed void Awake(){
			return;
		}
		public void SetMonoBehaviourAdaptorManager(IMonoBehaviourAdaptorManager manager){
			thisMonoBehaviourAdaptorManager =  manager;
		}
	}
}
