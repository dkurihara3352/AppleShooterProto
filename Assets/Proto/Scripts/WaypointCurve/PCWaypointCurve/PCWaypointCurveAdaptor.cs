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
		// public AbsWaypointCurveAdaptor[] subordinateCurveAdaptors;
		public override void SetUpReference(){
			base.SetUpReference();
			IShootingTargetSpawnManager targetSpawnManager = testTargetSpawnManagerAdaptor.GetShootingTargetSpawnManager();
			thisTypedCurve.SetTargetSpawnManager(targetSpawnManager);
		}
		public override void FinalizeSetUp(){
			IWaypointCurve[] subordinateCurves = CollectSubordinateCurves();
			thisTypedCurve.SetSubordinateCurves(subordinateCurves);
			base.FinalizeSetUp();
		}
		public WaypointCurveCycleManagerAdaptor[] subWaypointsManagersAdaptors;
		IWaypointCurve[] CollectSubordinateCurves(){
			List<IWaypointCurve> resultList = new List<IWaypointCurve>();
			// foreach(AbsWaypointCurveAdaptor adaptor in subordinateCurveAdaptors){
			// 	if(adaptor != null)
			// 		resultList.Add(adaptor.GetWaypointCurve());
			// }
			foreach(WaypointCurveCycleManagerAdaptor waypointsManagerAdaptor in subWaypointsManagersAdaptors){
				IWaypointCurveCycleManager manager = ((IWaypointCurveCycleManagerAdaptor)waypointsManagerAdaptor).GetWaypointsManager();
				IWaypointCurve[] curves = manager.GetAllWaypointCurves().ToArray();
				resultList.AddRange(curves);
			}
			return resultList.ToArray();
		}
	}
}
