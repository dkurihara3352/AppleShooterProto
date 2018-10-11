using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface ITestTargetSpawnManager: ITargetSpawnManager{
		void SetShootingTargetSpawnPoints(
			IShootingTargetSpawnPoint[] points
		);
		void SetTestShootingTargetReserve(ITestShootingTargetReserve reserve);
	}
	public class TestTargetSpawnManager: ITestTargetSpawnManager{
		public TestTargetSpawnManager(IConstArg arg){
			thisSpawnCount = arg.spawnCount;
			thisAdaptor = arg.adaptor;

		}
		readonly int thisSpawnCount;
		readonly ITestTargetSpawnManagerAdaptor thisAdaptor;

		IShootingTargetSpawnPoint[] thisSpawnPoints;
		public void SetShootingTargetSpawnPoints(IShootingTargetSpawnPoint[] points){
			thisSpawnPoints = points;
		}
		ITestShootingTargetReserve thisTargetReserve;
		public void SetTestShootingTargetReserve(ITestShootingTargetReserve reserve){
			thisTargetReserve = reserve;
		}
		ITestShootingTarget[] thisSpawnedTargets;
		/*  */
		public void Spawn(){
			SpawnStaticTargetsRandomely();
		}
		void SpawnStaticTargetsRandomely(){

			int[] spawnPointIndices = CalculateSpawnPointIndices();
			thisSpawnedTargets = new ITestShootingTarget[thisSpawnCount];
			int count = 0;
			foreach(int spawnPointIndex in spawnPointIndices){
				thisSpawnedTargets[count ++] = SpawnStaticTestTargetAt(spawnPointIndex);
			}
		}
		int[] thisIndices;
		int[] CalculateSpawnPointIndices(){
			int spawnCount = thisSpawnCount;
			int spawnPointCandidateCount = thisSpawnPoints.Length;
			int[] randomIndices = DKUtility.Calculator.GetRandomIntegers(
				spawnCount,
				spawnPointCandidateCount -1
			);
			thisIndices = randomIndices;
			return randomIndices;
		}
		public int[] GetSpawnPointIndices(){
			return thisIndices;
		}
		ITestShootingTarget SpawnStaticTestTargetAt(int spawnPointIndex){
			IShootingTargetSpawnPoint spawnPoint = thisSpawnPoints[spawnPointIndex];
			ITestShootingTarget target = thisTargetReserve.Unreserve();
			MakeTargetChildOfSpawnPoint(
				target,
				spawnPoint
			);
			target.SetPosition(spawnPoint.GetPosition());
			target.SetRotation(spawnPoint.GetRotation());
			return target;
		}
		void MakeTargetChildOfSpawnPoint(
			ITestShootingTarget target,
			IShootingTargetSpawnPoint spawnPoint
		){
			target.SetParent(spawnPoint.GetTransform());
		}
		public void Despawn(){
			foreach(ITestShootingTarget target in thisSpawnedTargets){
				thisTargetReserve.Reserve(target);
			}
		}
		/*  */
		public interface IConstArg{
			int spawnCount{get;}
			ITestTargetSpawnManagerAdaptor adaptor{get;}
		}
		public struct ConstArg: IConstArg{
			public ConstArg(
				int spawnCount,
				ITestTargetSpawnManagerAdaptor adaptor
			){
				thisSpawnCount  = spawnCount;
				thisAdaptor = adaptor;
			}
			readonly int thisSpawnCount;
			public int spawnCount{get{return thisSpawnCount;}}
			readonly ITestTargetSpawnManagerAdaptor thisAdaptor;
			public ITestTargetSpawnManagerAdaptor adaptor{get{return thisAdaptor;}}
		}
	}
}
