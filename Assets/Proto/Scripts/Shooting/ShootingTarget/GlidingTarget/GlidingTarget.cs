using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTarget: IShootingTarget{
		void SetWaypointsFollower(IWaypointsFollower follower);
		void SetGlidingTargetReserve(IGlidingTargetReserve reserve);
		// void SetWaypointsManager(IGlidingTargetWaypointsManager manager);
		// void SetWaypointCurve(IGlidingTargetWaypointCurve curve);
		void ActivateAt(IGlidingTargetWaypointCurve curve);
	}
	public class GlidingTarget: AbsShootingTarget, IGlidingTarget{
		public GlidingTarget(
			AbsShootingTarget.IConstArg arg
		): base(arg){}

		public void ActivateAt(IGlidingTargetWaypointCurve curve){
			Deactivate();
			// SetWaypointsManager(manager);
			SetWaypointCurve(curve);
			Activate();
		}

		public override void DeactivateImple(){
			base.DeactivateImple();
			if(thisWaypointCurve != null)
				thisWaypointCurve.CheckAndClearGlidingTarget(this);
			StopGlide();
		}
		public override void ActivateImple(){
			base.ActivateImple();
			StartGlide();
		}
		IWaypointsFollower thisWaypointsFollower;
		public void SetWaypointsFollower(IWaypointsFollower follower){
			thisWaypointsFollower = follower;
		}
		void StartGlide(){
			StopGlide();
			thisWaypointsFollower.StartFollowing();
		}
		void StopGlide(){
			thisWaypointsFollower.StopFollowing();
		}
		
		IGlidingTargetReserve thisGlidingTargetReserve;
		public void SetGlidingTargetReserve(IGlidingTargetReserve reserve){
			thisGlidingTargetReserve = reserve;
		}
		protected override void ReserveSelf(){
			thisGlidingTargetReserve.Reserve(this);
		}
		IGlidingTargetWaypointCurve thisWaypointCurve;
		public void SetWaypointCurve(IGlidingTargetWaypointCurve curve){
			thisWaypointCurve = curve;
			curve.SetGlidingTarget(this);
			thisWaypointsFollower.SetWaypointCurve(curve);
		}
		// public void SetWaypointsManager(IGlidingTargetWaypointsManager manager){
		// 	thisWaypointsFollower.SetWaypointsManager(manager);
		// 	// manager.SetWaypointsFollower(thisWaypointsFollower);
		// 	manager.SetGlidingTarget(this);
		// 	IWaypointCurve firstCurveInSequence = manager.GetWaypointCurvesInSequence()[0];
		// 	thisWaypointsFollower.SetWaypointCurve(firstCurveInSequence);
		// }
	}
}

