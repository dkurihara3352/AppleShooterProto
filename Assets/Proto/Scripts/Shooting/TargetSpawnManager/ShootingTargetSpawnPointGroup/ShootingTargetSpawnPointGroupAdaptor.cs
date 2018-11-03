using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnPointGroupAdaptor: IMonoBehaviourAdaptor{
		IShootingTargetSpawnPointGroup GetShootingTargetSpawnPointGroup();
	}
	public class ShootingTargetSpawnPointGroupAdaptor: MonoBehaviourAdaptor, IShootingTargetSpawnPointGroupAdaptor{
		public override void SetUp(){
			thisGroup = CreateShootingTargetSpawnPointGroup();
			thisSpawnPointAdaptors = CollectSpawnPointAdaptors();
		}
		IShootingTargetSpawnPointGroup thisGroup;
		public IShootingTargetSpawnPointGroup GetShootingTargetSpawnPointGroup(){
			return thisGroup;
		}
		IShootingTargetSpawnPointGroup CreateShootingTargetSpawnPointGroup(){
			ShootingTargetSpawnPointGroup.IConstArg arg = new ShootingTargetSpawnPointGroup.ConstArg(
				this
			);
			return new ShootingTargetSpawnPointGroup(arg);
		}
		IShootingTargetSpawnPointAdaptor[] thisSpawnPointAdaptors;
		IShootingTargetSpawnPointAdaptor[] CollectSpawnPointAdaptors(){
			List<IShootingTargetSpawnPointAdaptor> resultList = new List<IShootingTargetSpawnPointAdaptor>();
			Component[] comps = this.transform.GetComponentsInChildren<Component>();
			foreach(Component comp in comps){
				if(comp is IShootingTargetSpawnPointAdaptor)
					resultList.Add((IShootingTargetSpawnPointAdaptor)comp);
			}	
			return resultList.ToArray();
		}
		public override void SetUpReference(){
			IShootingTargetSpawnPoint[] spawnPoints = GetSpawnPoints();
			thisGroup.SetShootingTargetSpawnPoints(spawnPoints);
		}
		IShootingTargetSpawnPoint[] GetSpawnPoints(){
			List<IShootingTargetSpawnPoint> resultList = new List<IShootingTargetSpawnPoint>();
			int index = 0;
			foreach(IShootingTargetSpawnPointAdaptor adaptor in thisSpawnPointAdaptors){
				IShootingTargetSpawnPoint point = adaptor.GetShootingTargetSpawnPoint();
				point.SetIndex(index);
				resultList.Add(point);
				index ++;
			}
			
			return resultList.ToArray();
		}
	}
}

