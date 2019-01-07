using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlickBowShooting{
	public interface ILevelSectionShootingTargetSpawner: ISlickBowShootingSceneObject{
		void SetPlayerCharacterWaypointsFollower(IPlayerCharacterWaypointsFollower follower);
		void SetLevelSectionTargetSpawnDataInput(
			TargetSpawnDataInput[] input
		);
		
		void SetUpSpawnWaypointEvents();
		IShootingTargetSpawnWaypointEvent[] GetSpawnWaypointEvents();
		void Despawn();
		bool ShouldSpawnTargets();
	}
	public class LevelSectionShootingTargetSpawner : SlickBowShootingSceneObject, ILevelSectionShootingTargetSpawner {

		public LevelSectionShootingTargetSpawner(
			IConstArg arg
		): base(arg){
			thisSpawnValueLimit = arg.spawnValueLimit;
		}
		ILevelSectionShootingTargetSpawnerAdaptor thisLevelSectionShootingTargetSpawnerAdaptor{
			get{
				return (ILevelSectionShootingTargetSpawnerAdaptor)thisAdaptor;
			}
		}
		int thisSpawnValueLimit;
		TargetSpawnDataInput[] thisSpawnDataInput;
		public void SetLevelSectionTargetSpawnDataInput(TargetSpawnDataInput[] input){
			thisSpawnDataInput = input;
		}
		public void SetUpSpawnWaypointEvents(){
			IShootingTargetSpawnDataCalculator calculator = CreateCalculator();
			TargetSpawnData spawnData = calculator.CalculateTargetSpawnDataByTargetType();
			TargetSpawnData.Entry[] entries = spawnData.GetEntries();
			IShootingTargetSpawnWaypointEvent[] spawnWaypointEvents = CreateSpawnWaypointEvents(entries);

			thisSpawnEvents = spawnWaypointEvents;
		}
		IShootingTargetSpawnWaypointEvent[] CreateSpawnWaypointEvents(TargetSpawnData.Entry[] entries){

			List<IShootingTargetSpawnWaypointEvent> resultList = new List<IShootingTargetSpawnWaypointEvent>();

			foreach(TargetSpawnData.Entry entry in entries){
			
				int spawnCount = entry.numToCreate;
				IShootingTargetSpawnPoint[] spawnPoints = entry.spawnPoints;
				int spawnPointCount = spawnPoints.Length;

				int[] spawnPointIndicesToSpawn = DKUtility.Calculator.GetRandomIntegers(
					count: spawnCount, 
					maxNumber: spawnPointCount -1
				);
				
				IShootingTargetSpawnWaypointEvent[] eventsForEntry = CreateSpawnEventsForEntry(
					// entry,
					spawnPoints,
					entry.reserve,
					spawnPointIndicesToSpawn
				);
				resultList.AddRange(eventsForEntry);

			}
			return resultList.ToArray();
		}
		protected IShootingTargetSpawnWaypointEvent[] CreateSpawnEventsForEntry(
			IShootingTargetSpawnPoint[] spawnPoints,
			IShootingTargetReserve reserve,
			int[] indicesToSpawn
		){
			List<IShootingTargetSpawnWaypointEvent> resultList = new List<IShootingTargetSpawnWaypointEvent>();

			float sumOfTargetRareProbability = GetSumOfTargetTypeRareChance(
				indicesToSpawn.Length,
				reserve.GetTargetTypeRareProbability()
			);
			float sumOfRelativeRareChance = GetSumOfRelativeRareChance(
				spawnPoints,
				indicesToSpawn
			);

			foreach(int spawnPointIndex in indicesToSpawn){
				IShootingTargetSpawnPoint spawnPoint = spawnPoints[spawnPointIndex];

				IShootingTargetReserve resultReserve = reserve;

				float rareProbability = GetRareProbability(
					spawnPoint.GetRelativeRareChance(),
					sumOfRelativeRareChance,
					sumOfTargetRareProbability
				);
				bool isRare = false;

				if(this.ShouldSpawnRare(rareProbability)){
					isRare = true;
					IShootingTargetReserve rareTargetReserve = GetRareTargetReserve(reserve.GetTargetType());
					int currentTier = reserve.GetTier();
					rareTargetReserve.SetTier(currentTier);
					resultReserve = rareTargetReserve;
				}else
					resultReserve = reserve;

				float eventPoint = spawnPoint.GetEventPoint();

				ShootingTargetSpawnWaypointEvent.IConstArg eventConstArg = new ShootingTargetSpawnWaypointEvent.ConstArg(
					resultReserve,
					spawnPoint,
					eventPoint,
					this
				);
				IShootingTargetSpawnWaypointEvent spawnWaypointEvent = new ShootingTargetSpawnWaypointEvent(
					eventConstArg
				);

				if(isRare)
					spawnWaypointEvent.MarkRare();

				resultList.Add(spawnWaypointEvent);
			}
			return resultList.ToArray();
		}
		protected float GetSumOfTargetTypeRareChance(
			float spawnCount,
			float targetTypeRareProbability
		){
			return targetTypeRareProbability * spawnCount;
		}
		protected float GetSumOfRelativeRareChance(
			IShootingTargetSpawnPoint[] spawnPoints,
			int[] indicesToSpawn
		){
			float result = 0f;
			foreach(int index in indicesToSpawn)
				result += spawnPoints[index].GetRelativeRareChance();
			return result;
		}
		protected float GetRareProbability(
			float relativeRareChance,
			float sumOfRelativeRareChance,
			float sumOfTargetRareProbability
		){
			return relativeRareChance/ sumOfRelativeRareChance * sumOfTargetRareProbability;
		}
		protected bool ShouldSpawnRare(float probability){
			if(Random.Range(0f, 1f) <= probability)
				return true;
			return false;
		}
		IShootingTargetReserve[] thisRareTargetReserves{
			get{
				return thisLevelSectionShootingTargetSpawnerAdaptor.GetRareTargetReserves();
			}
		}
		
		protected virtual IShootingTargetReserve GetRareTargetReserve(TargetType targetType){
			foreach(IShootingTargetReserve rareTargetReserve in thisRareTargetReserves){
				if(rareTargetReserve.GetTargetType() == targetType){
					return rareTargetReserve;
				}
			}
			throw new System.InvalidOperationException(
				"there's no rare target reserve with matching targetType"
			);
		}
		protected virtual IShootingTargetSpawnDataCalculator CreateCalculator(){
			ShootingTargetSpawnDataCalculator.IConstArg arg = new ShootingTargetSpawnDataCalculator.ConstArg(
				thisSpawnValueLimit,
				thisSpawnDataInput
			);
			return new ShootingTargetSpawnDataCalculator(arg);
		}
		IShootingTargetSpawnWaypointEvent[] thisSpawnEvents;
		public IShootingTargetSpawnWaypointEvent[] GetSpawnWaypointEvents(){
			return thisSpawnEvents;
		}
		public void Despawn(){
			DeactivateSpawnedTargets();
		}
		void DeactivateSpawnedTargets(){
			IShootingTargetSpawnWaypointEvent[] spawnEvents = thisSpawnEvents;
			if(spawnEvents != null){
				foreach(IShootingTargetSpawnWaypointEvent spawnEvent in spawnEvents){
					IShootingTargetSpawnPoint spawnPoint = spawnEvent.GetSpawnPoint();
					IShootingTarget spawnedTarget = spawnPoint.GetSpawnedTarget();
					if(spawnedTarget != null)
						spawnedTarget.Deactivate();
				}	
			}
		}
		IPlayerCharacterWaypointsFollower thisPlayerCharacterWaypointsFollower;
		public void SetPlayerCharacterWaypointsFollower(IPlayerCharacterWaypointsFollower follower){
			thisPlayerCharacterWaypointsFollower = follower;
		}
		public bool ShouldSpawnTargets(){
			return thisPlayerCharacterWaypointsFollower.ShouldSpawnTargets();
		}
		/*  */
			public new interface IConstArg: SlickBowShootingSceneObject.IConstArg{
				int spawnValueLimit{get;}
			}
			public new class ConstArg: SlickBowShootingSceneObject.ConstArg, IConstArg{
				public ConstArg(
					ILevelSectionShootingTargetSpawnerAdaptor adaptor,
					int spawnValueLimit
				): base(
					adaptor
				){
					thisSpawnValueLimit = spawnValueLimit;
				}
				readonly int thisSpawnValueLimit;
				public int spawnValueLimit{get{return thisSpawnValueLimit;}}
			}
	}
	public interface ISpawnPointEventPointPair{
		IShootingTargetSpawnPoint GetSpawnPoint();
		float GetEventPoint();
	}
	public struct SpawnPointEventPointPair: ISpawnPointEventPointPair{
		public SpawnPointEventPointPair(
			IShootingTargetSpawnPoint spawnPoint,
			float eventPoint
		){
			thisSpawnPoint = spawnPoint;
			thisEventPoint = eventPoint;
		}
		readonly IShootingTargetSpawnPoint thisSpawnPoint;
		public IShootingTargetSpawnPoint GetSpawnPoint(){
			return thisSpawnPoint;
		}
		readonly float thisEventPoint;
		public float GetEventPoint(){
			return thisEventPoint;
		}
	}
	[System.Serializable]
	public struct SpawnPointAdaptorEventPointPair{
		public AbsShootingTargetSpawnPointAdaptor adaptor;
		public float eventPoint;
	}
	
}

