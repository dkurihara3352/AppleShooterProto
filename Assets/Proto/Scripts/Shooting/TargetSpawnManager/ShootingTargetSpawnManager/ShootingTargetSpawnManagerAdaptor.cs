using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnManagerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IShootingTargetSpawnManager GetManager();
	}
	public class ShootingTargetSpawnManagerAdaptor : AppleShooterMonoBehaviourAdaptor, IShootingTargetSpawnManagerAdaptor {
		
		IShootingTargetSpawnManager thisManager;
		public IShootingTargetSpawnManager GetManager(){
			return thisManager;
		}
		public override void SetUp(){
			thisManager = CreateManager();
		}
		IShootingTargetSpawnManager CreateManager(){
			ShootingTargetSpawnManager.IConstArg arg = new ShootingTargetSpawnManager.ConstArg(
				this,
				staticTargetSpawnValue,
				fattyTargetSpawnValue,
				glidingTargetSpawnValue
			);
			return new ShootingTargetSpawnManager(arg);
		}

		public float staticTargetSpawnValue;
		public float fattyTargetSpawnValue;
		public float glidingTargetSpawnValue;
	}
}
