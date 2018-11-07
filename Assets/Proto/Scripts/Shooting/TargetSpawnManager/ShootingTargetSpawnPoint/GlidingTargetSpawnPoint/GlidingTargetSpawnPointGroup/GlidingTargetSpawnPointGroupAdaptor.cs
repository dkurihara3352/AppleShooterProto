using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetSpawnPointGroupAdaptor: IMonoBehaviourAdaptor{
		IGlidingTargetSpawnPointGroup GetGroup();
	}
	public class GlidingTargetSpawnPointGroupAdaptor : MonoBehaviourAdaptor, IGlidingTargetSpawnPointGroupAdaptor {

		IGlidingTargetSpawnPointGroup thisGroup;
		public IGlidingTargetSpawnPointGroup GetGroup(){
			return thisGroup;
		}
		public override void SetUp(){
			thisGroup = CreateGroup();
			thisSpawnPointAdaptors = CollectSpawnPointAdaptors();
		}
		IGlidingTargetSpawnPointGroup CreateGroup(){
			GlidingTargetSpawnPointGroup.IConstArg arg = new GlidingTargetSpawnPointGroup.ConstArg(
				this
			);
			return new GlidingTargetSpawnPointGroup(arg);
		}
		IGlidingTargetSpawnPointAdaptor[] thisSpawnPointAdaptors;
		IGlidingTargetSpawnPointAdaptor[] CollectSpawnPointAdaptors(){
			List<IGlidingTargetSpawnPointAdaptor> resultList = new List<IGlidingTargetSpawnPointAdaptor>();
			Component[] childComps = transform.GetComponentsInChildren<Component>();
			foreach(Component comp in childComps)
				if(comp is IGlidingTargetSpawnPointAdaptor)
					resultList.Add((IGlidingTargetSpawnPointAdaptor)comp);
			return resultList.ToArray();
		}
		public override void SetUpReference(){
			IGlidingTargetSpawnPoint[] spawnPoints = GetSpawnPoints();
			thisGroup.SetSpawnPoints(spawnPoints);
		}

		IGlidingTargetSpawnPoint[] GetSpawnPoints(){
			List<IGlidingTargetSpawnPoint> resultList = new List<IGlidingTargetSpawnPoint>();
			foreach(IGlidingTargetSpawnPointAdaptor adaptor in thisSpawnPointAdaptors)
				resultList.Add(adaptor.GetGlidingTargetSpawnPoint());
			
			return resultList.ToArray();
		}
	}
}
