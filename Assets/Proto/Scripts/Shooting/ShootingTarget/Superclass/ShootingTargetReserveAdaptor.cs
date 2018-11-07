using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetReserveAdaptor: IMonoBehaviourAdaptor{
		IShootingTargetReserve GetReserve();
	}
	public abstract class AbsShootingTargetReserveAdaptor : MonoBehaviourAdaptor, IShootingTargetReserveAdaptor {
		protected IShootingTargetReserve thisReserve;
		public IShootingTargetReserve GetReserve(){
			return thisReserve;
		}
		public float reservedSpace;
	}
}
