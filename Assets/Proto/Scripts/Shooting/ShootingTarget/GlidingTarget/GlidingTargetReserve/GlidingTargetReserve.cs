using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetReserve: ISceneObjectReserve<IGlidingTarget>{
		// void ActivateGlidingTargetAt(IWaypointCurve waypointCurve);
		void ActivateGlidingTargetAt(IWaypointsManager manager);
	}
	public class GlidingTargetReserve: AbsSceneObjectReserve<IGlidingTarget>, IGlidingTargetReserve{
		public GlidingTargetReserve(
			IConstArg arg
		): base(
			arg
		){
		}
		public override void Reserve(IGlidingTarget target){
			target.SetParent(this);
			target.ResetLocalTransform();
			Vector3 reservedPosition = GetReservedLocalPosition(target.GetIndex());
			target.SetLocalPosition(reservedPosition);
		}
		float reservedSpace = 4f;
		Vector3 GetReservedLocalPosition(int index){
			float posX = index * reservedSpace;
			return new Vector3(posX, 0f, 0f);
		}
		// public void ActivateGlidingTargetAt(IWaypointCurve waypointCurve){
		// 	IGlidingTarget target = GetNext();
		// 	target.Deactivate();
		// 	target.SetWaypointCurveToFollow(waypointCurve);
		// 	target.Activate();
		// }
		public void ActivateGlidingTargetAt(IWaypointsManager manager){
			IGlidingTarget nextTarget = GetNext();
			nextTarget.Deactivate();
			nextTarget.SetWaypointsManager(manager);
			nextTarget.Activate();
		}
		/* Const */
			public new interface IConstArg: AbsSceneObject.IConstArg{}
			public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
				public ConstArg(
					IGlidingTargetReserveAdaptor adaptor
				): base(
					adaptor
				){

				}
			}
	}
}

