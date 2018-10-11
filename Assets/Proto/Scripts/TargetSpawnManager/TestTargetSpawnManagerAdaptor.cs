using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ITestTargetSpawnManagerAdaptor: IMonoBehaviourAdaptor{
		ITestTargetSpawnManager GetTestTargetSpawnManager();
	}
	public class TestTargetSpawnManagerAdaptor : MonoBehaviourAdaptor, ITestTargetSpawnManagerAdaptor{
		ITestTargetSpawnManager thisSpawnManager;
		public ITestTargetSpawnManager GetTestTargetSpawnManager(){
			return thisSpawnManager;
		}
		public int spawnCount;
		public Transform spawnPointsParent;
		public TestShootingTargetReserveAdaptor targetReserveAdaptor;
		public override void SetUp(){
			TestTargetSpawnManager.IConstArg arg = new TestTargetSpawnManager.ConstArg(
				spawnCount,
				this
			);
			thisSpawnManager = new TestTargetSpawnManager(arg);
		}
		public override void SetUpReference(){
			IShootingTargetSpawnPoint[] spawnPoints = CollectSpawnPoints();
			ITestShootingTargetReserve reserve = targetReserveAdaptor.GetTestShootingTargetReserve();
			thisSpawnManager.SetShootingTargetSpawnPoints(spawnPoints);
			thisSpawnManager.SetTestShootingTargetReserve(reserve);
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
	}
}
