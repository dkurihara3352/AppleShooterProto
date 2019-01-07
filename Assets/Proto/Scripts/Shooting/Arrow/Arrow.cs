using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IArrow: ISlickBowShootingSceneObject, IArrowStateHandler, IArrowStateImplementor{
		void SetLaunchPoint(ILaunchPoint launchPoint);
		void SetShootingManager(IShootingManager shootingManager);
		void SetArrowReserve(IArrowReserve reserve);
		void SetArrowTrailReserve(IArrowTrailReserve reserve);

		bool IsInFlight();
		bool IsActivated();
		bool IsInReserve();
		void BecomeChildToReserve();
		void Land(
			// IShootingTarget target,
			IArrowHitDetector detector,
			Vector3 hitPosition
		);
		void StartFlight();
		void StopFlight();
		void StartCollisionCheck();
		void StopColllisionCheck();

		float GetAttack();
		void SetAttack(float attack);

		void SetNormalizedDraw(float drawValue);

		int GetIndex();
		ArrowStateEngine.IState GetCurrentState();
		void SetLookRotation(Vector3 forward);
		string GetParentName();

		void SetArrowTrail(IArrowTrail trail);
		void CheckAndClearArrowTrail(IArrowTrail trail);

		Vector3 GetPrevPosition();
	}
	public class Arrow : SlickBowShootingSceneObject, IArrow{
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
			IArrowAdaptor thisArrowAdaptor{
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
				// thisArrowAdaptor.ToggleGameObject(true);
				thisArrowAdaptor.ToggleRenderer(true);
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

				// thisArrowAdaptor.ToggleGameObject(false);
				thisArrowAdaptor.ToggleRenderer(false);
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
				thisFlightProcess = thisSlickBowShootingProcessFactory.CreateArrowFlightProcess(
					this,
					thisShootingManager.GetFlightSpeed(),
					thisShootingManager.GetFlightDirection(),
					thisShootingManager.GetFlightGravity(),
					thisShootingManager.GetLauncherVelocity(),
					thisLaunchPoint.GetPosition(),
					thisShootingManager.GetFlightTime()
				);
				thisFlightProcess.Run();
				thisArrowAdaptor.PlayArrowReleaseSound();
				thisArrowTrailReserve.ActivateTrailAt(
					this,
					thisNormalizedDraw
				);
			}
			IArrowTrailReserve thisArrowTrailReserve;
			public void SetArrowTrailReserve(IArrowTrailReserve reserve){
				thisArrowTrailReserve = reserve;
			}
			public void StopFlight(){
				if(thisFlightProcess != null)
					if(thisFlightProcess.IsRunning())
						thisFlightProcess.Stop();
				thisFlightProcess = null;
			}
			public void Land(
				// IShootingTarget target,
				IArrowHitDetector detector,
				Vector3 hitPosition
			){
				// if(detector.IsActivated())
				if(detector.ShouldSpawnLandedArrow()){
					thisShootingManager.SpawnLandedArrowOn(
						detector,
						hitPosition,
						thisAdaptor.GetRotation()
					);
					thisArrowAdaptor.PlayArrowHitSound();
				}
				
				Deactivate();
			}
			public void StartCollisionCheck(){
				thisArrowAdaptor.StartCollisionCheck();
			}
			public void StopColllisionCheck(){
				thisArrowAdaptor.StopCollisionCheck();
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
				return thisArrowAdaptor.GetParentName();
			}
		/*  */
			float thisAttack;
			public float GetAttack(){
				return thisAttack;
			}
			public void SetAttack(float attack){
				thisAttack = attack;
			}
			public Vector3 GetPrevPosition(){
				return thisArrowAdaptor.GetPrevPosition();
			}
			float thisNormalizedDraw;
			public void SetNormalizedDraw(float drawValue){
				thisNormalizedDraw = drawValue;
			}
		/* Const */
			public new interface IConstArg: SlickBowShootingSceneObject.IConstArg{
				int index{get;}
			}
			public new class ConstArg: SlickBowShootingSceneObject.ConstArg, IConstArg{
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
