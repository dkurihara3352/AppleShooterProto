using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnPoint: ISceneObject{
		void SetTarget(IShootingTarget target);
		IShootingTarget GetSpawnedTarget();
	}
	public class ShootingTargetSpawnPoint: AbsSceneObject, IShootingTargetSpawnPoint {
		public ShootingTargetSpawnPoint(
			IConstArg arg
		): base(
			arg
		){
		}
		IShootingTarget thisShootingTarget;
		public void SetTarget(IShootingTarget target){
			thisShootingTarget = target;
		}
		public IShootingTarget GetSpawnedTarget(){
			return thisShootingTarget;
		}
		/*  */
		public new interface IConstArg: AbsSceneObject.IConstArg{
		}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IShootingTargetSpawnPointAdaptor adaptor
			): base(
				adaptor
			){
			}
		}
	}
}
