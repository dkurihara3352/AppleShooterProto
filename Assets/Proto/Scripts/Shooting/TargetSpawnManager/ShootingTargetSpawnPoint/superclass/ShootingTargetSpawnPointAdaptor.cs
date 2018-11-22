using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnPointAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IShootingTargetSpawnPoint GetSpawnPoint();
		float GetEventPoint();
	}
	public abstract class AbsShootingTargetSpawnPointAdaptor: AppleShooterMonoBehaviourAdaptor, IShootingTargetSpawnPointAdaptor{
		protected IShootingTargetSpawnPoint thisSpawnPoint;
		public IShootingTargetSpawnPoint GetSpawnPoint(){
			return thisSpawnPoint;
		}
		[Range(0f, 1f)]
		public float eventPoint;
		public float GetEventPoint(){
			return eventPoint;
		}
	}
}

