using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface ILandedArrow{
		void Reserve();
		void SetLandedArrowReserve(ILandedArrowReserve reserve);
		void SetPosition(Vector3 position);
		void SetRotation(Quaternion rotation);
		void SetParent(Transform parent);
		void ResetLocalTransform();
		void SetShootingTarget(IShootingTarget target);
		IShootingTarget GetShootingTarget();
		int GetIndex();
		void SetIndex(int index);
	}
	public class LandedArrow: ILandedArrow{
		public LandedArrow(
			IConstArg arg
		){
			thisAdaptor = arg.adaptor;
		}
		readonly ILandedArrowAdaptor thisAdaptor;
		ILandedArrowReserve thisReserve;
		public void SetLandedArrowReserve(ILandedArrowReserve reserve){
			thisReserve = reserve;
		}
		public void Reserve(){
			thisReserve.Reserve(this);
			SetShootingTarget(null);
		}
		public void SetPosition(Vector3 position){
			thisAdaptor.SetPosition(position);
		}
		public void SetRotation(Quaternion rotation){
			thisAdaptor.SetRotation(rotation);
		}
		public void SetParent(Transform parent){
			thisAdaptor.SetParent(parent);
		}
		public void ResetLocalTransform(){
			thisAdaptor.ResetLocalTransform();
		}

		IShootingTarget thisTarget;
		public void SetShootingTarget(IShootingTarget target){
			if(thisTarget != null)
				if(thisTarget != target){
					thisTarget.RemoveLandedArrow(this);
				}
			thisTarget = target;
			if(target != null)
				target.AddLandedArrow(this);
		}
		public IShootingTarget GetShootingTarget(){
			return thisTarget;
		}
		int thisIndex;
		public void SetIndex(int index){
			thisIndex = index;
		}
		public int GetIndex(){
			return thisIndex;
		}
		/* Const */
			public interface IConstArg{
				ILandedArrowAdaptor adaptor{get;}
			}
			public struct ConstArg: IConstArg{
				public ConstArg(
					ILandedArrowAdaptor adaptor
				){
					thisAdaptor = adaptor;
				}
				readonly ILandedArrowAdaptor thisAdaptor;
				public ILandedArrowAdaptor adaptor{get{return thisAdaptor;}}
			}
		/*  */
	}
}
