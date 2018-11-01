using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IStaticShootingTargetReserve: ISceneObjectReserve<IStaticShootingTarget>{
		void ActivateStaticShootingTargetAt(IShootingTargetSpawnPoint point);
	}
	public class StaticShootingTargetReserve : AbsSceneObjectReserve<IStaticShootingTarget>, IStaticShootingTargetReserve {
		public StaticShootingTargetReserve(
			IConstArg arg
		): base(
			arg
		){}
		public override void Reserve(IStaticShootingTarget target){
			target.SetParent(this);
			target.ResetLocalTransform();
			Vector3 reservedPosition = GetReservedLocalPosition(target.GetIndex());
			target.SetLocalPosition(reservedPosition);
		}
		float reservedSpace = 4f;
		Vector3 GetReservedLocalPosition(int index){
			float posX = index * reservedSpace;
			return new Vector3(
				posX, 0f, 0f
			);
		}
		public void ActivateStaticShootingTargetAt(IShootingTargetSpawnPoint point){
			
		}
		/*  */
			public new interface IConstArg: AbsSceneObject.IConstArg{}
			public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
				public ConstArg(
					IStaticShootingTargetReserveAdaptor adaptor
				): base(
					adaptor
				){}
			}
	}
}
