using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnPointPoolAdaptor: ISceneObjectPoolAdaptor<IShootingTargetSpawnPoint>{

	}
	public class ShootingTargetSpawnPointPoolAdaptor : SceneObjectPoolAdaptor<IShootingTargetSpawnPoint>, IShootingTargetSpawnPointPoolAdaptor {
		protected override Dictionary<IShootingTargetSpawnPoint, float> CreateRelativeProbabilityTable(
			List<AdaptorRelativeProbPair> pairs
		){
			Dictionary<IShootingTargetSpawnPoint, float> result = new Dictionary<IShootingTargetSpawnPoint, float>();
			foreach(AdaptorRelativeProbPair pair in pairs){
				IShootingTargetSpawnPointAdaptor typedAdaptor = (IShootingTargetSpawnPointAdaptor)pair.adaptor;
				IShootingTargetSpawnPoint spawnPoint = typedAdaptor.GetShootingTargetSpawnPoint();
				result.Add(spawnPoint, pair.relativeProb);
			}
			return result;
		}
	}
}
