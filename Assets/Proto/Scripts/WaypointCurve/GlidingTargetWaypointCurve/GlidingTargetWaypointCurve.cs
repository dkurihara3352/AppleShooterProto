using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetWaypointCurve: IWaypointCurve{
		void SetGlidingTarget(IGlidingTarget target);
		void CheckAndClearGlidingTarget(IGlidingTarget target);
		IGlidingTarget GetGlidingTarget();
	}
	public class GlidingTargetWaypointCurve: AbsWaypointCurve, IGlidingTargetWaypointCurve{
		public GlidingTargetWaypointCurve(
			AbsWaypointCurve.IConstArg arg
		): base(arg){

		}
		public override void OnReserve(){}

		IGlidingTarget thisGlidingTarget;
		public void SetGlidingTarget(IGlidingTarget target){
			if(thisGlidingTarget != null){
				if(thisGlidingTarget != target)
					thisGlidingTarget.Deactivate();
			}
			thisGlidingTarget = target;
		}
		public IGlidingTarget GetGlidingTarget(){
			return thisGlidingTarget;
		}
		public void CheckAndClearGlidingTarget(IGlidingTarget target){
			if(thisGlidingTarget != null){
				if(thisGlidingTarget == target)
					thisGlidingTarget = null;
			}
		}
		

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

