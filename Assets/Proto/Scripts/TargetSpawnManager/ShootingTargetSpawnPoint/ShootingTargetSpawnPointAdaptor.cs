using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface IShootingTargetSpawnPointAdaptor: IMonoBehaviourAdaptor{
		IShootingTargetSpawnPoint GetShootingTargetSpawnPoint();
	}
	public class ShootingTargetSpawnPointAdaptor : MonoBehaviourAdaptor, IShootingTargetSpawnPointAdaptor {

		public override void SetUp(){
			ShootingTargetSpawnPoint.IConstArg arg = new ShootingTargetSpawnPoint.ConstArg(this);
			thisSpawnPoint = new ShootingTargetSpawnPoint(arg);
		}
		IShootingTargetSpawnPoint thisSpawnPoint;
		public IShootingTargetSpawnPoint GetShootingTargetSpawnPoint(){
			return thisSpawnPoint;
		}

	}
}
