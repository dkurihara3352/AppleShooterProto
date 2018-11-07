using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnPoint: ISceneObject{
		void SetTarget(IShootingTarget target);
		void CheckAndClearTarget(IShootingTarget target);
		IShootingTarget GetSpawnedTarget();
		void SetIndex(int index);
		int GetIndex();
	}
	public abstract class AbsShootingTargetSpawnPoint: AbsSceneObject, IShootingTargetSpawnPoint{
		public AbsShootingTargetSpawnPoint(
			IConstArg arg
		): base(
			arg
		){
		}
		IShootingTarget thisShootingTarget;
		public void SetTarget(IShootingTarget target){
			thisShootingTarget = target;
		}
		public void CheckAndClearTarget(IShootingTarget target){
			if(thisShootingTarget == target)
				thisShootingTarget = null;
		}
		public IShootingTarget GetSpawnedTarget(){
			return thisShootingTarget;
		}
		int thisIndex;
		public void SetIndex(int index){
			thisIndex = index;
		}
		public int GetIndex(){
			return thisIndex;
		}
	}
}
