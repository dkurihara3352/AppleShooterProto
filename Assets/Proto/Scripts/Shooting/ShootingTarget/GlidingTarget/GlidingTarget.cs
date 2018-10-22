using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTarget: IShootingTarget{
		void SetWaypointsFollower(IWaypointsFollower follower);
	}
	public class GlidingTarget: TestShootingTarget, IGlidingTarget{
		public GlidingTarget(
			TestShootingTarget.IConstArg arg
		): base(arg){}
		public override void DeactivateImple(){
			base.DeactivateImple();
			ResetGlide();
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
			ResetGlide();
			thisWaypointsFollower.StartFollowing();
		}
		void StopGlide(){
			thisWaypointsFollower.StopFollowing();
		}
		void ResetGlide(){
			StopGlide();
			ResetTransformAtReserve();
		}
		protected override void DestroyTarget(){
			base.DestroyTarget();
			thisWaypointsFollower.StopFollowing();
		}
		
	}
}

