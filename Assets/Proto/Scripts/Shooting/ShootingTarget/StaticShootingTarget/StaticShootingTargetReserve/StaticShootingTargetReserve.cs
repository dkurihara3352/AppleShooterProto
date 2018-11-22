using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
namespace AppleShooterProto{
	public interface IStaticShootingTargetReserve: ISceneObjectReserve<IStaticShootingTarget>, IShootingTargetReserve{
		IStaticShootingTarget[] GetStaticShootingTargets();
	}
	public class StaticShootingTargetReserve : AbsShootingTargetReserve<IStaticShootingTarget>, IStaticShootingTargetReserve {
		public StaticShootingTargetReserve(
			IConstArg arg
		): base(
			arg
		){}
		public override void ActivateShootingTargetAt(IShootingTargetSpawnPoint point){
			IStaticShootingTarget target = GetNext();
			IStaticTargetSpawnPoint typedPoint = (IStaticTargetSpawnPoint)point;
			target.ActivateAt(typedPoint);
		}
		public IStaticShootingTarget[] GetStaticShootingTargets(){
			return thisSceneObjects;
		}
		public override TargetType GetTargetType(){
			return TargetType.Static;
		}
	}
}
