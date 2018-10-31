using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetReserve<T>: ISceneObject where T: IShootingTarget{
		void Reserve(T target);
		T GetNextShootingTarget();

		void SetShootingTargets(T[] shootingTargets);
		T[] GetShootingTargets();
	}
	public class ShootingTargetReserve<T> :AbsSceneObject, IShootingTargetReserve<T> where T: IShootingTarget{
		public ShootingTargetReserve(
			IConstArg arg
		): base(
			arg
		){}
		public virtual void SetShootingTargets(T[] targets){
			thisShootingTargets = targets;
		}
		protected T[] thisShootingTargets;
		public T[] GetShootingTargets(){
			return thisShootingTargets;
		}
		public virtual void Reserve(T target){
			target.SetParent(this);
			target.ResetLocalTransform();
			Vector3 reservedLocalPosition = GetReservedLocalPosition(target.GetIndex());
			target.SetLocalPosition(reservedLocalPosition);
		}
		float thisReservedSpace = 1f;
		Vector3 GetReservedLocalPosition(int index){
			float posX = index * thisReservedSpace;
			return new Vector3(
				posX,
				0f,
				0f
			);
		}
		int nextIndex = 0;
		public T GetNextShootingTarget(){
			T nextShootingTarget = thisShootingTargets[nextIndex];
			nextIndex ++;
			if(nextIndex >= thisShootingTargets.Length)
				nextIndex = 0;
			return nextShootingTarget;
		}
	}
}

