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

		IShootingManager thisShootingManager;
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
			thisShootingManager = new ShootingManager(arg);
		}
		public IShootingManager GetShootingManager(){
			return thisShootingManager;
		}
		public PlayerInputManagerAdaptor inputManagerAdaptor;
		public LaunchPointAdaptor launchPointAdaptor;
		public TrajectoryAdaptor trajectoryAdaptor;
		public LandedArrowReserveAdaptor landedArrowReserveAdaptor;
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

			SetUpArrows();
		}
		public int arrowCount = 20;
		public Transform arrowReserveTrans;
		public GameObject arrowPrefab;
		public int collisionDetectionIntervalFrameCount = 3;
		public float arrowAttack = 100f;
		void SetUpArrows(){
			IArrow[] arrows = new IArrow[arrowCount];
			IArrowAdaptor[] arrowAdaptors = new IArrowAdaptor[arrowCount];
			for(int i = 0; i < arrowCount; i++){
				GameObject arrowGO = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity, arrowReserveTrans);
				IArrowAdaptor arrowAdaptor = arrowGO.GetComponent(typeof(IArrowAdaptor)) as IArrowAdaptor;
				if(arrowAdaptor == null)
					throw new System.InvalidOperationException(
						"eh?"
					);
				arrowAdaptor.SetMonoBehaviourAdaptorManager(
					thisMonoBehaviourAdaptorManager
				);
				arrowAdaptor.SetLaunchPointAdaptor(launchPointAdaptor);
				arrowAdaptor.SetArrowReserveTransform(arrowReserveTrans);
				arrowAdaptor.SetIndex(i);
				arrowAdaptor.SetCollisionDetectionIntervalFrameCount(collisionDetectionIntervalFrameCount);
				arrowAdaptor.SetUp();
				IArrow arrow = arrowAdaptor.GetArrow();
				arrows[i] = arrow;
				arrowAdaptors[i] = arrowAdaptor;

				ILaunchPoint launchPoint = launchPointAdaptor.GetLaunchPoint();

				arrow.SetLaunchPoint(launchPoint);
			}
			
			IShootingManager shootingManager = GetShootingManager();
			shootingManager.SetArrows(arrows);

			foreach(IArrow arrow in arrows)
				arrow.ResetArrow();
		}
	}
}
