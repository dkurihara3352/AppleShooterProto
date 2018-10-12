using System.Collections;
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
		void Land(
			IShootingTarget target,
			Vector3 hitPosition
		);

		void StartFlight();
		void BecomeChildToReserve();
		void StopFlight();
		void StartCollisionCheck();
		void StopColllisionCheck();

		int GetIndex();
		IArrowState GetCurrentState();
		int GetIDInReserve();
		Vector3 GetPosition();
		void SetPosition(Vector3 position);
		void SetLookRotation(Vector3 forward);
		string GetParentName();
		int GetFlightID();

		float GetAttack();
	}
	public class Arrow : IArrow {
		/* Setup */
			public Arrow(IConstArg arg){

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
				thisAttack = arg.attack;
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
				thisShootingManager.RemoveArrowFromFlight(this);
				MoveToLaunchPosition();
			}
			void MoveToLaunchPosition(){
				thisAdaptor.BecomeChildToLaunchPoint();
				thisAdaptor.ResetLocalTransform();
			}
			public void ResetArrow(){
				thisShootingManager.AddArrowToReserve(this);
				thisShootingManager.RemoveArrowFromFlight(this);
				thisShootingManager.CheckAndClearNockedArrow(this);
				MoveToReservePosition();
				thisAdaptor.SetRotation(Quaternion.identity);
				CheckAndStopFlightProcess();
			}
			void CheckAndStopFlightProcess(){
				if(thisFlightProcess != null && thisFlightProcess.IsRunning()){
					thisFlightProcess.Stop();
					thisFlightProcess = null;
				}
			}
			void MoveToReservePosition(){
				thisAdaptor.BecomeChildToReserve();
				thisAdaptor.ResetLocalTransform();
			}
			public void TryRegisterShot(){
				if(thisShootingManager.AcceptsNewShot())
					thisStateEngine.SwitchToShotState();
				else
					TryResetArrow();
			}
			public void Fire(){
				thisShootingManager.AddArrowToFlight(this);
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
			public void Land(
				IShootingTarget target,
				Vector3 hitPosition
			){
				thisShootingManager.SpawnLandedArrowOn(
					target,
					hitPosition,
					thisAdaptor.GetRotation()
				);
				TryResetArrow();
			}
			public void BecomeChildToReserve(){
				thisAdaptor.BecomeChildToReserve();
			}
			public void StartCollisionCheck(){
				thisAdaptor.StartCollisionCheck();
			}
			public void StopColllisionCheck(){
				thisAdaptor.StopCollisionCheck();
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
			public void SetLookRotation(Vector3 forward){
				thisAdaptor.SetLookRotation(forward);
			}
			public string GetParentName(){
				return thisAdaptor.GetParentName();
			}
			public int GetFlightID(){
				return thisShootingManager.GetFlightID(this);
			}
		/*  */
			float thisAttack;
			public float GetAttack(){
				return thisAttack;
			}
		/* Const */
			public interface IConstArg{
				IArrowAdaptor adaptor{get;}
				IAppleShooterProcessFactory processFactory{get;}
				int index{get;}
				float attack{get;}
			}
			public struct ConstArg: IConstArg{
				public ConstArg(
					IArrowAdaptor adaptor,
					IAppleShooterProcessFactory processFactory,
					int index,
					float attack
				){
					thisAdaptor = adaptor;
					thisProcessFactory = processFactory;
					thisIndex = index;
					thisAttack = attack;
				}
				readonly IArrowAdaptor thisAdaptor;
				public IArrowAdaptor adaptor{get{return thisAdaptor;}}
				readonly IAppleShooterProcessFactory thisProcessFactory;
				public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
				readonly int thisIndex;
				public int index{get{return thisIndex;}}
				readonly float thisAttack;
				public float attack{get{return thisAttack;}}
			}
		/*  */
	}



}
