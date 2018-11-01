using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IShootingManagerAdaptor: IMonoBehaviourAdaptor{
		IShootingManager GetShootingManager();
		float GetInitialSpeed();
		float GetMaxFlightSpeed();
		float GetGravity();
		float GetMaxDrawTime();
	}
	public class ShootingManagerAdaptor : MonoBehaviourAdaptor, IShootingManagerAdaptor{

		public int drawProcessOrder;
		public float fireRate = 1f;

		public AnimationCurve drawStrengthCurve;
		public float globalMinDrawStrength;
		public float globalMaxDrawStrength;

		public float globalMinArrowAttack;
		public float arrowAttackMultiplier;
		public float globalMinFlightSpeed;
		public float flightSpeedMultiplier;

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

				drawStrengthCurve,
				globalMinDrawStrength,
				globalMaxDrawStrength,

				globalMinArrowAttack,
				arrowAttackMultiplier,
				globalMinFlightSpeed,
				flightSpeedMultiplier
			);
			return new ShootingManager(arg);
		}
		public PlayerInputManagerAdaptor inputManagerAdaptor;
		public LaunchPointAdaptor launchPointAdaptor;
		public TrajectoryAdaptor trajectoryAdaptor;
		public LandedArrowReserveAdaptor landedArrowReserveAdaptor;
		public ArrowReserveAdaptor arrowReserveAdaptor;
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
		}
	}
}
