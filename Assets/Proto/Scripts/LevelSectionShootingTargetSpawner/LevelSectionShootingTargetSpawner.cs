using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface ILevelSectionShootingTargetSpawner: ISceneObject{
		void SetShootingTargetSpawnManager(IShootingTargetSpawnManager manager);
		void SetLevelSectionTargetSpawnDataInput(
			TargetSpawnDataInput[] input
		);
		
		void SetUpSpawnWaypointEvents();
		IShootingTargetSpawnWaypointEvent[] GetSpawnWaypointEvents();
	}
	public class LevelSectionShootingTargetSpawner : AbsSceneObject, ILevelSectionShootingTargetSpawner {

		public LevelSectionShootingTargetSpawner(
			IConstArg arg
		): base(arg){
			thisSpawnValueLimit = arg.spawnValueLimit;
		}

		float thisSpawnValueLimit;
		TargetSpawnDataInput[] thisSpawnDataInput;
		public void SetLevelSectionTargetSpawnDataInput(TargetSpawnDataInput[] input){
			thisSpawnDataInput = input;
		}
		IShootingTargetSpawnManager thisShootingTargetSpawnManager;
		public void SetShootingTargetSpawnManager(IShootingTargetSpawnManager manager){
			thisShootingTargetSpawnManager = manager;
		}
		public void SetUpSpawnWaypointEvents(){
			IShootingTargetSpawnDataCalculator calculator = CreateCalculator();
			TargetSpawnData spawnData = calculator.CalculateTargetSpawnDataByTargetType();

			List<IShootingTargetSpawnWaypointEvent> spawnWaypointEventsList = new List<IShootingTargetSpawnWaypointEvent>();

			TargetSpawnData.Entry[] entries = spawnData.GetEntries();

			foreach(TargetSpawnData.Entry entry in entries){
			
				int spawnCount = entry.numToCreate;
				ISpawnPointEventPointPair[] pairs = entry.spawnPointEventPointPairs;

				// Debug.Log(
				// 	entry.targetType.ToString() + ": " +
				// 	"pairs.length: " + pairs.Length.ToString() + ", " +
				// 	"spawnCount: "  + spawnCount.ToString()
				// );
				int[] spawnPointIndexToSpawn = DKUtility.Calculator.GetRandomIntegers(
					count: spawnCount, 
					maxNumber: pairs.Length -1
				);
				// Debug.Log(
				// 	entry.targetType.ToString() + ": "+ 
				// 	"spawnPointIndex: " + GetIndicesString(spawnPointIndexToSpawn)
				// );

				foreach(int spawnPointIndex in spawnPointIndexToSpawn){
					ISpawnPointEventPointPair pointPair = pairs[spawnPointIndex];
					IShootingTargetSpawnPoint spawnPoint = pointPair.GetSpawnPoint();
					float eventPoint = pointPair.GetEventPoint();
					IShootingTargetReserve reserve = entry.reserve;
					ShootingTargetSpawnWaypointEvent.IConstArg eventConstArg = new ShootingTargetSpawnWaypointEvent.ConstArg(
						reserve,
						spawnPoint,
						eventPoint
					);
					IShootingTargetSpawnWaypointEvent spawnWaypointEvent = new ShootingTargetSpawnWaypointEvent(
						eventConstArg
					);

					spawnWaypointEventsList.Add(spawnWaypointEvent);

				}
			}
			thisSpawnEvents = spawnWaypointEventsList.ToArray();
		}
		string GetIndicesString(int[] indices){
			string result =  "";
			foreach(int i in indices)
				result += i.ToString() + ", " ;
			return result;
		}
		protected virtual IShootingTargetSpawnDataCalculator CreateCalculator(){
			ShootingTargetSpawnDataCalculator.IConstArg arg = new ShootingTargetSpawnDataCalculator.ConstArg(
				thisSpawnValueLimit,
				thisSpawnDataInput,
				thisShootingTargetSpawnManager
			);
			return new ShootingTargetSpawnDataCalculator(arg);
		}
		IShootingTargetSpawnWaypointEvent[] thisSpawnEvents;
		public IShootingTargetSpawnWaypointEvent[] GetSpawnWaypointEvents(){
			return thisSpawnEvents;
		}
		/*  */
			public new interface IConstArg: AbsSceneObject.IConstArg{
				float spawnValueLimit{get;}
			}
			public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
				public ConstArg(
					ILevelSectionShootingTargetSpawnerAdaptor adaptor,
					float spawnValueLimit
				): base(
					adaptor
				){
					thisSpawnValueLimit = spawnValueLimit;
				}
				readonly float thisSpawnValueLimit;
				public float spawnValueLimit{get{return thisSpawnValueLimit;}}
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

