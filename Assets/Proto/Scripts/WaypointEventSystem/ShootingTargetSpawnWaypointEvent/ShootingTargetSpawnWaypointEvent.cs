using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnWaypointEvent: IWaypointEvent{
		IShootingTargetSpawnPoint GetSpawnPoint();
		TargetType GetTargetType();
		bool IsRare();
		void MarkRare();
	}
	public class ShootingTargetSpawnWaypointEvent: AbsWaypointEvent, IShootingTargetSpawnWaypointEvent{
		public ShootingTargetSpawnWaypointEvent(
			IConstArg arg
		): base(arg){
			thisShootingTargetReserve = arg.shootingTargetReserve;
			thisShootingTargetSpawnPoint = arg.shootingTargetSpawnPoint;
			thisSpawner = arg.targetSpawner;
		}
		protected override void ExecuteImple(){
			thisShootingTargetReserve.ActivateShootingTargetAt(thisShootingTargetSpawnPoint);
		}
		IShootingTargetSpawnPoint thisShootingTargetSpawnPoint;
		IShootingTargetReserve thisShootingTargetReserve;
		ILevelSectionShootingTargetSpawner thisSpawner;
		protected override bool IsExecutable(){
			return thisSpawner.ShouldSpawnTargets();
		}
		public IShootingTargetSpawnPoint GetSpawnPoint(){
			return thisShootingTargetSpawnPoint;
		}
		public TargetType GetTargetType(){
			return thisShootingTargetReserve.GetTargetType();
		}
		public override string GetName(){
			return GetTargetType().ToString() + " spawn event";
		}
		bool thisIsRare = false;
		public void MarkRare(){
			thisIsRare = true;
		}
		public bool IsRare(){
			return thisIsRare;
		}
		/*  */
			public new interface IConstArg: AbsWaypointEvent.IConstArg{
				IShootingTargetReserve shootingTargetReserve{get;}
				IShootingTargetSpawnPoint shootingTargetSpawnPoint{get;}
				ILevelSectionShootingTargetSpawner targetSpawner{get;}
			}
			public new class ConstArg: AbsWaypointEvent.ConstArg, IConstArg{
				public ConstArg(
					IShootingTargetReserve reserve,
					IShootingTargetSpawnPoint point,

					float eventPoint,
					ILevelSectionShootingTargetSpawner spawner
				): base(
					eventPoint
				){
					thisReserve = reserve;
					thisPoint = point;
					thisSapwner = spawner;
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
				readonly ILevelSectionShootingTargetSpawner thisSapwner;
				public ILevelSectionShootingTargetSpawner targetSpawner{get{return thisSapwner;}}
			}
	}
}


