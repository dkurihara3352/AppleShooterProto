using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IStaticTargetSpawnPointGroupAdaptor: IMonoBehaviourAdaptor{
		IStaticTargetSpawnPointGroup GetSpawnPointGroup();
	}
	public class StaticTargetSpawnPointGroupAdaptor: MonoBehaviourAdaptor, IStaticTargetSpawnPointGroupAdaptor{
		public override void SetUp(){
			thisGroup = CreateSpawnPointGroup();
			thisSpawnPointAdaptors = CollectSpawnPointAdaptors();
		}
		IStaticTargetSpawnPointGroup thisGroup;
		public IStaticTargetSpawnPointGroup GetSpawnPointGroup(){
			return thisGroup;
		}
		IStaticTargetSpawnPointGroup CreateSpawnPointGroup(){
			StaticTargetSpawnPointGroup.IConstArg arg = new StaticTargetSpawnPointGroup.ConstArg(
				this
			);
			return new StaticTargetSpawnPointGroup(arg);
		}
		IStaticTargetSpawnPointAdaptor[] thisSpawnPointAdaptors;
		IStaticTargetSpawnPointAdaptor[] CollectSpawnPointAdaptors(){
			List<IStaticTargetSpawnPointAdaptor> resultList = new List<IStaticTargetSpawnPointAdaptor>();
			Component[] comps = this.transform.GetComponentsInChildren<Component>();
			foreach(Component comp in comps){
				if(comp is IStaticTargetSpawnPointAdaptor)
					resultList.Add((IStaticTargetSpawnPointAdaptor)comp);
			}	
			return resultList.ToArray();
		}
		public override void SetUpReference(){
			IStaticTargetSpawnPoint[] spawnPoints = GetSpawnPoints();
			thisGroup.SetShootingTargetSpawnPoints(spawnPoints);
		}
		IStaticTargetSpawnPoint[] GetSpawnPoints(){
			List<IStaticTargetSpawnPoint> resultList = new List<IStaticTargetSpawnPoint>();
			int index = 0;
			foreach(IStaticTargetSpawnPointAdaptor adaptor in thisSpawnPointAdaptors){
				IStaticTargetSpawnPoint point = adaptor.GetStaticTargetSpawnPoint();
				point.SetIndex(index);
				resultList.Add(point);
				index ++;
			}
			
			return resultList.ToArray();
		}
	}
}

