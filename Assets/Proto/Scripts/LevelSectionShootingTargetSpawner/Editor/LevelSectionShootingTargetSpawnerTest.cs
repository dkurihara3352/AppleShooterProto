// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using NUnit.Framework;
// using AppleShooterProto;
// using NSubstitute;

// [TestFixture, Category("AppleShooterProto")]
// public class LevelSectionShootingTargetSpawnerTest {

// 	[Test]
// 	public void SetUpSpawnWaypointEvents_Demo(){
// 		float spawnValueLimit = 0f;
// 		TestSpawner.IConstArg arg = new TestSpawner.ConstArg(
// 			Substitute.For<ILevelSectionShootingTargetSpawnerAdaptor>(),
// 			spawnValueLimit
// 		);
// 		TestSpawner spawner = new TestSpawner(arg);
// 		float[] eventPoints = new float[]{
// 			.1f, .2f, .5f, .7f, .9f
// 		};
// 		ISpawnPointEventPointPair[] pairs = CreatePairs(eventPoints);
// 		int numToCreate = 5;

// 		spawner.SetCalculatorFields(
// 			numToCreate,
// 			pairs
// 		);

// 		spawner.SetUpSpawnWaypointEvents();

// 		IShootingTargetSpawnWaypointEvent[] events = spawner.GetSpawnWaypointEvents();
// 		Debug.Log(
// 			DKUtility.DebugHelper.BlueString(
// 				"eventPoints.Length: " + eventPoints.Length.ToString() + ", " +
// 				"numToCreate: " + numToCreate.ToString()
// 			)
// 		);
// 		foreach(IShootingTargetSpawnWaypointEvent ev in events){
// 			IShootingTargetSpawnPoint spawnPoint = ev.GetSpawnPoint();
// 			float eventPoint = ev.GetEventPoint();
// 			Debug.Log(
// 				"spawnPoint# " + spawnPoint.GetIndex().ToString() + ", " + 
// 				"eventPoint: " + eventPoint.ToString()
// 			);
// 		}
// 	}
// 	public class TestSpawner: LevelSectionShootingTargetSpawner{
// 		public TestSpawner(IConstArg arg): base(arg){}
// 		public void SetCalculatorFields(
// 			int staticNumToCreate,
// 			ISpawnPointEventPointPair[] pairs
// 		){
// 			thisStaticNumToCreate = staticNumToCreate;
// 			thisPairs = pairs;
// 		}
// 		int thisStaticNumToCreate;
// 		ISpawnPointEventPointPair[] thisPairs;
// 		protected override IShootingTargetSpawnDataCalculator CreateCalculator(){
// 			TargetSpawnData data = CreateReturnedSpawnData(
// 				thisStaticNumToCreate,
// 				thisPairs
// 			);
// 			IShootingTargetSpawnDataCalculator mockCalculator = Substitute.For<IShootingTargetSpawnDataCalculator>();
// 			mockCalculator.CalculateTargetSpawnDataByTargetType().Returns(data);
// 			return mockCalculator;
// 		}	
// 		TargetSpawnData CreateReturnedSpawnData(
// 			int staticNumToCreate,
// 			ISpawnPointEventPointPair[] staticPairs
// 		){
// 			TargetSpawnData.Entry staticEntry = new TargetSpawnData.Entry(
// 				TargetType.Static,
// 				staticNumToCreate,
// 				Substitute.For<IStaticShootingTargetReserve>(),
// 				staticPairs
// 			);
// 			return new TargetSpawnData(new TargetSpawnData.Entry[]{staticEntry});
// 		}
// 	}
// 	ISpawnPointEventPointPair[] CreatePairs(float[] eventPoints){
// 		List<ISpawnPointEventPointPair> resultList = new List<ISpawnPointEventPointPair>();
// 		int index = 0;
// 		foreach(float eventPoint in eventPoints){
// 			IShootingTargetSpawnPoint spawnPoint = Substitute.For<IShootingTargetSpawnPoint>();
// 			spawnPoint.GetIndex().Returns(index++);
// 			ISpawnPointEventPointPair pair = new SpawnPointEventPointPair(
// 				spawnPoint,
// 				eventPoint
// 			);
// 			resultList.Add(pair);
// 		}
// 		return resultList.ToArray();
// 	}
// }
