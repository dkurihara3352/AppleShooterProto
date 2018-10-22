﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnManagerAdaptor: IMonoBehaviourAdaptor{
		IShootingTargetSpawnManager GetShootingTargetSpawnManager();
	}
	public class ShootingTargetSpawnManagerAdaptor : MonoBehaviourAdaptor, IShootingTargetSpawnManagerAdaptor{
		IShootingTargetSpawnManager thisSpawnManager;
		public IShootingTargetSpawnManager GetShootingTargetSpawnManager(){
			return thisSpawnManager;
		}
		public int spawnCount;
		public Transform spawnPointsParent;
		public StaticShootingTargetReserveAdaptor targetReserveAdaptor;
		public override void SetUp(){
			ShootingTargetSpawnManager.IConstArg arg = new ShootingTargetSpawnManager.ConstArg(
				spawnCount,
				this
			);
			thisSpawnManager = new ShootingTargetSpawnManager(arg);
		}
		public override void SetUpReference(){
			IShootingTargetSpawnPoint[] spawnPoints = CollectSpawnPoints();
			IStaticShootingTargetReserve reserve = targetReserveAdaptor.GetStaticShootingTargetReserve();
			List<IFlyingTarget> flyingTargets = CollectFlyingTargets();
			IGlidingTarget[] glidingTargets = CollectGlidingTargets();
			thisSpawnManager.SetShootingTargetSpawnPoints(spawnPoints);
			thisSpawnManager.SetTestShootingTargetReserve(reserve);
			thisSpawnManager.SetFlyingTargets(flyingTargets);
			thisSpawnManager.SetGlidingTargets(glidingTargets);
		}
		IShootingTargetSpawnPoint[] CollectSpawnPoints(){
			List<IShootingTargetSpawnPoint> resultList = new List<IShootingTargetSpawnPoint>();
			Component[] components = spawnPointsParent.GetComponentsInChildren(typeof(IShootingTargetSpawnPointAdaptor));
			foreach(Component comp in components){
				if(comp is IShootingTargetSpawnPointAdaptor){
					IShootingTargetSpawnPointAdaptor adaptor = (IShootingTargetSpawnPointAdaptor)comp;
					resultList.Add(
						adaptor.GetShootingTargetSpawnPoint()
					);
				}
			}
			return resultList.ToArray();
		}
		public List<FlyingTargetAdaptor> flyingTargetAdaptors = new List<FlyingTargetAdaptor>();
		List<IFlyingTarget> CollectFlyingTargets(){
			List<IFlyingTarget> result = new List<IFlyingTarget>();
			if(flyingTargetAdaptors != null && flyingTargetAdaptors.Count > 0){
				foreach(IFlyingTargetAdaptor adaptor in flyingTargetAdaptors){
					if(adaptor != null)
						result.Add((IFlyingTarget)adaptor.GetShootingTarget());
				}
			}
			return result;
		}
		public List<GlidingTargetAdaptor> glidingTargetAdaptors =  new List<GlidingTargetAdaptor>();
		IGlidingTarget[] CollectGlidingTargets(){
			List<IGlidingTarget> result = new List<IGlidingTarget>();
			foreach(GlidingTargetAdaptor adaptor in glidingTargetAdaptors){
				if(adaptor != null)
					result.Add((IGlidingTarget)adaptor.GetShootingTarget());
			}
			return result.ToArray();
		}
	}
}
