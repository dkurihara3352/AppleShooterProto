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
		public override void SetUp(){
			IAppleShooterProcessFactory processFactory = new AppleShooterProcessFactory(processManager);
			IShootingManagerConstArg arg = new ShootingManagerConstArg(
				processFactory,
				this,
				drawProcessOrder
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
		}
	}
}
