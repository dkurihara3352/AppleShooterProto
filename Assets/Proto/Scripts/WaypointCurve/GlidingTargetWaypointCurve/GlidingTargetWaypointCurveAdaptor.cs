using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IGlidingTargetWaypointCurveAdaptor: IWaypointCurveAdaptor{
		IGlidingTargetWaypointCurve GetGlidingTargetWaypointCurve();
	}
	public class GlidingTargetWaypointCurveAdaptor: AbsWaypointCurveAdaptor, IGlidingTargetWaypointCurveAdaptor{
		public override void SetUp(){
			Calculate();
			AbsWaypointCurve.IConstArg arg = new AbsWaypointCurve.ConstArg(
				this,
				thisControlPoints,
				thisCurvePoints
			);
			thisWaypointCurve = new GlidingTargetWaypointCurve(arg);
		}	
		public IGlidingTargetWaypointCurve GetGlidingTargetWaypointCurve(){
			return (IGlidingTargetWaypointCurve)thisWaypointCurve;
		}

	}
}
