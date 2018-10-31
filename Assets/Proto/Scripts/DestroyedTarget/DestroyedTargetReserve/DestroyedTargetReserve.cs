using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IDestroyedTargetReserve: ISceneObjectReserve<IDestroyedTarget>{
		void ActivateDestoryedTargetAt(IShootingTarget target);
	}
	public class DestroyedTargetReserve: AbsSceneObjectReserve<IDestroyedTarget>, IDestroyedTargetReserve{
		public DestroyedTargetReserve(
			IConstArg arg
		): base(
			arg
		){
		}
		public override void Reserve(IDestroyedTarget destroyedTarget){
			destroyedTarget.SetParent(this);
			destroyedTarget.ResetLocalTransform();
			Vector3 reservedPosition = CalculateReservedPosition(
				destroyedTarget.GetIndex()
			);
			destroyedTarget.SetLocalPosition(reservedPosition);
		}
		IDestroyedTargetReserveAdaptor thisTypedAdaptor{
			get{
				return (IDestroyedTargetReserveAdaptor)thisAdaptor;
			}
		}

		float spaceInReserve = 1f;
		Vector3 CalculateReservedPosition(int index){
			float posX = index * (spaceInReserve);
			return new Vector3(
				posX, 0f, 0f
			);
		}
		public void ActivateDestoryedTargetAt(IShootingTarget target){
			IDestroyedTarget nextTarget = GetNext();
			nextTarget.ActivateAt(target);
		}
		/* ConstArg */
			public new interface IConstArg: AbsSceneObject.IConstArg{
			}
			public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
				public ConstArg(
					IDestroyedTargetReserveAdaptor adaptor
				): base(
					adaptor
				){
				}
			}
		/*  */
	}
}

