using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTarget: IShootingTarget{
		void SetWaypointsFollower(IWaypointsFollower follower);
		void SetGlidingTargetReserve(IGlidingTargetReserve reserve);
		// void SetWaypointCurveToFollow(IWaypointCurve curve);
		void SetWaypointsManager(IWaypointsManager manager);
	}
	public class GlidingTarget: AbsShootingTarget, IGlidingTarget{
		public GlidingTarget(
			AbsShootingTarget.IConstArg arg
		): base(arg){}

		public override void DeactivateImple(){
			base.DeactivateImple();
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
		// public void SetWaypointCurveToFollow(IWaypointCurve curve){
		// 	thisWaypointsFollower.SetWaypointCurve(curve);
		// }
		public void SetWaypointsManager(IWaypointsManager manager){
			thisWaypointsFollower.SetWaypointsManager(manager);
			manager.SetWaypointsFollower(thisWaypointsFollower);
			// IWaypointCurve curve = manager.GetCurrentCurve();
			IWaypointCurve firstCurveInSequence = manager.GetWaypointCurvesInSequence()[0];
			thisWaypointsFollower.SetWaypointCurve(firstCurveInSequence);
		}
	}
}

