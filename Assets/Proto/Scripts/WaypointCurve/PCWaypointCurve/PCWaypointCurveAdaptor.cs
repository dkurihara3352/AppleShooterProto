using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPCWaypointCurveAdaptor: IWaypointCurveAdaptor{}
	public class PCWaypointCurveAdaptor: AbsWaypointCurveAdaptor, IPCWaypointCurveAdaptor{
		public override void SetUp(){
			AbsWaypointCurve.IConstArg arg = new AbsWaypointCurve.ConstArg(
				this,
				thisControlPoints,
				thisCurvePoints
			);
			thisWaypointCurve = new PCWaypointCurve(arg);
		}
		public ShootingTargetSpawnManagerAdaptor testTargetSpawnManagerAdaptor;
		IPCWaypointCurve thisTypedCurve{
			get{return thisWaypointCurve as IPCWaypointCurve;}
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IShootingTargetSpawnManager targetSpawnManager = testTargetSpawnManagerAdaptor.GetShootingTargetSpawnManager();
			thisTypedCurve.SetTargetSpawnManager(targetSpawnManager);
		}	
	}
}
