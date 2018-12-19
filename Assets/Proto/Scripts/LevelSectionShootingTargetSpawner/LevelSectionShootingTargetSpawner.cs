using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface ILevelSectionShootingTargetSpawner: IAppleShooterSceneObject{
		void SetPlayerCharacterWaypointsFollower(IPlayerCharacterWaypointsFollower follower);
		void SetLevelSectionTargetSpawnDataInput(
			TargetSpawnDataInput[] input
		);
		
		void SetUpSpawnWaypointEvents();
		IShootingTargetSpawnWaypointEvent[] GetSpawnWaypointEvents();
		void Despawn();
		bool ShouldSpawnTargets();
	}
	public class LevelSectionShootingTargetSpawner : AppleShooterSceneObject, ILevelSectionShootingTargetSpawner {

		public LevelSectionShootingTargetSpawner(
			IConstArg arg
		): base(arg){
			thisSpawnValueLimit = arg.spawnValueLimit;
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
					entry.reserve,
					spawnPoints,
					spawnPointIndicesToSpawn
				);
				resultList.AddRange(eventsForEntry);

			}
			return resultList.ToArray();
		}
		IShootingTargetSpawnWaypointEvent[] CreateSpawnEventsForEntry(
			IShootingTargetReserve reserve,
			IShootingTargetSpawnPoint[] spawnPoints,
			int[] indicesToSpawn
		){
			List<IShootingTargetSpawnWaypointEvent> resultList = new List<IShootingTargetSpawnWaypointEvent>();
			foreach(int spawnPointIndex in indicesToSpawn){
				IShootingTargetSpawnPoint spawnPoint = spawnPoints[spawnPointIndex];
				float eventPoint = spawnPoint.GetEventPoint();
				ShootingTargetSpawnWaypointEvent.IConstArg eventConstArg = new ShootingTargetSpawnWaypointEvent.ConstArg(
					reserve,
					spawnPoint,
					eventPoint,
					this
				);
				IShootingTargetSpawnWaypointEvent spawnWaypointEvent = new ShootingTargetSpawnWaypointEvent(
					eventConstArg
				);

				resultList.Add(spawnWaypointEvent);
			}
			return resultList.ToArray();
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
			public new interface IConstArg: AppleShooterSceneObject.IConstArg{
				int spawnValueLimit{get;}
			}
			public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
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

