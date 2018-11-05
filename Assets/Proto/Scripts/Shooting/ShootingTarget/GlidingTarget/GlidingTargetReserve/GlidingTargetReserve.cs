using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetReserve: ISceneObjectReserve<IGlidingTarget>{
		// void ActivateGlidingTargetAt(IWaypointCurve waypointCurve);
		IGlidingTarget[] GetGlidingTargets();
		void ActivateGlidingTargetAt(IGlidingTargetWaypointCurve curve);
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
		public void ActivateGlidingTargetAt(IGlidingTargetWaypointCurve curve){
			IGlidingTarget nextTarget = GetNext();
			nextTarget.ActivateAt(curve);
		}
		public IGlidingTarget[] GetGlidingTargets(){
			return thisSceneObjects;
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

