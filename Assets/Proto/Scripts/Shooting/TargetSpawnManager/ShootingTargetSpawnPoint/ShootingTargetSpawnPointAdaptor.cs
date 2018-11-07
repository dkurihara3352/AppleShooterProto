using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnPointAdaptor: IMonoBehaviourAdaptor{
		IShootingTargetSpawnPoint GetSpawnPoint();
	}
	public abstract class AbsShootingTargetSpawnPointAdaptor: MonoBehaviourAdaptor, IShootingTargetSpawnPointAdaptor{
		protected IShootingTargetSpawnPoint thisSpawnPoint;
		public IShootingTargetSpawnPoint GetSpawnPoint(){
			return thisSpawnPoint;
		}
	}
}

