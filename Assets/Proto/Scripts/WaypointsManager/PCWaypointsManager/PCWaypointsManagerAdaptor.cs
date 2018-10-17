using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPCWaypointsManagerAdaptor: IWaypointsManagerAdaptor{
		IPCWaypointsManager GetPCWaypointsManager();
	}
	public class PCWaypointsManagerAdaptor : WaypointsManagerAdaptor, IPCWaypointsManagerAdaptor {
		public override void SetUp(){
			PCWaypointsManager.IConstArg arg = new PCWaypointsManager.ConstArg(
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
