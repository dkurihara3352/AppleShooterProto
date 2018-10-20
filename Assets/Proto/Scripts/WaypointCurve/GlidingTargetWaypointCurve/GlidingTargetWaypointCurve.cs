using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetWaypointCurve: IWaypointCurve{}
	public class GlidingTargetWaypointCurve: AbsWaypointCurve, IGlidingTargetWaypointCurve{
		public GlidingTargetWaypointCurve(
			AbsWaypointCurve.IConstArg arg
		): base(arg){

		}
		public override void OnReserve(){}
	}
}

