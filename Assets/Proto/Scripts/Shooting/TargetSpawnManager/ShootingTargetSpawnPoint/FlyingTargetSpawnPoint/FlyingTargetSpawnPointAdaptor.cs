using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IFlyingTargetSpawnPointAdaptor: IShootingTargetSpawnPointAdaptor{
		IFlyingTargetSpawnPoint GetFlyingTargetSpawnPoint();
	}
	public class FlyingTargetSpawnPointAdaptor : AbsShootingTargetSpawnPointAdaptor, IFlyingTargetSpawnPointAdaptor {

		IFlyingTargetSpawnPoint thisFlyingTargetSpawnPoint{
			get{
				return (IFlyingTargetSpawnPoint)thisSpawnPoint;
			}
		}
		public IFlyingTargetSpawnPoint GetFlyingTargetSpawnPoint(){
			return thisFlyingTargetSpawnPoint;
		}

		public override void SetUp(){
			thisSpawnPoint = CreateSpawnPoint();
		}
		IFlyingTargetSpawnPoint CreateSpawnPoint(){
			FlyingTargetSpawnPoint.IConstArg arg = new FlyingTargetSpawnPoint.ConstArg(
				this
			);
			return new FlyingTargetSpawnPoint(arg);
		}
		public FlyingTargetWaypointManagerAdaptor flyingTargetWaypointManagerAdaptor;
		public override void SetUpReference(){
			IFlyingTargetWaypointManager manager = GetManager();
			thisFlyingTargetSpawnPoint.SetWaypointManager(manager);
		}
		IFlyingTargetWaypointManager GetManager(){
			return flyingTargetWaypointManagerAdaptor.GetFlyingTargetWaypointManager();
		}
	}
}
