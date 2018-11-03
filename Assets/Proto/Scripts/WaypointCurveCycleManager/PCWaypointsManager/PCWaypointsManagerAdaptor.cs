using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPCWaypointsManagerAdaptor: IWaypointCurveCycleManagerAdaptor{
		IPCWaypointsManager GetPCWaypointsManager();
	}
	public class PCWaypointsManagerAdaptor : WaypointCurveCycleManagerAdaptor, IPCWaypointsManagerAdaptor {
		public override void SetUp(){
			PCWaypointsManager.IConstArg arg = new PCWaypointsManager.ConstArg(
				this,
				reserve,
				curvesCountInSequence,
				initialCurvePosition,
				initialCurveRotation,
				cycleStartIndex
			);
			thisWaypointsManager = new PCWaypointsManager(arg);
		}
		public IPCWaypointsManager GetPCWaypointsManager(){
			return GetWaypointsManager() as IPCWaypointsManager;
		}
	}
}
