using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPCWaypointCurve: IWaypointCurve{
	/* Targets management */
		void SetTargetSpawnManager(IShootingTargetSpawnManager targetSpawnManager);
		void SpawnTargets();
		void DespawnTargets();
		int[] GetSpawnIndices();
		ITestShootingTarget[] GetSpawnedShootingTargets();
	}
	public class PCWaypointCurve: AbsWaypointCurve, IPCWaypointCurve{
		public PCWaypointCurve(IConstArg arg): base(arg){}
		public override void OnReserve(){
			DespawnTargets();
		}
		/* target Spawn */
			IShootingTargetSpawnManager thisTargetSpawnManager;
			public void SetTargetSpawnManager(IShootingTargetSpawnManager manager){
				thisTargetSpawnManager = manager;
			}
			public void SpawnTargets(){
				thisTargetSpawnManager.Spawn();
			}
			public void DespawnTargets(){
				thisTargetSpawnManager.Despawn();
			}
			public int[] GetSpawnIndices(){
				return thisTargetSpawnManager.GetSpawnPointIndices();
			}
			public ITestShootingTarget[] GetSpawnedShootingTargets(){
				return thisTargetSpawnManager.GetSpawnedTestShootingTargets();
			}
	}
}

