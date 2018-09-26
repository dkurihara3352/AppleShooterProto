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
		public ProcessManager processManager;
		public int drawProcessOrder;
		public float fireRate = 1f;
		public override void SetUp(){
			IAppleShooterProcessFactory processFactory = new AppleShooterProcessFactory(processManager);
			IShootingManagerConstArg arg = new ShootingManagerConstArg(
				processFactory,
				this,
				drawProcessOrder,
				fireRate
			);
			thisShootingManager = new ShootingManager(arg);
		}
		public IShootingManager GetShootingManager(){
			return thisShootingManager;
		}
		public PlayerInputManagerAdaptor inputManagerAdaptor;
		public LaunchPointAdaptor launchPointAdaptor;
		public TrajectoryAdaptor trajectoryAdaptor;
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

			SetUpArrows();
		}
		public int arrowCount = 20;
		public MonoBehaviourAdaptor arrowReserve;
		void SetUpArrows(){
			IArrow[] arrows = new IArrow[arrowCount];
			IArrowAdaptor[] arrowAdaptors = new IArrowAdaptor[arrowCount];
			for(int i = 0; i < arrowCount; i++){
				GameObject arrowGO = new GameObject("arrowGO");
				arrowGO.transform.parent = arrowReserve.GetTransform();
				arrowGO.transform.position = Vector3.zero;
				arrowGO.transform.rotation = Quaternion.identity;

				IArrowAdaptor arrowAdaptor = arrowGO.AddComponent<ArrowAdaptor>();
				arrowAdaptor.SetProcessManager(processManager);
				arrowAdaptor.SetLaunchPointAdaptor(launchPointAdaptor);
				arrowAdaptor.SetIndex(i);
				arrowAdaptor.SetArrowReserve(arrowReserve);
				arrowAdaptor.SetUp();
				IArrow arrow = arrowAdaptor.GetArrow();
				arrows[i] = arrow;
				arrowAdaptors[i] = arrowAdaptor;

				ILaunchPoint launchPoint = launchPointAdaptor.GetLaunchPoint();

				arrow.SetLaunchPoint(launchPoint);
			}
			
			IShootingManager shootingManager = GetShootingManager();
			shootingManager.SetArrows(arrows);
		}
	}
}
