using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface IGlidingTargetWaypointCurvePoolAdaptor: ISceneObjectPoolAdaptor<IGlidingTargetWaypointCurve>{}
	public class GlidingTargetWaypointCurvePoolAdaptor : SceneObjectPoolAdaptor<IGlidingTargetWaypointCurve>, IGlidingTargetWaypointCurvePoolAdaptor{

		protected override Dictionary<IGlidingTargetWaypointCurve, float> CreateRelativeProbabilityTable(
			List<AdaptorRelativeProbPair> pairs
		){
			Dictionary<IGlidingTargetWaypointCurve, float> result = new Dictionary<IGlidingTargetWaypointCurve, float>();
			foreach(AdaptorRelativeProbPair pair in pairs){
				IGlidingTargetWaypointCurveAdaptor typedAdaptor = (IGlidingTargetWaypointCurveAdaptor)pair.adaptor;
				IGlidingTargetWaypointCurve curve = typedAdaptor.GetGlidingTargetWaypointCurve();
				result.Add(curve, pair.relativeProb);
			}
			return result;
		}
	}
}
