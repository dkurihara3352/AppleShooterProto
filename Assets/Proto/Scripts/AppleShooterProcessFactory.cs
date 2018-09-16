using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IAppleShooterProcessFactory: IProcessFactory{
		IFollowWaypointProcess CreateFollowWaypointProcess(
			IWaypointsFollower follower,
			float speed,
			int processOrder
		);
		ISmoothFollowTargetProcess CreateSmoothFollowTargetProcess(
			ISmoothFollower follower,
			IMonoBehaviourAdaptor target,
			float smoothCoefficient,
			int processOrder
		);
		IPlayerCharacterLookAtTargetMotionProcess CreateLookAtTargetMotionProcess(
			IPlayerCharacterLookAtTarget lookAtTarget,
			ISmoothLooker smoothLooker,
			int processOrder
		);
		ISmoothLookProcess CreateSmoothLookProcess(
			ISmoothLooker smoothLooker,
			IMonoBehaviourAdaptor lookAtTarget,
			float smoothCoefficient,
			int processOrder
		);
		IWaitAndSwitchToIdleStateProcess CreateWaitAndSwitchToIdleStateProcess(
			IPlayerInputStateEngine engine
		);
		IDrawProcess CreateDrawProcess(
			IShootingManager shootingManager
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
			float speed,
			int processOrder
		){
			IFollowWaypointProcessConstArg arg = new FollowWaypointProcessConstArg(
				thisProcessManager,
				follower,
				speed,
				processOrder
			);
			return new FollowWaypointProcess(arg);
		}

		public ISmoothFollowTargetProcess CreateSmoothFollowTargetProcess(
			ISmoothFollower smoothFollower,
			IMonoBehaviourAdaptor target,
			float smoothCoefficient,
			int processOrder
		){
			ISmoothFollowTargetProcessConstArg arg = new SmoothFollowTargetProcessConstArg(
				thisProcessManager,
				smoothFollower,
				target,
				smoothCoefficient,
				processOrder
			);
			return new SmoothFollowTargetProcess(arg);
		}
		public IPlayerCharacterLookAtTargetMotionProcess CreateLookAtTargetMotionProcess(
			IPlayerCharacterLookAtTarget lookAtTarget,
			ISmoothLooker smoothLooker,
			int processOrder
		){
			IPlayerCharacterLookAtTargetMotionProcessConstArg arg = new PlayerCharacterLookAtTargetMotionProcessConstArg(
				thisProcessManager,
				lookAtTarget,
				smoothLooker,
				processOrder
			);
			return new PlayerCharacterLookAtTargetMotionProcess(arg);
		}
		public ISmoothLookProcess CreateSmoothLookProcess(
			ISmoothLooker smoothLooker,
			IMonoBehaviourAdaptor lookAtTarget,
			float smoothCoefficient,
			int processOrder
		){
			ISmoothLookProcessConstArg arg = new SmoothLookProcessConstArg(
				thisProcessManager,
				lookAtTarget,
				smoothLooker,
				smoothCoefficient,
				processOrder
			);
			return new SmoothLookProcess(arg);
		}
		public IWaitAndSwitchToIdleStateProcess CreateWaitAndSwitchToIdleStateProcess(
			IPlayerInputStateEngine engine
		){
			IWaitAndSwitchToIdleStateProcessConstArg arg = new WaitAndSwitchToIdleStateProcessCosntArg(
				thisProcessManager,
				thisProcessManager.GetWaitAndSwitchToIdleStateProcessExpireTime(),
				engine
			);
			return new WaitAndSwitchToIdleStateProcess(arg);
		}
		public IDrawProcess CreateDrawProcess(
			IShootingManager shootingManager
		){
			IDrawProcessConstArg arg = new DrawProcessConstArg(
				thisProcessManager,
				shootingManager
			);
			return new DrawProcess(arg);
		}
	}
}
