using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IDestroyedTarget: ISceneObject, IActivationStateHandler, IActivationStateImplementor{
		void ActivateAt(IShootingTarget shootingTarget);
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
