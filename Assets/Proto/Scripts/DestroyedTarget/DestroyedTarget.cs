using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IDestroyedTarget{
		void ActivateAt(IShootingTarget shootingTarget);
		void Deactivate();
	}
	public class DestroyedTarget : IDestroyedTarget {
		public DestroyedTarget(
			IConstArg arg
		){
			thisAdaptor = arg.adaptor;
		}
		readonly IDestroyedTargetAdaptor thisAdaptor;

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
		void SetPosition(Vector3 position){
			thisAdaptor.SetPosition(position);
		}
		void SetRotation(Quaternion rotation){
			thisAdaptor.SetRotation(rotation);
		}
		void Activate(){
			if(thisIsActivated)
				return;
			ActivateImle();
		}
		void ActivateImle(){
			thisAdaptor.StartDestruction();
			thisAdaptor.PopText("Destroyed");
		}
		public void Deactivate(){
			if(thisIsInitialized && !thisIsActivated)
				return;
			DeactivateImple();
			if(!thisIsInitialized)
				thisIsInitialized = true;
			thisIsActivated = false;
		}
		void DeactivateImple(){
			thisAdaptor.StopDestruction();
			thisAdaptor.ResetAtReserve();
		}

		/* Const */
			public interface IConstArg{
				IDestroyedTargetAdaptor adaptor{get;}
			}
			public struct ConstArg: IConstArg{
				public ConstArg(
					IDestroyedTargetAdaptor adaptor
				){
					thisAdaptor = adaptor;
				}
				readonly IDestroyedTargetAdaptor thisAdaptor;
				public IDestroyedTargetAdaptor adaptor{get{return thisAdaptor;}}
			}
		/*  */
	}
}
