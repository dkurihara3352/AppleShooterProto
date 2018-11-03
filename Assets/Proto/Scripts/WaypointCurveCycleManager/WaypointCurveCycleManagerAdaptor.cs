using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointCurveCycleManagerAdaptor: IMonoBehaviourAdaptor{
		IWaypointCurveCycleManager GetWaypointsManager();
	}
	public class WaypointCurveCycleManagerAdaptor : MonoBehaviourAdaptor, IWaypointCurveCycleManagerAdaptor {
		
		public override void SetUp(){
			MakeSureCycleStartIndexIsValid();
			WaypointCurveCycleManager.IConstArg arg = new WaypointCurveCycleManager.ConstArg(
				this,
				reserve,
				curvesCountInSequence,
				initialCurvePosition,
				initialCurveRotation,
				cycleStartIndex
			);
			thisWaypointsManager = new WaypointCurveCycleManager(arg);
		}
		void MakeSureCycleStartIndexIsValid(){
			if(cycleStartIndex < 1){
				throw new System.InvalidCastException(
					"cycleStartIndex should be at least 1"
				);
			}
		}
		protected IWaypointCurveCycleManager thisWaypointsManager;
		public IWaypointCurveCycleManager GetWaypointsManager(){
			return thisWaypointsManager;
		}
		public Vector3 initialCurvePosition;
		public Quaternion initialCurveRotation;
		public WaypointsFollowerAdaptor waypointsFollowerAdaptor;
		public Transform reserve;
		public int curvesCountInSequence;
		public int cycleStartIndex = 1;
		public override void SetUpReference(){

			if(waypointsFollowerAdaptor != null){
				IWaypointsFollower follower = waypointsFollowerAdaptor.GetWaypointsFollower();
				thisWaypointsManager.SetWaypointsFollower(follower);
			}
			
			List<IWaypointCurve> waypointCurves = GetWaypointCurvesInChildren();
			thisWaypointsManager.SetWaypointCurves(waypointCurves);
		}

		public override void FinalizeSetUp(){
			thisWaypointsManager.PlaceWaypointCurves();
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
			foreach(IWaypointCurve curve in result){
				int index = result.IndexOf(curve);
				curve.SetIndex(index);
			}
			return result;
		}
	}
}
