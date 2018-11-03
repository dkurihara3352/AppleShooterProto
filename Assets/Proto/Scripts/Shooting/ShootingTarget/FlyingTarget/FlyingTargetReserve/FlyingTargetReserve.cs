using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IFlyingTargetReserve: ISceneObjectReserve<IFlyingTarget>{
		void ActivateFlyingTargetAt(IFlyingTargetWaypointManager waypointManager);
	}
	public class FlyingTargetReserve : AbsSceneObjectReserve<IFlyingTarget>, IFlyingTargetReserve {
		public FlyingTargetReserve(
			IConstArg arg
		): base(arg){

		}
		public void ActivateFlyingTargetAt(IFlyingTargetWaypointManager waypointManager){
			IFlyingTarget target = GetNext();
			target.Deactivate();
			target.SetWaypointsManager(waypointManager);
			target.Activate();
		}
		public override void Reserve(IFlyingTarget target){
			target.SetParent(this);
			target.ResetLocalTransform();
			Vector2 reservedPosition = GetReservedLocalPosition(target.GetIndex());
			target.SetLocalPosition(reservedPosition);

			target.SetWaypointsManager(null);
		}
		float thisReservedSpace = 4f;
		Vector3 GetReservedLocalPosition(int index){
			float posX = index * thisReservedSpace;
			return new Vector3(
				posX,
				0f,
				0f
			);
		}
		/* Const */
			public new interface IConstArg: AbsSceneObject.IConstArg{
			}
			public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
				public ConstArg(
					IFlyingTargetReserveAdaptor adaptor
				): base(
					adaptor
				){}
			}
	}
}
