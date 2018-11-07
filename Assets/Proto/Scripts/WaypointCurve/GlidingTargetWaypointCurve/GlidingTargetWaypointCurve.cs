using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetWaypointCurve: IWaypointCurve{
	}
	public class GlidingTargetWaypointCurve: AbsWaypointCurve, IGlidingTargetWaypointCurve{
		public GlidingTargetWaypointCurve(
			AbsWaypointCurve.IConstArg arg
		): base(arg){

		}
		public override void OnReserve(){}
		public new interface IConstArg: AbsWaypointCurve.IConstArg{}
		public new class ConstArg: AbsWaypointCurve.ConstArg, IConstArg{
			public ConstArg(
				IGlidingTargetWaypointCurveAdaptor adaptor,
				ICurveControlPoint[] controlPoints,
				ICurvePoint[] curvePoints
			): base(
				adaptor,
				controlPoints,
				curvePoints
			){

			}
		}
	}
}

