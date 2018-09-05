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
		ISmoothFollowTargetProcess CreateSmoothFollowTargetProcess(
			ISmoothFollower follower,
			ISmoothFollowTargetMBAdaptor target,
			float smoothCoefficient
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

		public ISmoothFollowTargetProcess CreateSmoothFollowTargetProcess(
			ISmoothFollower smoothFollower,
			ISmoothFollowTargetMBAdaptor target,
			float smoothCoefficient
		){
			ISmoothFollowTargetProcessConstArg arg = new SmoothFollowTargetProcessConstArg(
				thisProcessManager,
				smoothFollower,
				target,
				smoothCoefficient
			);
			return new SmoothFollowTargetProcess(arg);
		}
	}
}
