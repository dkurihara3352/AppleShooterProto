using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointsManagerPoolAdaptor: ISceneObjectPoolAdaptor<IWaypointCurveCycleManager>{
	}
	public class WaypointsManagerPoolAdaptor : SceneObjectPoolAdaptor<IWaypointCurveCycleManager>, IWaypointsManagerPoolAdaptor {

		protected override Dictionary<IWaypointCurveCycleManager, float> CreateRelativeProbabilityTable(List<AdaptorRelativeProbPair> pairs){
			Dictionary<IWaypointCurveCycleManager, float> result = new Dictionary<IWaypointCurveCycleManager, float>();
			foreach(AdaptorRelativeProbPair pair in pairs){
				IWaypointCurveCycleManagerAdaptor typedAdaptor = (IWaypointCurveCycleManagerAdaptor)pair.adaptor;
				IWaypointCurveCycleManager manager = typedAdaptor.GetWaypointsManager();
				result.Add(manager, pair.relativeProb);
			}
			return result;
		}
	}
}
