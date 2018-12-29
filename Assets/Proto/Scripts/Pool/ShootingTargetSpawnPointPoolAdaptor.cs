// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityBase;

// namespace AppleShooterProto{
// 	public interface IShootingTargetSpawnPointPoolAdaptor: ISceneObjectPoolAdaptor<IStaticTargetSpawnPointAdaptor>{

// 	}
// 	public class ShootingTargetSpawnPointPoolAdaptor : SceneObjectPoolAdaptor<IStaticTargetSpawnPointAdaptor>, IShootingTargetSpawnPointPoolAdaptor {
// 		protected override Dictionary<IStaticTargetSpawnPointAdaptor, float> CreateRelativeProbabilityTable(
// 			List<AdaptorRelativeProbPair> pairs
// 		){
// 			Dictionary<IStaticTargetSpawnPointAdaptor, float> result = new Dictionary<IStaticTargetSpawnPointAdaptor, float>();
// 			foreach(AdaptorRelativeProbPair pair in pairs){
// 				IStaticTargetSpawnPointAdaptor typedAdaptor = (IStaticTargetSpawnPointAdaptor)pair.adaptor;
// 				// IStaticTargetSpawnPointAdaptor spawnPoint = typedAdaptor.GetStaticTargetSpawnPoint();
// 				result.Add(typedAdaptor, pair.relativeProb);
// 			}
// 			return result;
// 		}
// 	}
// }
