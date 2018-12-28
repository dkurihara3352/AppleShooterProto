using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IShootingManagerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IShootingManager GetShootingManager();
		float GetGravity();
		AnimationCurve GetBowDrawProfileCurve();
		float GetGlobalMinDrawStrength();
		float GetGlobalMaxDrawStrength();
		float GetGlobalMinAttack();
		float GetGlobalMaxAttack();
		float GetGlobalMinFlightSpeed();
		float GetGlobalMaxFlightSpeed();

		Animator GetAimAnimator();
		int GetAimHash();

		void PlayDrawSound();
		void StopDrawSound();
		void PauseDrawSound();
	}
	public class ShootingManagerAdaptor : AppleShooterMonoBehaviourAdaptor, IShootingManagerAdaptor{

		public int drawProcessOrder;

		public AnimationCurve bowDrawProfileCurve;
		public AnimationCurve GetBowDrawProfileCurve(){
			return bowDrawProfileCurve;
		}

		public float globalMinDrawStrength;
		public float GetGlobalMinDrawStrength(){
			return globalMinDrawStrength;
		}

		public float globalMaxDrawStrength;
		public float GetGlobalMaxDrawStrength(){
			return globalMaxDrawStrength;
		}

		public float globalMinArrowAttack;
		public float GetGlobalMinAttack(){
			return globalMinArrowAttack;
		}
		public float globalMaxArrowAttack;
		public float GetGlobalMaxAttack(){
			return globalMaxArrowAttack;
		}
		public float globalMinFlightSpeed;
		public float GetGlobalMinFlightSpeed(){
			return globalMinFlightSpeed;
		}
		public float globalMaxFlightSpeed;
		public float GetGlobalMaxFlightSpeed(){
			return globalMaxFlightSpeed;
		}

		public float arrowFlightTime = 4f;
		public override void SetUp(){
			thisShootingManager = CreateShootingManager();
			thisAimHash = Animator.StringToHash("aimBool");
		}
		int thisAimHash;
		public int GetAimHash(){
			return thisAimHash;
		}
		public Animator aimAnimator;
		public Animator GetAimAnimator(){
			return aimAnimator;
		}
		IShootingManager thisShootingManager;
		public IShootingManager GetShootingManager(){
			return thisShootingManager;
		}
		protected virtual IShootingManager CreateShootingManager(){
			ShootingManager.IConstArg arg = new ShootingManager.ConstArg(
				this,
				drawProcessOrder,

				bowDrawProfileCurve,
				
				globalMinDrawStrength,
				globalMaxDrawStrength,

				globalMinArrowAttack,
				globalMaxArrowAttack,
				globalMinFlightSpeed,
				globalMaxFlightSpeed,

				arrowFlightTime
			);
			return new ShootingManager(arg);
		}
		public PlayerInputManagerAdaptor inputManagerAdaptor;
		public LaunchPointAdaptor launchPointAdaptor;
		public TrajectoryAdaptor trajectoryAdaptor;
		public LandedArrowReserveAdaptor landedArrowReserveAdaptor;
		public ArrowReserveAdaptor arrowReserveAdaptor;
		public ArrowTrailReserveAdaptor arrowTrailReserveAdaptor;
		public CriticalFlashAdaptor criticalFlashAdaptor;
		public ShootingDataManagerAdaptor shootingDataManagerAdaptor;
		// public float initialFlightSpeed;
		// public float GetInitialSpeed(){
		// 	return initialFlightSpeed;
		// }
		// public float maxFlightSpeed;
		// public float GetMaxFlightSpeed(){
		// 	return maxFlightSpeed;
		// }
		public float shotGravity;
		public float GetGravity(){
			return shotGravity;
		}
		public override void SetUpReference(){
			IPlayerInputManager inputManager = inputManagerAdaptor.GetInputManager();
			thisShootingManager.SetInputManager(inputManager);

			ILaunchPoint launchPoint = launchPointAdaptor.GetLaunchPoint();
			thisShootingManager.SetLaunchPoint(launchPoint);

			ITrajectory trajectory = trajectoryAdaptor.GetTrajectory();
			thisShootingManager.SetTrajectory(trajectory);

			ILandedArrowReserve landedArrowReserve = landedArrowReserveAdaptor.GetLandedArrowReserve();
			thisShootingManager.SetLandedArrowReserve(landedArrowReserve);

			IArrowReserve arrowReserve = arrowReserveAdaptor.GetArrowReserve();
			thisShootingManager.SetArrowReserve(arrowReserve);

			IArrowTrailReserve arrowTrailReserve = arrowTrailReserveAdaptor.GetArrowTrailReserve();
			thisShootingManager.SetArrowTrailReserve(arrowTrailReserve);

			ICriticalFlash flash = criticalFlashAdaptor.GetCriticalFlash();
			thisShootingManager.SetCriticalFlash(flash);

			IShootingDataManager shootingDataManager = shootingDataManagerAdaptor.GetShootingDataManager();
			thisShootingManager.SetShootingDataManager(shootingDataManager);
		}
		public void PlayDrawSound(){
			// StopDrawSound();
			drawSoundSource.Play(0);
		}
		public void StopDrawSound(){
			if(drawSoundSource.isPlaying)
				drawSoundSource.Stop();
		}
		public void PauseDrawSound(){
			drawSoundSource.Pause();
		}
		public AudioSource drawSoundSource;
	}
}
