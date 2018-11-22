using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IFlyingTargetWaypoint: IAppleShooterSceneObject{
	}
	public class FlyingTargetWaypoint: AppleShooterSceneObject, IFlyingTargetWaypoint{
		public FlyingTargetWaypoint(
			IConstArg arg
		): base(
			arg
		){
		}
		/* Const */
		public new interface IConstArg: AppleShooterSceneObject.IConstArg{
		}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IFlyingTargetWaypointAdaptor adaptor
			): base(
				adaptor
			){
			}
		}
	}
}
