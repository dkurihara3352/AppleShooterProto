using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnPoint: IAppleShooterSceneObject{
		void SetTarget(IShootingTarget target);
		void CheckAndClearTarget(IShootingTarget target);
		IShootingTarget GetSpawnedTarget();
		void SetIndex(int index);
		int GetIndex();
		float GetEventPoint();
		float GetRelativeRareChance();
	}
	public abstract class AbsShootingTargetSpawnPoint: AppleShooterSceneObject, IShootingTargetSpawnPoint{
		public AbsShootingTargetSpawnPoint(
			IConstArg arg
		): base(
			arg
		){
			thisEventPoint = arg.eventPoint;
		}
		IShootingTargetSpawnPointAdaptor thisShootingTargetSpawnPointAdaptor{
			get{
				return (IShootingTargetSpawnPointAdaptor)thisAdaptor;
			}
		}
		readonly float thisEventPoint;
		public float GetEventPoint(){
			return thisEventPoint;
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
		public float GetRelativeRareChance(){
			return thisShootingTargetSpawnPointAdaptor.GetRelativeRareChance();
		}
		public new interface IConstArg: AppleShooterSceneObject.IConstArg{
			float eventPoint{get;}
		}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				float eventPoint,
				IShootingTargetSpawnPointAdaptor adaptor
			): base(
				adaptor
			){
				thisEventPoint = eventPoint;
			}
			readonly float thisEventPoint;
			public float eventPoint{
				get{
					return thisEventPoint;
				}
			}
		}
	}
}
