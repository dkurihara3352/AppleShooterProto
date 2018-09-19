using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingManager{
		void SetInputManager(IPlayerInputManager inputManager);
		void SetLaunchPoint(ILaunchPoint launchPoint);
		void SetTrajectory(ITrajectory trajectory);
		
		void StartDraw();
		void Draw(float deltaTime);
		void HoldDraw();
		void Fire();
		void ResetDraw();
		float GetMaxZoom();
	}
	public class ShootingManager : IShootingManager {

		public ShootingManager(
			IShootingManagerConstArg arg
		){
			thisProcessFactory = arg.processFactory;
			thisAdaptor = arg.adaptor;
		}
		IPlayerInputManager thisInputManager;
		public void SetInputManager(IPlayerInputManager inputManager){
			thisInputManager = inputManager;
		}
		public float GetMaxZoom(){
			return thisMaxZoom;
		}
		float thisMaxZoom = 20f;
		float thisMaxDrawTime = 3f;
		float thisDrawElapsedTime = 0f;
		IDrawProcess thisDrawProcess;
		readonly IAppleShooterProcessFactory thisProcessFactory;
		readonly IShootingManagerAdaptor thisAdaptor;
		ITrajectory thisTrajectory;
		public void SetTrajectory(ITrajectory trajectory){
			thisTrajectory = trajectory;
		}

		public void StartDraw(){
			thisDrawProcess = thisProcessFactory.CreateDrawProcess(this);
			thisDrawProcess.Run();
		}
		public float initialSpeed{
			get{return thisAdaptor.GetInitialSpeed();}
		}
		public float gravity{
			get{return thisAdaptor.GetGravity();}
		}
		public void HoldDraw(){
			if(thisDrawProcess != null)
				if(thisDrawProcess.IsRunning()){
					thisDrawProcess.Stop();
					thisDrawProcess = null;
				}
		}
		public void Draw(float deltaTime){
			if(thisDrawElapsedTime < thisMaxDrawTime){
				thisDrawElapsedTime += deltaTime;
				float normalizedDrawTime = thisDrawElapsedTime/ thisMaxDrawTime;
				if(normalizedDrawTime > 1f)
					normalizedDrawTime = 1f;
				thisInputManager.Zoom(normalizedDrawTime);
			}
			DrawTrajectory();
		}
		ILaunchPoint thisLaunchPoint;
		public void SetLaunchPoint(ILaunchPoint launchPoint){
			thisLaunchPoint = launchPoint;
		}
		void DrawTrajectory(){
			Vector3 aimDirection = thisLaunchPoint.GetWorldDirection();
			thisTrajectory.DrawTrajectory(
				aimDirection,
				initialSpeed,
				gravity,
				thisLaunchPoint.GetWorldPosition()
			);
		}
		public void Fire(){
			ResetDraw();
		}
		public void ResetDraw(){
			HoldDraw();
			thisDrawElapsedTime = 0f;
		}
	}


	public interface IShootingManagerConstArg{
		IAppleShooterProcessFactory processFactory{get;}
		IShootingManagerAdaptor adaptor{get;}
	}
	public class ShootingManagerConstArg: IShootingManagerConstArg{
		public ShootingManagerConstArg(
			IAppleShooterProcessFactory processFactory,
			IShootingManagerAdaptor adaptor
		){
			thisProcessFactory = processFactory;
			thisAdaptor = adaptor;
		}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
		readonly IShootingManagerAdaptor thisAdaptor;
		public IShootingManagerAdaptor adaptor{get{return thisAdaptor;}}
	}
}
