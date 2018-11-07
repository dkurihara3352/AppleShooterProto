using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface IStaticTargetSpawnPointAdaptor: IShootingTargetSpawnPointAdaptor{
		IStaticTargetSpawnPoint GetStaticTargetSpawnPoint();
	}
	public class ShootingTargetSpawnPointAdaptor : AbsShootingTargetSpawnPointAdaptor, IStaticTargetSpawnPointAdaptor {

		public override void SetUp(){
			thisSpawnPoint = CreateSpawnPoint();
		}
		IStaticTargetSpawnPoint CreateSpawnPoint(){
			StaticTargetSpawnPoint.IConstArg arg = new StaticTargetSpawnPoint.ConstArg(this);
			return new StaticTargetSpawnPoint(arg);
		}
		public IStaticTargetSpawnPoint GetStaticTargetSpawnPoint(){
			return (IStaticTargetSpawnPoint)thisSpawnPoint;
		}
	}
}
