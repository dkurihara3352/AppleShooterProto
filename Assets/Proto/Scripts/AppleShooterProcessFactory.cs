using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IAppleShooterProcessFactory: IProcessFactory{
		IFollowWaypointProcess CreateFollowWaypointProcess(
			IWaypointsFollower follower,
			float speed,
			int processOrder,
			IWaypointCurve initialCurve,
			IWaypointsManager waypointsManager
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
			IShootingManager shootingManager,
			int processOrder
		);
		ISmoothZoomProcess CreateSmoothZoomProcess(
			IPlayerCamera playerCamera,
			float smoothCoefficient
		);
		IShootingProcess CreateShootingProcess(
			IShootingManager shootingManager,
			float fireRate
		);
		IArrowFlightProcess CreateArrowFlightProcess(
			IArrow arrow,
			float flightSpeed,
			Vector3 worldDirection,
			float flightGravity,
			Vector3 launcherDeltaPosition,
			Vector3 launchPosition
		);
		IFadeProcess CreateFadeProcess(
			IFadable fadable,
			bool fadeIn,
			float fadeTime
		);
		IFlyingTargetFlightProcess CreateFlyingTargetFlightProcess(
			IFlyingTarget flyingTarget,
			Vector3 initialVelocity,
			float distanceThreshold,
			float speed
		);
		IArrowTwangProcess CreateArrowTwangProcess(
			IArrowTwangAdaptor adaptor,
			float twangTime
		);
		IMarkerUIMarkProcess CreateMarkerUIMarkProcess(
			IMarkerUI markerUI
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
			int processOrder,
			IWaypointCurve initialCurve,
			IWaypointsManager waypointsManager
		){
			IFollowWaypointProcessConstArg arg = new FollowWaypointProcessConstArg(
				thisProcessManager,
				follower,
				speed,
				processOrder,

				initialCurve,
				waypointsManager
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
			IShootingManager shootingManager,
			int processOrder
		){
			IDrawProcessConstArg arg = new DrawProcessConstArg(
				thisProcessManager,
				shootingManager,
				processOrder
			);
			return new DrawProcess(arg);
		}
		public ISmoothZoomProcess CreateSmoothZoomProcess(
			IPlayerCamera playerCamera,
			float smoothCoefficient
		){
			ISmoothZoomProcessConstArg arg = new SmoothZoomProcessConstArg(
				playerCamera,
				smoothCoefficient,
				thisProcessManager
			);
			return new SmoothZoomProcess(arg);
		}
		public IShootingProcess CreateShootingProcess(
			IShootingManager shootingManager,
			float fireRate
		){
			IShootingProcessConstArg arg = new ShootingProcessConstArg(
				shootingManager,
				fireRate,
				thisProcessManager
			);
			return new ShootingProcess(arg);
		}
		public IArrowFlightProcess CreateArrowFlightProcess(
			IArrow arrow,
			float flightSpeed,
			Vector3 flightDirection,
			float flightGravity,
			Vector3 launcherVelocity,
			Vector3 launchPosition
		){
			IArrowFlightProcessConstArg arg = new ArrowFlightProcessConstArg(
				arrow,
				flightSpeed,
				flightDirection,
				flightGravity,
				launcherVelocity,
				launchPosition,

				thisProcessManager
			);
			return new ArrowFlightProcess(arg);
		}
		public IFadeProcess CreateFadeProcess(
			IFadable fadable,
			bool fadeIn,
			float fadeTime
		){
			FadeProcess.IConstArg arg = new FadeProcess.ConstArg(
				fadable,
				fadeIn,
				fadeTime,

				thisProcessManager
			);
			return new FadeProcess(arg);
		}
		public IFlyingTargetFlightProcess CreateFlyingTargetFlightProcess(
			IFlyingTarget flyingTarget,
			Vector3 initialVelocity,
			float distanceThreshold,
			float speed
		){
			FlyingTargetFlightProcess.IConstArg arg = new FlyingTargetFlightProcess.ConstArg(
				flyingTarget,
				initialVelocity,
				distanceThreshold,
				speed,
				thisProcessManager
			);
			return new FlyingTargetFlightProcess(arg);
		}
		public IArrowTwangProcess CreateArrowTwangProcess(
			IArrowTwangAdaptor adaptor,
			float twangTime
		){
			ArrowTwangProcess.IConstArg arg = new ArrowTwangProcess.ConstArg(
				thisProcessManager,
				twangTime,
				adaptor
			);
			return new ArrowTwangProcess(arg);
		}
		public IMarkerUIMarkProcess CreateMarkerUIMarkProcess(
			IMarkerUI markerUI
		){
			MarkerUIMarkProcess.IConstArg arg = new MarkerUIMarkProcess.ConstArg(
				markerUI,
				thisProcessManager
			);
			return new MarkerUIMarkProcess(arg);
		}
	}
}
