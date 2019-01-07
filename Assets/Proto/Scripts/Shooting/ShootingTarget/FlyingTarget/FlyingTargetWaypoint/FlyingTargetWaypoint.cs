using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IFlyingTargetWaypoint: ISlickBowShootingSceneObject{
	}
	public class FlyingTargetWaypoint: SlickBowShootingSceneObject, IFlyingTargetWaypoint{
		public FlyingTargetWaypoint(
			IConstArg arg
		): base(
			arg
		){
		}
		/* Const */
		public new interface IConstArg: SlickBowShootingSceneObject.IConstArg{
		}
		public new class ConstArg: SlickBowShootingSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IFlyingTargetWaypointAdaptor adaptor
			): base(
				adaptor
			){
			}
		}
	}
}
