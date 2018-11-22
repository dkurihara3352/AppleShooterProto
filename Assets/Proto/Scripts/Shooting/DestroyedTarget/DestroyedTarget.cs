using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
namespace AppleShooterProto{
	public interface IDestroyedTarget: IAppleShooterSceneObject, IActivationStateHandler, IActivationStateImplementor{
		void ActivateAt(IShootingTarget shootingTarget);
		void SetPopUIReserve(IPopUIReserve reserve);
		void SetDestroyedTargetReserve(IDestroyedTargetReserve reserve);
		int GetIndex();
		IShootingTarget GetShootingTarget();
		void StopParticleSystem();

	}
	public class DestroyedTarget : AppleShooterSceneObject, IDestroyedTarget {
		public DestroyedTarget(
			IConstArg arg
		): base(
			arg
		){
			thisIndex = arg.index;
			thisActivationStateEngine = new ActivationStateEngine(this);
		}
		IActivationStateEngine thisActivationStateEngine;
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
			thisTarget = target;
			thisTarget.SetDestroyedTarget(this);
					
			Vector3 position = target.GetPosition();
			Quaternion rotation = target.GetRotation();
			SetPosition(position);
			SetRotation(rotation);
			Activate();
		}
		public void Activate(){
			thisActivationStateEngine.Activate();
		}
		public void ActivateImple(){
			thisTypedAdaptor.StartDestruction();
			thisPopUIReserve.PopText(
				this,
				"Destroyed"
			);
		}
		IShootingTarget thisTarget;
		public IShootingTarget GetShootingTarget(){
			return thisTarget;
		}
		public bool IsActivated(){
			return thisActivationStateEngine.IsActivated();
		}
		public void Deactivate(){
			thisActivationStateEngine.Deactivate();
		}
		IDestroyedTargetReserve thisDestroyedTargetReserve;
		public void SetDestroyedTargetReserve(IDestroyedTargetReserve reserve){
			thisDestroyedTargetReserve = reserve;
		}
		public void DeactivateImple(){
			if(thisTarget != null)
				thisTarget.CheckAndClearDestroyedTarget(this);
			thisTarget = null;
			thisTypedAdaptor.StopDestruction();
			// thisTypedAdaptor.StopParticleSystem();
			thisDestroyedTargetReserve.Reserve(this);
		}
		public void StopParticleSystem(){
			thisTypedAdaptor.StopParticleSystem();
		}
		/* Const */
			public new interface IConstArg: AppleShooterSceneObject.IConstArg{
				int index{get;}
			}
			public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
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
