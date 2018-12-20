using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILevelSectionShootingTargetSpawnerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		ILevelSectionShootingTargetSpawner GetSpawner();
		IShootingTargetReserve[] GetRareTargetReserves();
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

			thisRareTargetReserves = CollectRareTargetReserves();
		}
		public TargetSpawnDataInput[] spawnDataInput;
		public PlayerCharacterWaypointsFollowerAdaptor playerCharacterWaypointsFollowerAdaptor;
		IPlayerCharacterWaypointsFollower GetFollower(){
			return playerCharacterWaypointsFollowerAdaptor.GetPlayerCharacterWaypointsFollower();
		}
		public AbsShootingTargetReserveAdaptor[] rareTargetReserveAdaptors;
		IShootingTargetReserve[] thisRareTargetReserves;
		public IShootingTargetReserve[] GetRareTargetReserves(){
			return thisRareTargetReserves;
		}
		IShootingTargetReserve[] CollectRareTargetReserves(){
			List<IShootingTargetReserve> resultList = new List<IShootingTargetReserve>();
			foreach(AbsShootingTargetReserveAdaptor rareTargetReserveAdaptor in rareTargetReserveAdaptors){
				IShootingTargetReserve rareTargetReserve = rareTargetReserveAdaptor.GetReserve();
				resultList.Add(rareTargetReserve);
			}
			return resultList.ToArray();
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
