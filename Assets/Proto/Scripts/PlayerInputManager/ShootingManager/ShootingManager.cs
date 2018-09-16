using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingManager{
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
			thisInputManager = arg.inputManager;
			thisInputManager.SetMaxZoom(thisMaxZoom);
			thisProcessFactory = arg.processFactory;
		}
		readonly IPlayerInputManager thisInputManager;
		float thisMaxZoom = 20f;
		float thisMaxDrawTime = 3f;
		float thisDrawElapsedTime = 0f;
		IDrawProcess thisDrawProcess;
		IAppleShooterProcessFactory thisProcessFactory;

		public void StartDraw(){
			thisDrawProcess = thisProcessFactory.CreateDrawProcess(this);
			thisDrawProcess.Run();
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
		IPlayerInputManager inputManager{get;}
		IAppleShooterProcessFactory processFactory{get;}
	}
	public class ShootingManagerConstArg: IShootingManagerConstArg{
		public ShootingManagerConstArg(
			IPlayerInputManager inputManager,
			IAppleShooterProcessFactory processFactory
		){
			thisInputManager = inputManager;
			thisProcessFactory = processFactory;
		}
		readonly IPlayerInputManager thisInputManager;
		public IPlayerInputManager inputManager{get{return thisInputManager;}}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
	}
}
