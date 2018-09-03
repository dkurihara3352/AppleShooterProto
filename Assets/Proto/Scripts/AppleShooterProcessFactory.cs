using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IAppleShooterProcessFactory: IProcessFactory{
		IFollowWaypointProcess CreateFollowWaypointProcess(
			IWaypointsFollower follower,
			float speed
		);
	}

	public class AppleShooterProcessFactory: AbsProcessFactory, IAppleShooterProcessFactory {
		public AppleShooterProcessFactory(
			IProcessManager processManager
		): base(
			processManager
		){
		}
		public IFollowWaypointProcess CreateFollowWaypointProcess(
			IWaypointsFollower follower,
			float speed
		){
			IFollowWaypointProcessConstArg arg = new FollowWaypointProcessConstArg(
				thisProcessManager,
				follower,
				speed
			);
			return new FollowWaypointProcess(arg);
		}
	}
}
