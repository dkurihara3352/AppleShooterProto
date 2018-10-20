using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetWaypointCurveAdaptor: IWaypointCurveAdaptor{}
	public class GlidingTargetWaypointCurveAdaptor: AbsWaypointCurveAdaptor, IGlidingTargetWaypointCurveAdaptor{
		public override void SetUp(){
			AbsWaypointCurve.IConstArg arg = new AbsWaypointCurve.ConstArg(
				this,
				thisControlPoints,
				thisCurvePoints
			);
			thisWaypointCurve = new GlidingTargetWaypointCurve(arg);
		}	

	}
}
