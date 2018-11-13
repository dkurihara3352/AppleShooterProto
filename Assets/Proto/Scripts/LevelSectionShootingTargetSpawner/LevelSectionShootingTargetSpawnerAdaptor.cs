using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILevelSectionShootingTargetSpawnerAdaptor: IMonoBehaviourAdaptor{
		ILevelSectionShootingTargetSpawner GetSpawner();
	}	
	public class LevelSectionShootingTargetSpawnerAdaptor : MonoBehaviourAdaptor, ILevelSectionShootingTargetSpawnerAdaptor {
		public override void SetUp(){
			thisSpawner = CreateSpawner();
		}
		ILevelSectionShootingTargetSpawner thisSpawner;
		public ILevelSectionShootingTargetSpawner GetSpawner(){
			return thisSpawner;
		}
		public float spawnValueLimit;
		ILevelSectionShootingTargetSpawner CreateSpawner(){
			LevelSectionShootingTargetSpawner.IConstArg arg = new LevelSectionShootingTargetSpawner.ConstArg(
				this,
				spawnValueLimit
			);
			return new LevelSectionShootingTargetSpawner(arg);
		}

		public override void SetUpReference(){
			IShootingTargetSpawnManager manager = GetManager();
			thisSpawner.SetShootingTargetSpawnManager(manager);

			thisSpawner.SetLevelSectionTargetSpawnDataInput(spawnDataInput);

		}
		public ShootingTargetSpawnManagerAdaptor managerAdaptor;
		IShootingTargetSpawnManager GetManager(){
			return managerAdaptor.GetManager();
		}
		public TargetSpawnDataInput[] spawnDataInput;

	}
	[System.Serializable]
	public struct TargetSpawnDataInput{
		public TargetType targetType;
		public AbsShootingTargetReserveAdaptor reserveAdaptor;
		public IShootingTargetReserve reserve{
			get{
				return reserveAdaptor.GetReserve();
			}
		}
		public float spawnValue{
			get{return reserveAdaptor.GetSpawnValue();}
		}
		public float relativeProbability;
		public int maxCount;
		public AbsShootingTargetSpawnPointGroupAdaptor spawnPointGroupAdaptor;
	}
}
