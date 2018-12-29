using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnPointGroupAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IShootingTargetSpawnPointGroup GetGroup();
		IShootingTargetSpawnPointAdaptor[] GetAdaptors();
	}
	public abstract class AbsShootingTargetSpawnPointGroupAdaptor : AppleShooterMonoBehaviourAdaptor, IShootingTargetSpawnPointGroupAdaptor {

		public override void SetUp(){
			thisGroup = CreateGroup();
			if(thisSpawnPointAdaptors == null)
				thisSpawnPointAdaptors = CollectSpawnPointAdaptors();
		}
		protected IShootingTargetSpawnPointGroup thisGroup;
		public IShootingTargetSpawnPointGroup GetGroup(){
			return thisGroup;
		}
		protected abstract IShootingTargetSpawnPointGroup CreateGroup();

		IShootingTargetSpawnPointAdaptor[] thisSpawnPointAdaptors;
		public IShootingTargetSpawnPointAdaptor[] GetAdaptors(){
			if(thisSpawnPointAdaptors == null)
				thisSpawnPointAdaptors = CollectSpawnPointAdaptors();
			return thisSpawnPointAdaptors;
		}
		protected abstract IShootingTargetSpawnPointAdaptor[] CollectSpawnPointAdaptors();
		public override void SetUpReference(){
			IShootingTargetSpawnPoint[] spawnPoints = CollectSpawnPoints();
			int index = 0;
			foreach(IShootingTargetSpawnPoint spawnPoint in spawnPoints)
				spawnPoint.SetIndex(index++);
			thisGroup.SetSpawnPoints(spawnPoints);
		}
		public IShootingTargetSpawnPoint[] CollectSpawnPoints(){
			List<IShootingTargetSpawnPoint> resultList = new List<IShootingTargetSpawnPoint>();
			foreach(IShootingTargetSpawnPointAdaptor adaptor in thisSpawnPointAdaptors)
				resultList.Add(adaptor.GetSpawnPoint());
			return resultList.ToArray();
		}
		protected T[] CollectTypedSpawnPointAdaptorFromChildren<T>() where T: class, IShootingTargetSpawnPointAdaptor{
			List<T> resultList = new List<T>();
			Component[] comps = this.transform.GetComponentsInChildren<Component>();
			foreach(Component comp in comps){
				if(comp is T){
					resultList.Add(comp as T);
				}
			}
			return resultList.ToArray();
		}
	}
}
