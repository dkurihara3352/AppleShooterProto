using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IArrow: IAppleShooterSceneObject, IArrowStateHandler, IArrowStateImplementor{
		void SetLaunchPoint(ILaunchPoint launchPoint);
		void SetShootingManager(IShootingManager shootingManager);
		void SetArrowReserve(IArrowReserve reserve);

		bool IsInFlight();
		bool IsActivated();
		bool IsInReserve();
		void BecomeChildToReserve();
		void Land(
			IShootingTarget target,
			Vector3 hitPosition
		);
		void StartFlight();
		void StopFlight();
		void StartCollisionCheck();
		void StopColllisionCheck();

		float GetAttack();
		void SetAttack(float attack);

		int GetIndex();
		ArrowStateEngine.IState GetCurrentState();
		void SetLookRotation(Vector3 forward);
		string GetParentName();

		void SetArrowTrail(IArrowTrail trail);
		void CheckAndClearArrowTrail(IArrowTrail trail);
	}
	public class Arrow : AppleShooterSceneObject, IArrow{
		/* Setup */
			public Arrow(
				IConstArg arg
			):base(
				arg
			){
				thisIndex = arg.index;

				ArrowStateEngine.IConstArg stateEngineArg = new ArrowStateEngine.ConstArg(
					this
				);
				thisStateEngine = new ArrowStateEngine(stateEngineArg);
			}
			IArrowAdaptor thisTypedAdaptor{
				get{
					return (IArrowAdaptor)thisAdaptor;
				}
			}
			readonly IArrowStateEngine thisStateEngine;
			IShootingManager thisShootingManager;
			public void SetShootingManager(IShootingManager shootingManager){
				thisShootingManager = shootingManager;
			}
			ILaunchPoint thisLaunchPoint;
			public void SetLaunchPoint(ILaunchPoint launchPoint){
				thisLaunchPoint = launchPoint;
			}
			IArrowReserve thisArrowReserve;
			public void SetArrowReserve(IArrowReserve reserve){
				thisArrowReserve = reserve;
			}
			public void BecomeChildToReserve(){
				SetParent(thisArrowReserve);
			}
			public bool IsInFlight(){
				return thisStateEngine.IsInFlight();
			}
			public bool IsActivated(){
				return thisStateEngine.IsActivated();
			}
			public bool IsInReserve(){
				return thisStateEngine.IsInReserve();
			}
		/* Engine delegate */
			public void Nock(){
				thisStateEngine.Nock();
			}
			public void Release(){
				thisStateEngine.Release();
			}
			public void Deactivate(){
				thisStateEngine.Deactivate();
			}
		/* action */
			public void NockImple(){
				thisShootingManager.SetNockedArrow(this);
				MoveToLaunchPosition();
			}
			void MoveToLaunchPosition(){
				SetParent(thisLaunchPoint);
				ResetLocalTransform();
			}
			public void ReleaseImple(){
				thisShootingManager.RegisterShot(this);
			}
			public void DeactivateImple(){
				DetachTrail();

				StopFlight();
				SetAttack(0f);
				thisArrowReserve.Reserve(this);
				thisShootingManager.CheckAndClearNockedArrow(this);

			}
			IArrowTrail thisTrail;
			public void SetArrowTrail(IArrowTrail trail){
				thisTrail = trail;
			}
			void DetachTrail(){
				if(thisTrail != null)
					thisTrail.Detach();
				thisTrail = null;
			}
			public void CheckAndClearArrowTrail(IArrowTrail trail){
				if(thisTrail != null && thisTrail == trail)
					thisTrail = null;
			}
			public void TryRegisterShot(){
				if(thisShootingManager.AcceptsNewShot())
					thisShootingManager.RegisterShot(this);
				else
					Deactivate();
			}
		/* Flight */
			IArrowFlightProcess thisFlightProcess;
			public void StartFlight(){
				StopFlight();
				thisFlightProcess = thisAppleShooterProcessFactory.CreateArrowFlightProcess(
					this,
					thisShootingManager.GetFlightSpeed(),
					thisShootingManager.GetFlightDirection(),
					thisShootingManager.GetFlightGravity(),
					thisShootingManager.GetLauncherVelocity(),
					thisLaunchPoint.GetPosition()
				);
				thisFlightProcess.Run();
			}
			public void StopFlight(){
				if(thisFlightProcess != null)
					if(thisFlightProcess.IsRunning())
						thisFlightProcess.Stop();
				thisFlightProcess = null;
			}
			public void Land(
				IShootingTarget target,
				Vector3 hitPosition
			){
				if(target.IsActivated())
					thisShootingManager.SpawnLandedArrowOn(
						target,
						hitPosition,
						thisAdaptor.GetRotation()
					);
				Deactivate();
			}
			public void StartCollisionCheck(){
				thisTypedAdaptor.StartCollisionCheck();
			}
			public void StopColllisionCheck(){
				thisTypedAdaptor.StopCollisionCheck();
			}
		/* Debug */
			readonly int thisIndex;
			public int GetIndex(){
				return thisIndex;
			}
			public ArrowStateEngine.IState GetCurrentState(){
				return thisStateEngine.GetCurrentState();
			}
			public void SetLookRotation(Vector3 forward){
				thisAdaptor.SetLookRotation(forward);
			}
			public string GetParentName(){
				return thisTypedAdaptor.GetParentName();
			}
		/*  */
			float thisAttack;
			public float GetAttack(){
				return thisAttack;
			}
			public void SetAttack(float attack){
				thisAttack = attack;
			}
		/* Const */
			public new interface IConstArg: AppleShooterSceneObject.IConstArg{
				int index{get;}
			}
			public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
				public ConstArg(
					int index,
					IArrowAdaptor adaptor
				): base(
					adaptor
				){
					thisIndex = index;
				}
				readonly int thisIndex;
				public int index{get{return thisIndex;}}
			}
		/*  */
	}



}
