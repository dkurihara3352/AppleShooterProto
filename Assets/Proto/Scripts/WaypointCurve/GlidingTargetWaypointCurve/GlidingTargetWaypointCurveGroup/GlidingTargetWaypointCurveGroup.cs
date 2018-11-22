using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetWaypointCurveGroup: IAppleShooterSceneObject{
		IGlidingTargetWaypointCurve[] GetCurves();
		void SetCurves(IGlidingTargetWaypointCurve[] curves);
	}
	public class GlidingTargetWaypointCurveGroup: AppleShooterSceneObject, IGlidingTargetWaypointCurveGroup{
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
		public new interface IConstArg: AppleShooterSceneObject.IConstArg{}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IGlidingTargetWaypointCurveGroupAdaptor adaptor
			): base(
				adaptor
			){}
		}
	}
}

