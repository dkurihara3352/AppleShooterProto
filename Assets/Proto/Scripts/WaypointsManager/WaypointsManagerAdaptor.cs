using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointsManagerAdaptor: IMonoBehaviourAdaptor{
		IWaypointsManager GetWaypointsManager();
	}
	public class WaypointsManagerAdaptor : MonoBehaviourAdaptor, IWaypointsManagerAdaptor {
		
		public override void SetUp(){
			MakeSureCycleStartIndexIsValid();
			WaypointsManager.IConstArg arg = new WaypointsManager.ConstArg(
				reserve,
				curvesCountInSequence,
				initialCurvePosition,
				initialCurveRotation,
				cycleStartIndex
			);
			thisWaypointsManager = new WaypointsManager(arg);
		}
		void MakeSureCycleStartIndexIsValid(){
			if(cycleStartIndex < 1){
				throw new System.InvalidCastException(
					"cycleStartIndex should be at least 1"
				);
			}
		}
		protected IWaypointsManager thisWaypointsManager;
		public IWaypointsManager GetWaypointsManager(){
			return thisWaypointsManager;
		}
		public Vector3 initialCurvePosition;
		public Quaternion initialCurveRotation;
		public WaypointsFollowerAdaptor waypointsFollowerAdaptor;
		public Transform reserve;
		public int curvesCountInSequence;
		public int cycleStartIndex = 1;
		public override void SetUpReference(){

			IWaypointsFollower follower = waypointsFollowerAdaptor.GetWaypointsFollower();
			thisWaypointsManager.SetWaypointsFollower(follower);
			
			List<IWaypointCurve> waypointCurves = GetWaypointCurvesInChildren();
			thisWaypointsManager.SetWaypointCurves(waypointCurves);
		}
		List<IWaypointCurve> GetWaypointCurvesInChildren(){
			List<IWaypointCurve> result = new List<IWaypointCurve>();
			int childCount = transform.childCount;
			for(int i = 0; i < childCount; i++){
				Transform child = transform.GetChild(i);
				IWaypointCurveAdaptor curveAdaptor = (IWaypointCurveAdaptor)child.GetComponent(typeof(IWaypointCurveAdaptor));
				if(curveAdaptor != null)
					result.Add(curveAdaptor.GetWaypointCurve());
			}
			foreach(IWaypointCurve curve in result)
				curve.SetIndex(result.IndexOf(curve));
			return result;
		}
	}
}
