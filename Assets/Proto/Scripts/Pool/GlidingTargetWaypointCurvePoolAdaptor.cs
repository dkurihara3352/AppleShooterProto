// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityBase;
// namespace SlickBowShooting{
// 	public interface IGlidingTargetWaypointCurvePoolAdaptor: ISceneObjectPoolAdaptor<IGlidingTargetWaypointCurveAdaptor>{}
// 	public class GlidingTargetWaypointCurvePoolAdaptor : SceneObjectPoolAdaptor<IGlidingTargetWaypointCurveAdaptor>, IGlidingTargetWaypointCurvePoolAdaptor{

// 		protected override Dictionary<IGlidingTargetWaypointCurveAdaptor, float> CreateRelativeProbabilityTable(
// 			List<AdaptorRelativeProbPair> pairs
// 		){
// 			Dictionary<IGlidingTargetWaypointCurveAdaptor, float> result = new Dictionary<IGlidingTargetWaypointCurveAdaptor, float>();
// 			foreach(AdaptorRelativeProbPair pair in pairs){
// 				IGlidingTargetWaypointCurveAdaptor typedAdaptor = (IGlidingTargetWaypointCurveAdaptor)pair.adaptor;
// 				// IGlidingTargetWaypointCurve curve = typedAdaptor.GetGlidingTargetWaypointCurve();
// 				result.Add(typedAdaptor, pair.relativeProb);
// 			}
// 			return result;
// 		}
// 	}
// }
