using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILevelSectionShootingTargetSpawnerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		ILevelSectionShootingTargetSpawner GetSpawner();
	}	
	public class LevelSectionShootingTargetSpawnerAdaptor : AppleShooterMonoBehaviourAdaptor, ILevelSectionShootingTargetSpawnerAdaptor {
		public override void SetUp(){
			thisSpawner = CreateSpawner();
		}
		ILevelSectionShootingTargetSpawner thisSpawner;
		public ILevelSectionShootingTargetSpawner GetSpawner(){
			return thisSpawner;
		}
		public int spawnValueLimit;
		ILevelSectionShootingTargetSpawner CreateSpawner(){
			LevelSectionShootingTargetSpawner.IConstArg arg = new LevelSectionShootingTargetSpawner.ConstArg(
				this,
				spawnValueLimit
			);
			return new LevelSectionShootingTargetSpawner(arg);
		}

		public override void SetUpReference(){
			thisSpawner.SetLevelSectionTargetSpawnDataInput(spawnDataInput);
			IPlayerCharacterWaypointsFollower follower = GetFollower();
			thisSpawner.SetPlayerCharacterWaypointsFollower(follower);
		}
		public TargetSpawnDataInput[] spawnDataInput;
		public PlayerCharacterWaypointsFollowerAdaptor playerCharacterWaypointsFollowerAdaptor;
		IPlayerCharacterWaypointsFollower GetFollower(){
			return playerCharacterWaypointsFollowerAdaptor.GetPlayerCharacterWaypointsFollower();
		}

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
		public int spawnValue{
			get{return reserveAdaptor.GetSpawnValue();}
		}
		public float relativeProbability;
		public int maxCount;
		public AbsShootingTargetSpawnPointGroupAdaptor spawnPointGroupAdaptor;
	}
}
