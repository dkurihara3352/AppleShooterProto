using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IShootingManagerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IShootingManager GetShootingManager();
		float GetInitialSpeed();
		float GetMaxFlightSpeed();
		float GetGravity();
		float GetMaxDrawTime();
	}
	public class ShootingManagerAdaptor : AppleShooterMonoBehaviourAdaptor, IShootingManagerAdaptor{

		public int drawProcessOrder;
		public float fireRate = 1f;

		public AnimationCurve bowDrawProfileCurve;
		public float bowMinDrawStrength;
		public float bowMaxDrawStrength;


		public float globalMinDrawStrength;
		public float globalMaxDrawStrength;

		public float globalMinArrowAttack;
		public float globalMaxArrowAttack;
		public float globalMinFlightSpeed;
		public float globalMaxFlightSpeed;

		public float arrowFlightTime = 4f;
		public override void SetUp(){
			thisShootingManager = CreateShootingManager();
		}
		IShootingManager thisShootingManager;
		public IShootingManager GetShootingManager(){
			return thisShootingManager;
		}
		protected virtual IShootingManager CreateShootingManager(){
			ShootingManager.IConstArg arg = new ShootingManager.ConstArg(
				this,
				drawProcessOrder,
				fireRate,

				bowDrawProfileCurve,
				bowMinDrawStrength,
				bowMaxDrawStrength,

				
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
		public float initialFlightSpeed;
		public float GetInitialSpeed(){
			return initialFlightSpeed;
		}
		public float maxFlightSpeed;
		public float GetMaxFlightSpeed(){
			return maxFlightSpeed;
		}
		public float maxDrawTime;
		public float GetMaxDrawTime(){
			return maxDrawTime;
		}
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
		}
	}
}
