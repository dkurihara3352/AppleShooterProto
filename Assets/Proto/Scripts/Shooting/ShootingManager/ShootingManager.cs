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
	}
	public class ShootingManager : IShootingManager {

		public ShootingManager(
			IShootingManagerConstArg arg
		){
			thisProcessFactory = arg.processFactory;
			thisAdaptor = arg.adaptor;
			thisDrawProcessOrder = arg.drawProcessOrder;
		}
		IPlayerInputManager thisInputManager;
		public void SetInputManager(IPlayerInputManager inputManager){
			thisInputManager = inputManager;
		}
		float maxZoom{
			get{return thisInputManager.GetMaxZoom();}
		}
		float thisDrawElapsedTime = 0f;
		float thisMaxDrawTime{
			get{return thisAdaptor.GetMaxDrawTime();}
		}
		float GetNormalizedDraw(){
			float result = thisDrawElapsedTime/ thisMaxDrawTime;
			if(result > 1f)
				result = 1f;
			return result;
		}
		IDrawProcess thisDrawProcess;
		readonly IAppleShooterProcessFactory thisProcessFactory;
		readonly IShootingManagerAdaptor thisAdaptor;
		ITrajectory thisTrajectory;
		public void SetTrajectory(ITrajectory trajectory){
			thisTrajectory = trajectory;
		}
		readonly int thisDrawProcessOrder;
		public void StartDraw(){
			thisDrawProcess = thisProcessFactory.CreateDrawProcess(
				this,
				thisDrawProcessOrder
			);
			thisDrawProcess.Run();
		}
		public float initialSpeed{
			get{return thisAdaptor.GetInitialSpeed();}
		}
		public float maxFlightSpeed{
			get{return thisAdaptor.GetMaxFlightSpeed();}
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
				float normalizedDraw = GetNormalizedDraw();
				thisInputManager.Zoom(normalizedDraw);
				this.UpdateFlightSpeed();
			}
			DrawTrajectory();
		}
		ILaunchPoint thisLaunchPoint;
		public void SetLaunchPoint(ILaunchPoint launchPoint){
			thisLaunchPoint = launchPoint;
		}
		void UpdateFlightSpeed(){
			thisFlightSpeed = CalcFlightSpeed(
				GetNormalizedDraw()
			);
		}
		float thisFlightSpeed;
		float CalcFlightSpeed(
			float normalizedDraw
		){
			return Mathf.Lerp(
				initialSpeed,
				maxFlightSpeed,
				normalizedDraw
			);
		}
		void DrawTrajectory(){
			Vector3 aimDirection = thisLaunchPoint.GetWorldDirection();
			thisTrajectory.DrawTrajectory(
				aimDirection,
				thisFlightSpeed,
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
			UpdateFlightSpeed();
		}
	}


	public interface IShootingManagerConstArg{
		IAppleShooterProcessFactory processFactory{get;}
		IShootingManagerAdaptor adaptor{get;}
		int drawProcessOrder{get;}
	}
	public class ShootingManagerConstArg: IShootingManagerConstArg{
		public ShootingManagerConstArg(
			IAppleShooterProcessFactory processFactory,
			IShootingManagerAdaptor adaptor,
			int drawProcessOrder
		){
			thisProcessFactory = processFactory;
			thisAdaptor = adaptor;
			thisDrawProcessOrder = drawProcessOrder;
		}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
		readonly IShootingManagerAdaptor thisAdaptor;
		public IShootingManagerAdaptor adaptor{get{return thisAdaptor;}}
		readonly int thisDrawProcessOrder;
		public int drawProcessOrder{get{return thisDrawProcessOrder;}}
	}
}
