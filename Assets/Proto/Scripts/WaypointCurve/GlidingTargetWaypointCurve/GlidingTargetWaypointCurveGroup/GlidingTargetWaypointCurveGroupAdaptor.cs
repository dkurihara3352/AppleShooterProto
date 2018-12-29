using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetWaypointCurveGroupAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IGlidingTargetWaypointCurveGroup GetCurveGroup();
		IGlidingTargetWaypointCurveAdaptor[] GetCurveAdaptors();
	}
	public class GlidingTargetWaypointCurveGroupAdaptor: AppleShooterMonoBehaviourAdaptor, IGlidingTargetWaypointCurveGroupAdaptor{
		IGlidingTargetWaypointCurveGroup thisGroup;
		public IGlidingTargetWaypointCurveGroup GetCurveGroup(){
			return thisGroup;
		}
		public override void SetUp(){
			thisGroup = CreateCurveGroup();
		}
		IGlidingTargetWaypointCurveGroup CreateCurveGroup(){
			GlidingTargetWaypointCurveGroup.IConstArg arg = new GlidingTargetWaypointCurveGroup.ConstArg(
				this
			);
			return new GlidingTargetWaypointCurveGroup(arg);
		}
		public IGlidingTargetWaypointCurveAdaptor[] GetCurveAdaptors(){
			return CollectCurveAdaptors();
		}
		IGlidingTargetWaypointCurveAdaptor[] CollectCurveAdaptors(){
			Component[] comps =  this.transform.GetComponentsInChildren<Component>();
			List<IGlidingTargetWaypointCurveAdaptor> adaptorsList = new List<IGlidingTargetWaypointCurveAdaptor>();
			foreach(Component comp in comps){
				if(comp is IGlidingTargetWaypointCurveAdaptor)
					adaptorsList.Add((IGlidingTargetWaypointCurveAdaptor)comp);
			}
			return adaptorsList.ToArray();
		}
	}
}

