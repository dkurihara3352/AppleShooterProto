using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface ILandedArrow: ISceneObject, IActivationStateHandler, IActivationStateImplementor{
		void SetLandedArrowReserve(ILandedArrowReserve reserve);
		void ActivateAt(
			IShootingTarget target,
			Vector3 position,
			Quaternion rotation
		);

		IShootingTarget GetShootingTarget();
		int GetIndex();
		void SetIndex(int index);

		void SetArrowTwang(IArrowTwang twang);
	}
	public class LandedArrow: AbsSceneObject, ILandedArrow{
		public LandedArrow(
			IConstArg arg
		): base(
			arg
		){

		}
		ILandedArrowReserve thisReserve;
		ILandedArrowAdaptor thisTypedAdaptor{
			get{
				return (ILandedArrowAdaptor)thisAdaptor;
			}
		}
		public void SetLandedArrowReserve(ILandedArrowReserve reserve){
			thisReserve = reserve;
		}
		public void ActivateAt(
			IShootingTarget target,
			Vector3 position,
			Quaternion rotation
		){
			
			SetShootingTarget(
				target,
				position,
				rotation
			);
			Activate();
		}
		IShootingTarget thisTarget;
		void SetShootingTarget(
			IShootingTarget target,
			Vector3 position,
			Quaternion rotation
		){
			RemoveSelfFromCurrentTarget();
			thisTarget = target;
			AddSelfToCurrentTarget();

			SetParent(target);
			ResetLocalTransform();
			SetPosition(position);
			SetRotation(rotation);
		}
		void RemoveSelfFromCurrentTarget(){
			if(thisTarget != null)
				thisTarget.RemoveLandedArrow(this);
		}
		void AddSelfToCurrentTarget(){
			if(thisTarget != null)
				thisTarget.AddLandedArrow(this);
		}
		public IShootingTarget GetShootingTarget(){
			return thisTarget;
		}
		int thisIndex;
		public void SetIndex(int index){
			thisIndex = index;
			thisTypedAdaptor.SetIndexOnTextMesh(index);
		}
		public int GetIndex(){
			return thisIndex;
		}
		public void SetArrowTwang(IArrowTwang twang){
			thisArrowTwang = twang;
		}
		IArrowTwang thisArrowTwang;
		/* ActivationState */
			IActivationStateEngine thisActivationStateEngine;
			public void Activate(){
				thisActivationStateEngine.Activate();
			}
			public void Deactivate(){
				thisActivationStateEngine.Deactivate();
			}
			public bool IsActivated(){
				return thisActivationStateEngine.IsActivated();
			}
			public void ActivateImple(){
				thisArrowTwang.Twang();
			}
			public void DeactivateImple(){
				thisArrowTwang.StopTwang();
				RemoveSelfFromCurrentTarget();
				thisReserve.Reserve(this);
			}
		/* Const */
			public new interface IConstArg: AbsSceneObject.IConstArg{
			}
			public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
				public ConstArg(
					ILandedArrowAdaptor adaptor
				): base(
					adaptor
				){
				}
			}
		/*  */
	}
}
