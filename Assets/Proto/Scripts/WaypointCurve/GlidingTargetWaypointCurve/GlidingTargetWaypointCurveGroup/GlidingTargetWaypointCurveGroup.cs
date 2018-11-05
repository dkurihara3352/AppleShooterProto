using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetWaypointCurveGroup: ISceneObject{
		IGlidingTargetWaypointCurve[] GetCurves();
		void SetCurves(IGlidingTargetWaypointCurve[] curves);
	}
	public class GlidingTargetWaypointCurveGroup: AbsSceneObject, IGlidingTargetWaypointCurveGroup{
		public GlidingTargetWaypointCurveGroup(
			IConstArg arg
		): base(arg){
			
		}
		IGlidingTargetWaypointCurve[] thisCurves;
		public void SetCurves(IGlidingTargetWaypointCurve[] curves){
			thisCurves = curves;
		}
		public IGlidingTargetWaypointCurve[] GetCurves(){
			return thisCurves;
		}
		public new interface IConstArg: AbsSceneObject.IConstArg{}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IGlidingTargetWaypointCurveGroupAdaptor adaptor
			): base(
				adaptor
			){}
		}
	}
}

