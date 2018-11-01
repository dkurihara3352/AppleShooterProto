using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IDestroyedTarget: ISceneObject{
		void ActivateAt(IShootingTarget shootingTarget);
		void Deactivate();
		void SetPopUIReserve(IPopUIReserve reserve);
		void SetDestroyedTargetReserve(IDestroyedTargetReserve reserve);
		int GetIndex();
	}
	public class DestroyedTarget : AbsSceneObject, IDestroyedTarget {
		public DestroyedTarget(
			IConstArg arg
		): base(
			arg
		){
			thisIndex = arg.index;
		}
		int thisIndex;
		public int GetIndex(){
			return thisIndex;
		}
		IDestroyedTargetAdaptor thisTypedAdaptor{
			get{
				return (IDestroyedTargetAdaptor)thisAdaptor;
			}
		}
		IPopUIReserve thisPopUIReserve;
		public void SetPopUIReserve(IPopUIReserve reserve){
			thisPopUIReserve = reserve;
		}

		public void ActivateAt(IShootingTarget target){
			Deactivate();
			Vector3 position = target.GetPosition();
			Quaternion rotation = target.GetRotation();
			SetPosition(position);
			SetRotation(rotation);
			Activate();
		}
		bool thisIsInitialized = false;
		bool thisIsActivated = false;
		void Activate(){
			if(thisIsActivated)
				return;
			ActivateImle();
		}
		void ActivateImle(){
			thisTypedAdaptor.StartDestruction();
			thisPopUIReserve.PopText(
				this,
				"Destroyed"
			);
		}
		public void Deactivate(){
			if(thisIsInitialized && !thisIsActivated)
				return;
			DeactivateImple();
			if(!thisIsInitialized)
				thisIsInitialized = true;
			thisIsActivated = false;
		}
		IDestroyedTargetReserve thisDestroyedTargetReserve;
		public void SetDestroyedTargetReserve(IDestroyedTargetReserve reserve){
			thisDestroyedTargetReserve = reserve;
		}
		void DeactivateImple(){
			thisTypedAdaptor.StopDestruction();
			thisDestroyedTargetReserve.Reserve(this);
		}

		/* Const */
			public new interface IConstArg: AbsSceneObject.IConstArg{
				int index{get;}
			}
			public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
				public ConstArg(
					int index,
					IDestroyedTargetAdaptor adaptor
				): base(
					adaptor
				){
					thisIndex = index;
				}
				readonly int thisIndex;
				public int index{get{return thisIndex;}}
			}
		/*  */
	}
}
