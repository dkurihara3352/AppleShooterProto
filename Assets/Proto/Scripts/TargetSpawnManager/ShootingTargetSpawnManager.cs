using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnManager{
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
		IStaticShootingTarget SpawnStaticTargetAt(int spawnPointIndex){
			IShootingTargetSpawnPoint spawnPoint = thisSpawnPoints[spawnPointIndex];
			IStaticShootingTarget target = thisTargetReserve.Unreserve();
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
			IStaticShootingTarget target,
			IShootingTargetSpawnPoint spawnPoint
		){
			target.SetParent(spawnPoint.GetTransform());
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
