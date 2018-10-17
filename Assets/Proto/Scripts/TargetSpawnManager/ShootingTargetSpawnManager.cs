using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnManager{
		void SetShootingTargetSpawnPoints(
			IShootingTargetSpawnPoint[] points
		);
		void SetTestShootingTargetReserve(ITestShootingTargetReserve reserve);
		void SetFlyingTargets(List<IFlyingTarget> flyingTargets);

		void Spawn();
		void Despawn();
		int[] GetSpawnPointIndices();
		ITestShootingTarget[] GetSpawnedTestShootingTargets();
	}
	public class ShootingTargetSpawnManager: IShootingTargetSpawnManager{
		public ShootingTargetSpawnManager(IConstArg arg){
			thisSpawnCount = arg.spawnCount;
			thisAdaptor = arg.adaptor;

		}
		readonly int thisSpawnCount;
		readonly IShootingTargetSpawnManagerAdaptor thisAdaptor;

		IShootingTargetSpawnPoint[] thisSpawnPoints;
		public void SetShootingTargetSpawnPoints(IShootingTargetSpawnPoint[] points){
			thisSpawnPoints = points;
		}
		ITestShootingTargetReserve thisTargetReserve;
		public void SetTestShootingTargetReserve(ITestShootingTargetReserve reserve){
			thisTargetReserve = reserve;
		}
		/*  */
		public void Spawn(){
			SpawnStaticTargetsRandomely();
		}
		void SpawnStaticTargetsRandomely(){

			int[] spawnPointIndices = CalculateSpawnPointIndices();
			foreach(int spawnPointIndex in spawnPointIndices){
				SpawnStaticTargetAt(spawnPointIndex);
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
		ITestShootingTarget SpawnStaticTargetAt(int spawnPointIndex){
			IShootingTargetSpawnPoint spawnPoint = thisSpawnPoints[spawnPointIndex];
			ITestShootingTarget target = thisTargetReserve.Unreserve();
			spawnPoint.SetTarget(target);
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
			// DespawnLandedArrowsOnAllSpawnedShootingTargets();
			DespawnFlyingTargets();
			foreach(IShootingTargetSpawnPoint point in thisSpawnPoints){
				ITestShootingTarget spawnedTarget = point.GetSpawnedTarget() as ITestShootingTarget;
				if(spawnedTarget != null){
					point.SetTarget(null);
					thisTargetReserve.Reserve(spawnedTarget);
				}
			}
		}
		// void DespawnLandedArrowsOnAllSpawnedShootingTargets(){
		// 	ITestShootingTarget[] targetsToDespawn = GetSpawnedTestShootingTargets();
		// 	foreach(ITestShootingTarget target in targetsToDespawn){
		// 		Debug.Log(
		// 			"target " + target.GetIndex().ToString() +
		// 			" is reserved"
		// 		);
		// 		target.ReserveAllLandedArrow();
		// 	}
		// }
		public void SetFlyingTargets(List<IFlyingTarget> flyingTargets){
			thisFlyingTargets = flyingTargets;
		}
		List<IFlyingTarget> thisFlyingTargets;
		void DespawnFlyingTargets(){
			foreach(IFlyingTarget flyingTarget in thisFlyingTargets)
				flyingTarget.ResetTarget();
		}
		public ITestShootingTarget[] GetSpawnedTestShootingTargets(){
			List<ITestShootingTarget> resultList = new List<ITestShootingTarget>();
			foreach(IShootingTargetSpawnPoint point in thisSpawnPoints){
				ITestShootingTarget spawnedTarget = point.GetSpawnedTarget() as ITestShootingTarget;
				if(spawnedTarget != null)
					resultList.Add(spawnedTarget);
			}
			foreach(IFlyingTarget flyingTarget in thisFlyingTargets){
				resultList.Add(flyingTarget);
			}
			return resultList.ToArray();
		}
		/*  */
		public interface IConstArg{
			int spawnCount{get;}
			IShootingTargetSpawnManagerAdaptor adaptor{get;}
		}
		public struct ConstArg: IConstArg{
			public ConstArg(
				int spawnCount,
				IShootingTargetSpawnManagerAdaptor adaptor
			){
				thisSpawnCount  = spawnCount;
				thisAdaptor = adaptor;
			}
			readonly int thisSpawnCount;
			public int spawnCount{get{return thisSpawnCount;}}
			readonly IShootingTargetSpawnManagerAdaptor thisAdaptor;
			public IShootingTargetSpawnManagerAdaptor adaptor{get{return thisAdaptor;}}
		}
	}
}
