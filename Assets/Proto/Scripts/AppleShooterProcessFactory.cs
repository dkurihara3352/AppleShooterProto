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
			IMonoBehaviourAdaptor target,
			float smoothCoefficient
		);
		IPlayerCharacterLookAtTargetMotionProcess CreateLookAtTargetMotionProcess(
			IPlayerCharacterLookAtTarget lookAtTarget,
			ISmoothLooker smoothLooker
		);
		ISmoothLookProcess CreateSmoothLookProcess(
			ISmoothLooker smoothLooker,
			IMonoBehaviourAdaptor lookAtTarget,
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
			IMonoBehaviourAdaptor target,
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
		public IPlayerCharacterLookAtTargetMotionProcess CreateLookAtTargetMotionProcess(
			IPlayerCharacterLookAtTarget lookAtTarget,
			ISmoothLooker smoothLooker
		){
			IPlayerCharacterLookAtTargetMotionProcessConstArg arg = new PlayerCharacterLookAtTargetMotionProcessConstArg(
				thisProcessManager,
				lookAtTarget,
				smoothLooker
			);
			return new PlayerCharacterLookAtTargetMotionProcess(arg);
		}
		public ISmoothLookProcess CreateSmoothLookProcess(
			ISmoothLooker smoothLooker,
			IMonoBehaviourAdaptor lookAtTarget,
			float smoothCoefficient
		){
			ISmoothLookProcessConstArg arg = new SmoothLookProcessConstArg(
				thisProcessManager,
				lookAtTarget,
				smoothLooker,
				smoothCoefficient
			);
			return new SmoothLookProcess(arg);
		}
	}
}
