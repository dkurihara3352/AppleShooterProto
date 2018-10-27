using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IDestroyedTargetReserve{
		void Reserve(IDestroyedTargetAdaptor targetAdaptor);
		void ActivateDestoryedTargetAt(IShootingTarget target);
		void SetDestroyedTargets(IDestroyedTarget[] targets);
	}
	public class DestroyedTargetReserve: IDestroyedTargetReserve{
		public DestroyedTargetReserve(
			IConstArg arg
		){
			thisAdaptor = arg.adaptor;
		}
		public void Reserve(IDestroyedTargetAdaptor adaptor){
			adaptor.SetParent(thisAdaptor.GetTransform());
			adaptor.ResetLocalTransform();
			Vector3 reservedPosition = CalculateReservedPosition(
				adaptor.GetIndex()
			);
			adaptor.SetLocalPosition(reservedPosition);
		}
		IDestroyedTargetReserveAdaptor thisAdaptor;

		float spaceInReserve = 1f;
		Vector3 CalculateReservedPosition(int index){
			float posX = index * (spaceInReserve);
			return new Vector3(
				posX, 0f, 0f
			);
		}
		public void ActivateDestoryedTargetAt(IShootingTarget target){
			IDestroyedTarget nextTarget = GetNextDestroyedTarget();
			nextTarget.ActivateAt(target);
		}
		int nextIndex = 0;
		IDestroyedTarget[] thisTargets;
		public void SetDestroyedTargets(IDestroyedTarget[] targets){
			thisTargets = targets;
		}
		IDestroyedTarget GetNextDestroyedTarget(){
			IDestroyedTarget nextTarget = thisTargets[nextIndex];
			nextIndex ++;
			if(nextIndex >= thisTargets.Length)
				nextIndex = 0;
			return nextTarget;
		}
		/* ConstArg */
			public interface IConstArg{
				IDestroyedTargetReserveAdaptor adaptor{get;}
			}
			public struct ConstArg: IConstArg{
				public ConstArg(
					IDestroyedTargetReserveAdaptor adaptor
				){
					thisAdaptor = adaptor;
				}
				readonly IDestroyedTargetReserveAdaptor thisAdaptor;
				public IDestroyedTargetReserveAdaptor adaptor{get{return thisAdaptor;}}
			}
		/*  */
	}
}

