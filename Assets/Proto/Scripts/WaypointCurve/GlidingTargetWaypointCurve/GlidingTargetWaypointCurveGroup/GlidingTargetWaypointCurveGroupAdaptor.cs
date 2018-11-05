using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetWaypointCurveGroupAdaptor: IMonoBehaviourAdaptor{
		IGlidingTargetWaypointCurveGroup GetCurveGroup();
	}
	public class GlidingTargetWaypointCurveGroupAdaptor: MonoBehaviourAdaptor, IGlidingTargetWaypointCurveGroupAdaptor{
		IGlidingTargetWaypointCurveGroup thisGroup;
		public IGlidingTargetWaypointCurveGroup GetCurveGroup(){
			return thisGroup;
		}
		public override void SetUp(){
			thisGroup = CreateCurveGroup();
			thisCurveAdaptors = CollectCurveAdaptor();
		}
		IGlidingTargetWaypointCurveGroup CreateCurveGroup(){
			GlidingTargetWaypointCurveGroup.IConstArg arg = new GlidingTargetWaypointCurveGroup.ConstArg(
				this
			);
			return new GlidingTargetWaypointCurveGroup(arg);
		}
		IGlidingTargetWaypointCurveAdaptor[] thisCurveAdaptors;
		IGlidingTargetWaypointCurveAdaptor[] CollectCurveAdaptor(){
			Component[] comps =  this.transform.GetComponentsInChildren<Component>();
			List<IGlidingTargetWaypointCurveAdaptor> adaptorsList = new List<IGlidingTargetWaypointCurveAdaptor>();
			foreach(Component comp in comps){
				if(comp is IGlidingTargetWaypointCurveAdaptor)
					adaptorsList.Add((IGlidingTargetWaypointCurveAdaptor)comp);
			}
			return adaptorsList.ToArray();
		}
		public override void SetUpReference(){
			IGlidingTargetWaypointCurve[] curves = GetCurves();
			thisGroup.SetCurves(curves);
		}
		IGlidingTargetWaypointCurve[] GetCurves(){
			List<IGlidingTargetWaypointCurve> resultList = new List<IGlidingTargetWaypointCurve>();
			int index = 0;
			foreach(IGlidingTargetWaypointCurveAdaptor adaptor in thisCurveAdaptors){
				IGlidingTargetWaypointCurve curve = (IGlidingTargetWaypointCurve)adaptor.GetWaypointCurve();
				curve.SetIndex(index);
				resultList.Add(curve);
				index ++;
			}
			return resultList.ToArray();
		}
	}
}

