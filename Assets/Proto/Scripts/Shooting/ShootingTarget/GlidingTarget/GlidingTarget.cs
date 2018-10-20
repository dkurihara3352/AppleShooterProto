using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTarget: IShootingTarget{
		void SetWaypointsFollower(IWaypointsFollower follower);
		void StartGlide();
	}
	public class GlidingTarget: TestShootingTarget, IGlidingTarget{
		public GlidingTarget(
			TestShootingTarget.IConstArg arg
		): base(arg){}
		
		public override void ResetTarget(){
			base.ResetTarget();
			thisWaypointsFollower.StopFollowing();
			ResetTransformToReserve();
		}
		void ResetTransformToReserve(){
			ResetTransform();
		}
		IWaypointsFollower thisWaypointsFollower;
		public void SetWaypointsFollower(IWaypointsFollower follower){
			thisWaypointsFollower = follower;
		}
		public void StartGlide(){
			ResetTarget();
			thisWaypointsFollower.StartFollowing();
		}
		protected override void DestroyTarget(){
			base.DestroyTarget();
			thisWaypointsFollower.StopFollowing();
		}
		
	}
}

