using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointsManagerAdaptor: IMonoBehaviourAdaptor{
		IWaypointsManager GetWaypointsManager();
	}
	public class WaypointsManagerAdaptor : MonoBehaviourAdaptor, IWaypointsManagerAdaptor {
		
		public override void SetUp(){
			IWaypointsManagerConstArg arg = new WaypointsManagerConstArg(
				reserve,
				curvesCountInSequence,
				initialCurvePosition,
				initialCurveRotation
			);
			thisWaypointsManager = new WaypointsManager(arg);
		}
		IWaypointsManager thisWaypointsManager;
		public IWaypointsManager GetWaypointsManager(){
			return thisWaypointsManager;
		}
		public Vector3 initialCurvePosition;
		public Quaternion initialCurveRotation;
		public WaypointsFollowerAdaptor waypointsFollowerAdaptor;
		public List<WaypointCurveAdaptor> waypointCurveAdaptors;
		public Transform reserve;
		public int curvesCountInSequence;
		public override void SetUpReference(){

			IWaypointsFollower follower = waypointsFollowerAdaptor.GetWaypointsFollower();
			thisWaypointsManager.SetWaypointsFollower(follower);
			
			List<IWaypointCurve> waypointCurves = GetWaypointCurves();
			thisWaypointsManager.SetWaypointCurves(waypointCurves);
		}
		List<IWaypointCurve> GetWaypointCurves(){
			List<IWaypointCurve> result = new List<IWaypointCurve>();
			int index = 0;
			foreach(IWaypointCurveAdaptor adaptor in waypointCurveAdaptors){
				IWaypointCurve curve = adaptor.GetWaypointCurve();
				curve.SetIndex(index);
				result.Add(curve);
				index ++;
			}
			return result;
		}
	}
}
