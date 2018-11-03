using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnManager: ISceneObject{
		void SetShootingTargetSpawnPoints(
			IShootingTargetSpawnPoint[] points
		);
		void SetTestShootingTargetReserve(IStaticShootingTargetReserve reserve);
		void SetFlyingTargets(List<IFlyingTarget> flyingTargets);
		void SetGlidingTargets(IGlidingTarget[] targets);

		void Spawn();
		void Despawn();
		int[] GetSpawnPointIndices();
		IShootingTarget[] GetSpawnedShootingTargets();
	}
	public class ShootingTargetSpawnManager: AbsSceneObject, IShootingTargetSpawnManager{
		public ShootingTargetSpawnManager(
			IConstArg arg
		): base(
			arg
		){
			thisSpawnCount = arg.spawnCount;
		}
		readonly int thisSpawnCount;

		IShootingTargetSpawnPoint[] thisSpawnPoints;
		public void SetShootingTargetSpawnPoints(IShootingTargetSpawnPoint[] points){
			thisSpawnPoints = points;
		}
		IStaticShootingTargetReserve thisTargetReserve;
		public void SetTestShootingTargetReserve(IStaticShootingTargetReserve reserve){
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
		void SpawnStaticTargetAt(int spawnPointIndex){
			IShootingTargetSpawnPoint spawnPoint = thisSpawnPoints[spawnPointIndex];
			thisTargetReserve.ActivateStaticShootingTargetAt(spawnPoint);

		}
		public void Despawn(){
			DeactivateFlyingTargets();
			DeactiveteGlidingTargets();
			foreach(IShootingTargetSpawnPoint point in thisSpawnPoints){
				IStaticShootingTarget spawnedTarget = point.GetSpawnedTarget() as IStaticShootingTarget;
				if(spawnedTarget != null){
					point.SetTarget(null);
					thisTargetReserve.Reserve(spawnedTarget);
				}
			}
		}
		/* FlyingTargets */
			public void SetFlyingTargets(List<IFlyingTarget> flyingTargets){
				thisFlyingTargets = flyingTargets;
			}
			List<IFlyingTarget> thisFlyingTargets;
			void DeactivateFlyingTargets(){
				foreach(IFlyingTarget flyingTarget in thisFlyingTargets)
					flyingTarget.Deactivate();
			}
		/* GlidingTargets */
			IGlidingTarget[] thisGlidingTargets;
			public void SetGlidingTargets(IGlidingTarget[] targets){
				thisGlidingTargets = targets;
			}
			void DeactiveteGlidingTargets(){
				foreach(IGlidingTarget target in thisGlidingTargets)
					target.Deactivate();
			}
		/*  */
		public IShootingTarget[] GetSpawnedShootingTargets(){
			List<IShootingTarget> resultList = new List<IShootingTarget>();
			foreach(IShootingTargetSpawnPoint point in thisSpawnPoints){
				IStaticShootingTarget spawnedTarget = point.GetSpawnedTarget() as IStaticShootingTarget;
				if(spawnedTarget != null)
					resultList.Add(spawnedTarget);
			}
			foreach(IFlyingTarget flyingTarget in thisFlyingTargets){
				resultList.Add(flyingTarget);
			}
			foreach(IGlidingTarget glidingTarget in thisGlidingTargets){
				resultList.Add(glidingTarget);
			}
			return resultList.ToArray();
		}
		/*  */
		public new interface IConstArg: AbsSceneObject.IConstArg{
			int spawnCount{get;}
		}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				int spawnCount,
				IShootingTargetSpawnManagerAdaptor adaptor
			): base(
				adaptor
			){
				thisSpawnCount  = spawnCount;
			}
			readonly int thisSpawnCount;
			public int spawnCount{get{return thisSpawnCount;}}
		}
	}
}
