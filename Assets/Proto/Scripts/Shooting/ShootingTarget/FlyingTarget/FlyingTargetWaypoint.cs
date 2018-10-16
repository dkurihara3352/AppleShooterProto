using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IFlyingTargetWaypoint{
		Vector3 GetPosition();
		IMonoBehaviourAdaptor GetAdaptor();
	}
	public class FlyingTargetWaypoint: IFlyingTargetWaypoint{
		public FlyingTargetWaypoint(IConstArg arg){
			thisAdaptor = arg.adaptor;
		}
		readonly IFlyingTargetWaypointAdaptor thisAdaptor;
		public IMonoBehaviourAdaptor GetAdaptor(){
			return thisAdaptor;
		}
		public Vector3 GetPosition(){
			return thisAdaptor.GetPosition();
		}
		/* Const */
		public interface IConstArg{
			IFlyingTargetWaypointAdaptor adaptor{get;}
		}
		public struct ConstArg: IConstArg{
			public ConstArg(
				IFlyingTargetWaypointAdaptor adaptor
			){
				thisAdaptor = adaptor;
			}
			readonly IFlyingTargetWaypointAdaptor thisAdaptor;
			public IFlyingTargetWaypointAdaptor adaptor{get{return thisAdaptor;}}
		}
	}
}
