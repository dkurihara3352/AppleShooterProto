using System.Collections;
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
		public TestShootingTargetReserveAdaptor targetReserveAdaptor;
		public override void SetUp(){
			ShootingTargetSpawnManager.IConstArg arg = new ShootingTargetSpawnManager.ConstArg(
				spawnCount,
				this
			);
			thisSpawnManager = new ShootingTargetSpawnManager(arg);
		}
		public override void SetUpReference(){
			IShootingTargetSpawnPoint[] spawnPoints = CollectSpawnPoints();
			ITestShootingTargetReserve reserve = targetReserveAdaptor.GetTestShootingTargetReserve();
			List<IFlyingTarget> flyingTargets = CollectFlyingTargets();
			thisSpawnManager.SetShootingTargetSpawnPoints(spawnPoints);
			thisSpawnManager.SetTestShootingTargetReserve(reserve);
			thisSpawnManager.SetFlyingTargets(flyingTargets);
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
	}
}
