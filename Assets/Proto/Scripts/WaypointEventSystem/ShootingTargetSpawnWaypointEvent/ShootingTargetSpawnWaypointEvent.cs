using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnWaypointEvent: IWaypointEvent{
		IShootingTargetSpawnPoint GetSpawnPoint();
		TargetType GetTargetType();
	}
	public class ShootingTargetSpawnWaypointEvent: AbsWaypointEvent, IShootingTargetSpawnWaypointEvent{
		public ShootingTargetSpawnWaypointEvent(
			IConstArg arg
		): base(arg){
			thisShootingTargetReserve = arg.shootingTargetReserve;
			thisShootingTargetSpawnPoint = arg.shootingTargetSpawnPoint;
		}
		protected override void ExecuteImple(){
			thisShootingTargetReserve.ActivateShootingTargetAt(thisShootingTargetSpawnPoint);
		}
		IShootingTargetSpawnPoint thisShootingTargetSpawnPoint;
		IShootingTargetReserve thisShootingTargetReserve;
		public IShootingTargetSpawnPoint GetSpawnPoint(){
			return thisShootingTargetSpawnPoint;
		}
		public TargetType GetTargetType(){
			return thisShootingTargetReserve.GetTargetType();
		}
		public override string GetName(){
			return GetTargetType().ToString() + " spawn event";
		}
		/*  */
			public new interface IConstArg: AbsWaypointEvent.IConstArg{
				IShootingTargetReserve shootingTargetReserve{get;}
				IShootingTargetSpawnPoint shootingTargetSpawnPoint{get;}
			}
			public new class ConstArg: AbsWaypointEvent.ConstArg, IConstArg{
				public ConstArg(
					IShootingTargetReserve reserve,
					IShootingTargetSpawnPoint point,

					float eventPoint
				): base(
					eventPoint
				){
					thisReserve = reserve;
					thisPoint = point;
				}
				readonly IShootingTargetReserve thisReserve;
				public IShootingTargetReserve shootingTargetReserve{
					get{return thisReserve;}
				}
				readonly IShootingTargetSpawnPoint thisPoint;
				public IShootingTargetSpawnPoint shootingTargetSpawnPoint{
					get{
						return thisPoint;
					}
				}
		}
	}
}


