using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetWaypointCurveGroup: IAppleShooterSceneObject{
	}
	public class GlidingTargetWaypointCurveGroup: AppleShooterSceneObject, IGlidingTargetWaypointCurveGroup{
		public GlidingTargetWaypointCurveGroup(
			IConstArg arg
		): base(arg){
			
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

