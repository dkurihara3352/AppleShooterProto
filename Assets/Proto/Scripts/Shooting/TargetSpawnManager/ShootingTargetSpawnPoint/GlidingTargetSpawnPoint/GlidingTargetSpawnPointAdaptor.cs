using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetSpawnPointAdaptor: IShootingTargetSpawnPointAdaptor{
		IGlidingTargetSpawnPoint GetGlidingTargetSpawnPoint();
	}
	public class GlidingTargetSpawnPointAdaptor: AbsShootingTargetSpawnPointAdaptor, IGlidingTargetSpawnPointAdaptor{
		public GlidingTargetWaypointCurveAdaptor curveAdaptor;
		public override void SetUp(){
			thisSpawnPoint = CreateSpawnPoint();
			DisableAllMeshRenderer();
		}
		IShootingTargetSpawnPoint CreateSpawnPoint(){
			GlidingTargetSpawnPoint.IConstArg arg = new GlidingTargetSpawnPoint.ConstArg(
				eventPoint,
				this
			);
			return new GlidingTargetSpawnPoint(arg);
		}
		IGlidingTargetSpawnPoint thisGlidingTargetSpawnPoint{
			get{
				return (IGlidingTargetSpawnPoint)thisSpawnPoint;
			}
		}
		public IGlidingTargetSpawnPoint GetGlidingTargetSpawnPoint(){
			return thisGlidingTargetSpawnPoint;
		}
		public override void SetUpReference(){
			IGlidingTargetWaypointCurve curve = GetCurve();
			thisGlidingTargetSpawnPoint.SetGlidingTargetWaypointCurve(curve);
		}
		IGlidingTargetWaypointCurve GetCurve(){
			return curveAdaptor.GetGlidingTargetWaypointCurve();
		}

	}
}
