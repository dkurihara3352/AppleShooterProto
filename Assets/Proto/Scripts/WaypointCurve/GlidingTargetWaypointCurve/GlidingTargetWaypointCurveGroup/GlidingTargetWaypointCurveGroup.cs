using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IGlidingTargetWaypointCurveGroup: ISlickBowShootingSceneObject{
	}
	public class GlidingTargetWaypointCurveGroup: SlickBowShootingSceneObject, IGlidingTargetWaypointCurveGroup{
		public GlidingTargetWaypointCurveGroup(
			IConstArg arg
		): base(arg){
			
		}
		public new interface IConstArg: SlickBowShootingSceneObject.IConstArg{}
		public new class ConstArg: SlickBowShootingSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IGlidingTargetWaypointCurveGroupAdaptor adaptor
			): base(
				adaptor
			){}
		}
	}
}

