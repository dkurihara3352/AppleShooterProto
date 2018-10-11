using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetReserve<T> where T: IShootingTarget{
		void Reserve(T target);
		T Unreserve();
		void GetTargetsReadyInReserve();
	}
}

