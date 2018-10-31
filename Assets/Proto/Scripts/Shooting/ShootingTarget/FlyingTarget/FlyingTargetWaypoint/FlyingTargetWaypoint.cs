using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IFlyingTargetWaypoint: ISceneObject{
	}
	public class FlyingTargetWaypoint: AbsSceneObject, IFlyingTargetWaypoint{
		public FlyingTargetWaypoint(
			IConstArg arg
		): base(
			arg
		){
		}
		/* Const */
		public new interface IConstArg: AbsSceneObject.IConstArg{
		}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IFlyingTargetWaypointAdaptor adaptor
			): base(
				adaptor
			){
			}
		}
	}
}
