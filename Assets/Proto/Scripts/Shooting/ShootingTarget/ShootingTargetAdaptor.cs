using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetAdaptor: IMonoBehaviourAdaptor{
		IShootingTarget GetShootingTarget();
	}
	public abstract class ShootingTargetAdaptor: MonoBehaviourAdaptor, IShootingTargetAdaptor{
		public IShootingTarget GetShootingTarget(){
			return thisShootingTarget;
		}
		protected IShootingTarget thisShootingTarget;
	}
}
