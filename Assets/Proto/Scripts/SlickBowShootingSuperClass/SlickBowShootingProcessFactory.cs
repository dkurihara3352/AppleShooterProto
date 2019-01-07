using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
using UnityBase;
using UISystem;

namespace SlickBowShooting{
	public interface ISlickBowShootingProcessFactory: IUISystemProcessFactory{
		IFollowWaypointProcess CreateFollowWaypointProcess(
			IWaypointsFollower follower,
			float speed,
			int processOrder,
			IWaypointCurve initialCurve,
			IWaypointCurveCycleManager waypointsManager,
			float initialTime
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
			IPlayerInputStateEngine engine,
			float waitTime
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
			Vector3 launchPosition,
			float flightTime
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

		IDestroyedTargetParticleProcess CreateDestroyedTargetParticleProcess(
			IDestroyedTargetAdaptor adaptor,
			float particleSystemDuration
		);
		IHeatCountDownProcess CreateHeatCountDownProcess(
			float heatDecayRate,
			IHeatManagerStateEngine engine
		);
		IHeatImageSmoothFollowDeltaImageProcess CreateHeatImageSmoothFollowDeltaImageProcess(
			IHeatImage heatImage,
			float followTime,
			float targetHeat
		);
		// IHeatImageWaitForNextAdditionProcess CreateHeatImageWaitForNextAdditionProcess(
		// 	IHeatImage heatImage,
		// 	float comboTime
		// );
		IHeatLevelUpProcess CreateHeatLevelUpProcess(
			IHeatManager heatManager,
			float targetMaxHeat,
			float smoothTime
		);
		IArrowTrailFadeProcess CreateArrowTrailFadeProcess(
			float fadeTime,
			IArrowTrail trail,
			float initialAlpha
		);
		IWaypointsFollowerChangeSpeedProcess CreateWaypointsFollowerChangeSpeedProcess(
			IFollowWaypointProcess followProcess,
			float time,
			AnimationCurve speedCurve
		);
		IGameplayUnpauseProcess CreateGameplayUnpauseProcess(
			IGameplayPause gameplayPause,
			float time
		);
		ICriticalFlashProcess CreateCriticalFlashProcess(
			ICriticalFlash flash,
			AnimationCurve flashCurve,
			float flashTime
		);
	}

	public class SlickBowShootingProcessFactory: UISystemProcessFactory, ISlickBowShootingProcessFactory {
		public SlickBowShootingProcessFactory(
			IProcessManager processManager,
			ISlickBowShootingMonoBehaviourAdaptorManager adaptorManager
		): base(
			processManager,
			adaptorManager
		){
		}
		public IFollowWaypointProcess CreateFollowWaypointProcess(
			IWaypointsFollower follower,
			float speed,
			int processOrder,
			IWaypointCurve initialCurve,
			IWaypointCurveCycleManager waypointsManager,
			float initialTime
		){
			FollowWaypointProcess.IConstArg arg = new FollowWaypointProcess.ConstArg(
				thisProcessManager,
				follower,
				speed,
				processOrder,

				initialCurve,
				waypointsManager,

				initialTime
			);
			return new FollowWaypointProcess(arg);
		}

		public ISmoothFollowTargetProcess CreateSmoothFollowTargetProcess(
			ISmoothFollower smoothFollower,
			IMonoBehaviourAdaptor target,
			float smoothCoefficient,
			int processOrder
		){
			SmoothFollowTargetProcess.IConstArg arg = new SmoothFollowTargetProcess.ConstArg(
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
			PlayerCharacterLookAtTargetMotionProcess.IConstArg arg = new PlayerCharacterLookAtTargetMotionProcess.ConstArg(
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
			SmoothLookProcess.IConstArg arg = new SmoothLookProcess.ConstArg(
				thisProcessManager,
				lookAtTarget,
				smoothLooker,
				smoothCoefficient,
				processOrder
			);
			return new SmoothLookProcess(arg);
		}
		public IWaitAndSwitchToIdleStateProcess CreateWaitAndSwitchToIdleStateProcess(
			IPlayerInputStateEngine engine,
			float waitTime//2f
		){
			WaitAndSwitchToIdleStateProcess.IConstArg arg = new WaitAndSwitchToIdleStateProcess.ConstArg(
				thisProcessManager,
				waitTime,
				engine
			);
			return new WaitAndSwitchToIdleStateProcess(arg);
		}
		public IDrawProcess CreateDrawProcess(
			IShootingManager shootingManager,
			int processOrder
		){
			DrawProcess.IConstArg arg = new DrawProcess.ConstArg(
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
			SmoothZoomProcess.IConstArg arg = new SmoothZoomProcess.ConstArg(
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
			ShootingProcess.IConstArg arg = new ShootingProcess.ConstArg(
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
			Vector3 launchPosition,
			float flightTime
		){
			ArrowFlightProcess.IConstArg arg = new ArrowFlightProcess.ConstArg(
				arrow,
				flightSpeed,
				flightDirection,
				flightGravity,
				launcherVelocity,
				launchPosition,

				thisProcessManager,
				flightTime
			);
			return new ArrowFlightProcess(arg);
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
		public IDestroyedTargetParticleProcess CreateDestroyedTargetParticleProcess(
			IDestroyedTargetAdaptor adaptor,
			float particleDuration
		){
			DestroyedTargetParticleProcess.IConstArg arg = new DestroyedTargetParticleProcess.ConstArg(
				adaptor,
				thisProcessManager,
				particleDuration
			);
			return new DestroyedTargetParticleProcess(arg);
		}
		public IHeatCountDownProcess CreateHeatCountDownProcess(
			float heatDecayRate,
			IHeatManagerStateEngine engine
		){
			HeatCountDownProcess.IConstArg arg = new HeatCountDownProcess.ConstArg(
				thisProcessManager,
				engine,
				heatDecayRate
			);
			return new HeatCountDownProcess(arg);
		}
		public IHeatImageSmoothFollowDeltaImageProcess CreateHeatImageSmoothFollowDeltaImageProcess(
			IHeatImage heatImage,
			float followTime,
			float targetHeat
		){
			HeatImageSmoothFollowDeltaImageProcess.IConstArg arg = new HeatImageSmoothFollowDeltaImageProcess.ConstArg(
				thisProcessManager,
				heatImage,
				targetHeat,
				followTime
			);
			return new HeatImageSmoothFollowDeltaImageProcess(arg);
		}
		// public IHeatImageWaitForNextAdditionProcess CreateHeatImageWaitForNextAdditionProcess(
		// 	IHeatImage heatImage,
		// 	float comboTime
		// ){
		// 	HeatImageWaitForNextAdditionProcess.IConstArg arg = new HeatImageWaitForNextAdditionProcess.ConstArg(
		// 		thisProcessManager,
		// 		comboTime,
		// 		heatImage
		// 	);
		// 	return new HeatImageWaitForNextAdditionProcess(arg);
		// }
		public IHeatLevelUpProcess CreateHeatLevelUpProcess(
			IHeatManager heatManager,
			float targetMaxHeat,
			float smoothTime
		){
			HeatLevelUpProcess.IConstArg arg = new HeatLevelUpProcess.ConstArg(
				thisProcessManager,
				heatManager,
				targetMaxHeat,
				smoothTime
			);
			return new HeatLevelUpProcess(arg);
		}
		public IArrowTrailFadeProcess CreateArrowTrailFadeProcess(
			float fadeTime,
			IArrowTrail trail,
			float initialAlpha
		){
			ArrowTrailFadeProcess.IConstArg arg = new ArrowTrailFadeProcess.ConstArg(
				thisProcessManager,
				fadeTime,
				trail,
				initialAlpha
			);
			return new ArrowTrailFadeProcess(arg);
		}
		public IWaypointsFollowerChangeSpeedProcess CreateWaypointsFollowerChangeSpeedProcess(
			IFollowWaypointProcess followProcess,
			float time,
			AnimationCurve speedCurve
		){
			WaypointsFollowerChangeSpeedProcess.IConstArg arg= new WaypointsFollowerChangeSpeedProcess.ConstArg(
				thisProcessManager,
				time,
				followProcess,
				speedCurve
			);
			return new WaypointsFollowerChangeSpeedProcess(arg);
		}
		public IGameplayUnpauseProcess CreateGameplayUnpauseProcess(
			IGameplayPause gameplayPause,
			float time
		){
			GameplayUnpauseProcess.IConstArg arg = new GameplayUnpauseProcess.ConstArg(
				thisProcessManager,
				time,
				gameplayPause
			);
			return new GameplayUnpauseProcess(arg);
		}
		public ICriticalFlashProcess CreateCriticalFlashProcess(
			ICriticalFlash flash,
			AnimationCurve flashCurve,
			float flashTime
		){
			CriticalFlashProcess.IConstArg arg = new CriticalFlashProcess.ConstArg(
				thisProcessManager,
				flashTime,
				flash,
				flashCurve
			);
			return new CriticalFlashProcess(arg);
		}
	}
}
