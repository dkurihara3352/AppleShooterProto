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
		ILookAtTargetMotionProcess CreateLookAtTargetMotionProcess(
			ILookAtTarget lookAtTarget,
			ISmoothLooker smoothLooker,
			float smoothCoefficient
		);
		ILookAtTargetProcess CreateLookAtTargetProcess(
			ISmoothLooker smoothLooker,
			ILookAtTarget target
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
		public ILookAtTargetMotionProcess CreateLookAtTargetMotionProcess(
			ILookAtTarget lookAtTarget,
			ISmoothLooker smoothLooker,
			float smoothCoefficient
		){
			ILookAtTargetMotionProcessConstArg arg = new LookAtTargetMotionProcessConstArg(
				thisProcessManager,
				lookAtTarget,
				smoothLooker,
				smoothCoefficient
			);
			return new LookAtTargetMotionProcess(arg);
		}
		public ILookAtTargetProcess CreateLookAtTargetProcess(
			ISmoothLooker smoothLooker,
			ILookAtTarget target
		){
			ILookAtTargetProcessConstArg arg = new LookAtTargetProcessConstArg(
				thisProcessManager,
				target,
				smoothLooker
			);
			return new LookAtTargetProcess(arg);
		}
	}
}
