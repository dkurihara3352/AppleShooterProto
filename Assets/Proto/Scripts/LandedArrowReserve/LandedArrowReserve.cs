using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILandedArrowReserve: ISceneObjectReserve<ILandedArrow>{
		void ActivateLandedArrowAt(
			IShootingTarget target,
			Vector3 position,
			Quaternion rotation
		);
	}
	public class LandedArrowReserve : AbsSceneObjectReserve<ILandedArrow>, ILandedArrowReserve {
		public LandedArrowReserve(
			IConstArg arg
		): base(
			arg
		){
		}
		ILandedArrowReserveAdaptor thisTypedAdaptor{
			get{
				return (ILandedArrowReserveAdaptor)thisAdaptor;
			}
		}
		public override void Reserve(ILandedArrow arrow){
			arrow.SetParent(arrow);
			arrow.ResetLocalTransform();
			Vector3 reservedLocalPosition = GetReservedLocalPosition(arrow.GetIndex());
			arrow.SetLocalPosition(reservedLocalPosition);
		}
		float arrowSpace = 1f;
		Vector3 GetReservedLocalPosition(int index){
			float posX = index * arrowSpace;
			return new Vector3(
				posX,
				0f,
				0f
			);
		}

		public void ActivateLandedArrowAt(
			IShootingTarget target,
			Vector3 position,
			Quaternion rotation
		){
			ILandedArrow nextLandedArrow = GetNext();
			nextLandedArrow.Deactivate();
			nextLandedArrow.ActivateAt(
				target,
				position,
				rotation
			);
		}

		/* Const */
			public new interface IConstArg: AbsSceneObject.IConstArg{
			}	
			public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
				public ConstArg(
					ILandedArrowReserveAdaptor adaptor
				): base(adaptor){
				}
			}
		/*  */
	}
}
