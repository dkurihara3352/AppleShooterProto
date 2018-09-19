using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IShootingManagerAdaptor: IMonoBehaviourAdaptor{
		IShootingManager GetShootingManager();
		float GetInitialSpeed();
		float GetGravity();
	}
	public class ShootingManagerAdaptor : MonoBehaviourAdaptor, IShootingManagerAdaptor{

		IShootingManager thisShootingManager;
		public ProcessManager processManager;
		public override void SetUp(){
			IAppleShooterProcessFactory processFactory = new AppleShooterProcessFactory(processManager);
			IShootingManagerConstArg arg = new ShootingManagerConstArg(
				processFactory,
				this
			);
			thisShootingManager = new ShootingManager(arg);
		}
		public IShootingManager GetShootingManager(){
			return thisShootingManager;
		}
		public PlayerInputManagerAdaptor inputManagerAdaptor;
		public LaunchPointAdaptor launchPointAdaptor;
		public TrajectoryAdaptor trajectoryAdaptor;
		public float shotInitialSpeed;
		public float GetInitialSpeed(){
			return shotInitialSpeed;
		}
		public float shotGravity;
		public float GetGravity(){
			return shotGravity;
		}
		public override void SetUpReference(){
			IPlayerInputManager inputManager = inputManagerAdaptor.GetInputManager();
			thisShootingManager.SetInputManager(inputManager);

			inputManager.SetMaxZoom(thisShootingManager.GetMaxZoom());

			ILaunchPoint launchPoint = launchPointAdaptor.GetLaunchPoint();
			thisShootingManager.SetLaunchPoint(launchPoint);

			ITrajectory trajectory = trajectoryAdaptor.GetTrajectory();
			thisShootingManager.SetTrajectory(trajectory);
		}
	}
}
