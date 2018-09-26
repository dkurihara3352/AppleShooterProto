﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IArrow{
		void SetLaunchPoint(ILaunchPoint launchPoint);
		void SetShootingManager(IShootingManager shootingManager);

		void TryNock();
		void TryFire();
		void TryResetArrow();

		void Nock();
		void ResetArrow();
		void TryRegisterShot();
		void Fire();

		void StartFlight();
		void BecomeChildToReserve();
		void StopFlight();

		int GetIndex();
		IArrowState GetCurrentState();
		int GetIDInReserve();
		Vector3 GetPosition();
		void SetPosition(Vector3 position);
		string GetParentName();
	}
	public class Arrow : IArrow {
		/* Setup */
			public Arrow(IArrowConstArg arg){

				thisAdaptor = arg.adaptor;
				thisProcessFactory = arg.processFactory;
				thisIndex = arg.index;

				IArrowStateEngineConstArg stateEngineConstArg = new ArrowStateEngineConstArg(
					this,
					arg.processFactory
				);
				thisStateEngine = new ArrowStateEngine(
					stateEngineConstArg
				);
			}
			readonly IArrowAdaptor thisAdaptor;
			readonly IAppleShooterProcessFactory thisProcessFactory;
			readonly IArrowStateEngine thisStateEngine;
			IShootingManager thisShootingManager;
			public void SetShootingManager(IShootingManager shootingManager){
				thisShootingManager = shootingManager;
			}
			ILaunchPoint thisLaunchPoint;
			public void SetLaunchPoint(ILaunchPoint launchPoint){
				thisLaunchPoint = launchPoint;
				thisStateEngine.SetLaunchPoint(launchPoint);
			}
		/* Engine delegate */
			public void TryNock(){
				thisStateEngine.TryNock();
			}
			public void TryFire(){
				thisStateEngine.TryFire();
			}
			public void TryResetArrow(){
				thisStateEngine.TryResetArrow();
			}
		/* action */
			public void Nock(){
				thisShootingManager.SetNockedArrow(this);
				thisShootingManager.RemoveArrowFromReserve(this);
				MoveToLaunchPosition();
			}
			void MoveToLaunchPosition(){
				thisAdaptor.BecomeChildToLaunchPoint();
				thisAdaptor.ResetTransform();
			}
			public void ResetArrow(){
				thisShootingManager.AddArrowToReserve(this);
				thisShootingManager.CheckAndClearNockedArrow(this);
				MoveToReservePosition();
			}
			void MoveToReservePosition(){
				thisAdaptor.BecomeChildToReserve();
				thisAdaptor.ResetTransform();
			}
			public void TryRegisterShot(){
				if(thisShootingManager.AcceptsNewShot())
					thisStateEngine.SwitchToShotState();
				else
					TryResetArrow();
			}
			public void Fire(){
				thisShootingManager.AddArrowToReserve(this);
				thisShootingManager.RegisterShot(this);
			}
		/* Flight */
			IArrowFlightProcess thisFlightProcess;
			public void StartFlight(){
				thisFlightProcess = thisProcessFactory.CreateArrowFlightProcess(
					this,
					thisShootingManager.GetFlightSpeed(),
					thisShootingManager.GetFlightDirection(),
					thisShootingManager.GetFlightGravity(),
					thisShootingManager.GetLauncherVelocity(),
					thisLaunchPoint.GetWorldPosition()
				);
				thisFlightProcess.Run();
			}
			public void StopFlight(){
				if(thisFlightProcess != null)
					if(thisFlightProcess.IsRunning())
						thisFlightProcess.Stop();
			}
			public void BecomeChildToReserve(){
				thisAdaptor.BecomeChildToReserve();
			}
		/* Debug */
			readonly int thisIndex;
			public int GetIndex(){
				return thisIndex;
			}
			public IArrowState GetCurrentState(){
				return thisStateEngine.GetCurrentState();
			}
			public int GetIDInReserve(){
				IArrowState currentState = this.GetCurrentState();
				if(!(currentState is IArrowNockedState))
					return thisShootingManager.GetArrowReserveID(this);
				else return -1;
			}
			public Vector3 GetPosition(){
				return thisAdaptor.GetPosition();
			}
			public void SetPosition(Vector3 position){
				thisAdaptor.SetPosition(position);
			}
			public string GetParentName(){
				return thisAdaptor.GetParentName();
			}
		/*  */
	}



	public interface IArrowConstArg{
		IArrowAdaptor adaptor{get;}
		IAppleShooterProcessFactory processFactory{get;}
		int index{get;}
	}
	public struct ArrowConstArg: IArrowConstArg{
		public ArrowConstArg(
			IArrowAdaptor adaptor,
			IAppleShooterProcessFactory processFactory,
			int index
		){
			thisAdaptor = adaptor;
			thisProcessFactory = processFactory;
			thisIndex = index;
		}
		readonly IArrowAdaptor thisAdaptor;
		public IArrowAdaptor adaptor{get{return thisAdaptor;}}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
		readonly int thisIndex;
		public int index{get{return thisIndex;}}
	}
}
