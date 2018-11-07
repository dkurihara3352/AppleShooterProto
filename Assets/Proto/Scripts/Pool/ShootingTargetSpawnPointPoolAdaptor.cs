using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnPointPoolAdaptor: ISceneObjectPoolAdaptor<IStaticTargetSpawnPoint>{

	}
	public class ShootingTargetSpawnPointPoolAdaptor : SceneObjectPoolAdaptor<IStaticTargetSpawnPoint>, IShootingTargetSpawnPointPoolAdaptor {
		protected override Dictionary<IStaticTargetSpawnPoint, float> CreateRelativeProbabilityTable(
			List<AdaptorRelativeProbPair> pairs
		){
			Dictionary<IStaticTargetSpawnPoint, float> result = new Dictionary<IStaticTargetSpawnPoint, float>();
			foreach(AdaptorRelativeProbPair pair in pairs){
				IStaticTargetSpawnPointAdaptor typedAdaptor = (IStaticTargetSpawnPointAdaptor)pair.adaptor;
				IStaticTargetSpawnPoint spawnPoint = typedAdaptor.GetStaticTargetSpawnPoint();
				result.Add(spawnPoint, pair.relativeProb);
			}
			return result;
		}
	}
}
