using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetAdaptor: IMonoBehaviourAdaptor{
		IShootingTarget GetShootingTarget();
		void ResetTransformAtReserve();
	}
	public abstract class ShootingTargetAdaptor: MonoBehaviourAdaptor, IShootingTargetAdaptor{
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
	}
}
