using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingManager: ISceneObject{

		void SetInputManager(IPlayerInputManager inputManager);
		void SetLaunchPoint(ILaunchPoint launchPoint);
		void SetTrajectory(ITrajectory trajectory);
		void SetLandedArrowReserve(ILandedArrowReserve reserve);
		void SetArrowReserve(IArrowReserve reserve);

		void NockArrow();
		void SetNockedArrow(IArrow arrow);
		void CheckAndClearNockedArrow(IArrow arrow);

		void StartDraw();
		void DrawImple(float deltaTime);
		void HoldDraw();
		void StopDraw();

		float GetDrawElapsedTime();
		float GetDrawStrength();
		float GetGlobalDrawStrength();
		float GetArrowAttack();

		void Release();
		
		bool AcceptsNewShot();
		void RegisterShot(IArrow arrow);

		IShot GetShotInBuffer();
		void ClearShotBuffer();
		void ClearShootingProcess();


		float GetFlightSpeed();
		Vector3 GetFlightDirection();
		float GetFlightGravity();
		Vector3 GetLauncherVelocity();

		void SpawnLandedArrowOn(
			IShootingTarget target,
			Vector3 position,
			Quaternion rotation
		);

		void DeactivateArrow();
	}
	public class ShootingManager : AbsSceneObject, IShootingManager {
		/* SetUp */
			public ShootingManager(
				IConstArg arg
			): base(
				arg
			){
				thisDrawProcessOrder = arg.drawProcessOrder;
				thisFireRate = arg.fireRate;

				thisDrawStrengthCurve = arg.drawStrengthCurve;
				thisGlobalMinDrawStrength = arg.globalMinDrawStrength;
				thisGlobalMaxDrawStrength = arg.globalMaxDrawStrength;
				
				thisGlobalMinAttack = arg.globalMinArrowAttack;
				thisArrowAttackMultiplier = arg.arrowAttackMultiplier;
				thisGlobalMinFlightSpeed = arg.globalMinFlightSpeed;
				thisFlightSpeedMultiplier = arg.flightSpeedMultiplier;
			}
			IShootingManagerAdaptor thisTypedAdaptor{
				get{
					return (IShootingManagerAdaptor)thisAdaptor;
				}
			}
			readonly int thisDrawProcessOrder;
			readonly float thisFireRate;
			IPlayerInputManager thisInputManager;
			public void SetInputManager(IPlayerInputManager inputManager){
				thisInputManager = inputManager;
			}
			ITrajectory thisTrajectory;
			public void SetTrajectory(ITrajectory trajectory){
				thisTrajectory = trajectory;
			}
			ILaunchPoint thisLaunchPoint;
			public void SetLaunchPoint(ILaunchPoint launchPoint){
				thisLaunchPoint = launchPoint;
			}
			ILandedArrowReserve thisLandedArrowReserve;
			public void SetLandedArrowReserve(ILandedArrowReserve reserve){
				thisLandedArrowReserve = reserve;
			}
		/* Nock */
			IArrow thisNockedArrow;
			public void NockArrow(){
				IArrow nextArrow = thisArrowReserve.GetNextArrow();
				nextArrow.Nock();
			}
			public virtual void SetNockedArrow(IArrow arrow){
				thisNockedArrow = arrow;
			}
			public virtual void CheckAndClearNockedArrow(IArrow arrow){
				if(thisNockedArrow == arrow)
					thisNockedArrow = null;
			}
		/* Draw */
			public void StartDraw(){
				StopDraw();
				thisDrawProcess = thisProcessFactory.CreateDrawProcess(
					this,
					thisDrawProcessOrder
				);
				thisDrawProcess.Run();
			}
			public void DrawImple(float deltaTime){
				if(thisDrawElapsedTime < thisMaxDrawTime){
					thisDrawElapsedTime += deltaTime;
					float normalizedDraw = GetNormalizedDraw();
					thisDrawStrength = CalculateDrawStrength(normalizedDraw);

					thisGlobalDrawStrength = CalculateGlobalDrawStrength(thisDrawStrength);
					thisInputManager.Zoom(thisGlobalDrawStrength);
					thisFlightSpeed = CalculateFlightSpeed();
					thisArrowAttack = CalculateArrowAttack();
				}
				DrawTrajectory();
			}
			/* DrawStrength */
				AnimationCurve thisDrawStrengthCurve;
				float CalculateDrawStrength(float normalizedDraw){
					return thisDrawStrengthCurve.Evaluate(normalizedDraw);
				}
				float thisDrawStrength;
				public float GetDrawStrength(){
					return thisDrawStrength;
				}
				float thisGlobalDrawStrength;
				/*  draw strength relative to global min and max
					0f equals minimum possible draw strength
					1f to maximum
				*/
				public float GetGlobalDrawStrength(){
					return thisGlobalDrawStrength;
				}
				float thisGlobalMinDrawStrength;
				float thisGlobalMaxDrawStrength;
				float CalculateGlobalDrawStrength(float drawStrength){
					float numerator = drawStrength - thisGlobalMinDrawStrength;
					float denominator = thisGlobalMaxDrawStrength - thisGlobalMinDrawStrength;
					return numerator / denominator;
				}
			/* ArrowAttack */
				float thisArrowAttack;
				public float GetArrowAttack(){
					return thisArrowAttack;
				}
				float thisGlobalMinAttack;
				float thisArrowAttackMultiplier;
				float CalculateArrowAttack(){
					return thisGlobalMinAttack + (thisGlobalDrawStrength * thisArrowAttackMultiplier);
				}
			/* FlightSpeed */
				float thisFlightSpeed;			
				public virtual float GetFlightSpeed(){
					return thisFlightSpeed;
				}
				float thisGlobalMinFlightSpeed;
				float thisFlightSpeedMultiplier;
				float CalculateFlightSpeed(){
					return thisGlobalMinFlightSpeed + (thisGlobalDrawStrength * thisFlightSpeedMultiplier);
				}
			/*  */
			public void HoldDraw(){
				thisDrawProcess.Hold();
			}
			public void StopDraw(){
				if(thisDrawProcess != null && thisDrawProcess.IsRunning()){
					thisDrawProcess.Stop();
				}
				thisDrawProcess = null;
				ClearDrawFields();
			}
			void ClearDrawFields(){
				// StopDraw();
				thisDrawElapsedTime = 0f;
				thisDrawStrength = CalculateDrawStrength(0f);
				thisGlobalDrawStrength = CalculateGlobalDrawStrength(thisDrawStrength);
				thisFlightSpeed = CalculateFlightSpeed();
				thisArrowAttack = CalculateArrowAttack();
				thisTrajectory.Clear();
			}
			float maxZoom{
				get{return thisInputManager.GetMaxZoom();}
			}
			float thisDrawElapsedTime = 0f;
			public float GetDrawElapsedTime(){
				return thisDrawElapsedTime;
			}
			float thisMaxDrawTime{
				get{return thisTypedAdaptor.GetMaxDrawTime();}
			}
			float GetNormalizedDraw(){
				float result = thisDrawElapsedTime/ thisMaxDrawTime;
				if(result > 1f)
					result = 1f;
				return result;
			}
			IDrawProcess thisDrawProcess;
		/* flight & trajectory */
			public float initialSpeed{
				get{return thisTypedAdaptor.GetInitialSpeed();}
			}
			public float maxFlightSpeed{
				get{return thisTypedAdaptor.GetMaxFlightSpeed();}
			}
			public float gravity{
				get{return thisTypedAdaptor.GetGravity();}
			}
			public virtual float GetFlightGravity(){
				return gravity;
			}
			Vector3 flightDirection{
				get{return thisLaunchPoint.GetForwardDirection();}
			}
			public virtual Vector3 GetFlightDirection(){
				return flightDirection;
			}
			void DrawTrajectory(){
				thisTrajectory.DrawTrajectory(
					flightDirection,
					thisFlightSpeed,
					gravity,
					thisLaunchPoint.GetPosition()
				);
			}
		/* Arrow */
			IArrowReserve thisArrowReserve;
			public void SetArrowReserve(IArrowReserve reserve){
				thisArrowReserve = reserve;
			}
			IArrow[] GetArrowsInFlight(){
				List<IArrow> resultList = new List<IArrow>();
				IArrow[] arrows = thisArrowReserve.GetArrows();
				foreach(IArrow arrow in arrows)
					if(arrow.IsInFlight())
						resultList.Add(arrow);
				return resultList.ToArray();
			}
		/* Release */
			public void Release(){
				IArrow arrow = thisNockedArrow;
				thisNockedArrow = null;
				arrow.SetAttack(thisArrowAttack);
				arrow.Release();
				StopDraw();
				// ResetDraw();
			}
		/* shooting Process */
			public virtual bool AcceptsNewShot(){
				return thisShotInBuffer == null;
			}
			IShootingProcess thisShootingProcess;
			IShot thisShotInBuffer;
			public IShot GetShotInBuffer(){
				return thisShotInBuffer;
			}
			public virtual void RegisterShot(IArrow arrow){
				if(thisShotInBuffer == null){

					thisShotInBuffer = new Shot(arrow);
					if(thisShootingProcess == null){
						thisShootingProcess = thisProcessFactory.CreateShootingProcess(
							this,
							thisFireRate
						);
						thisShootingProcess.Run();
					}
				}
			}
			public void ClearShotBuffer(){
				thisShotInBuffer = null;
			}
			public void ClearShootingProcess(){
				thisShootingProcess = null;
			}
			public virtual Vector3 GetLauncherVelocity(){
				return thisInputManager.GetLauncherVelocity();
			}
		/* Spawn Landed Arrow */
			public virtual void SpawnLandedArrowOn(
				IShootingTarget target,
				Vector3 position,
				Quaternion rotation
			){
				thisLandedArrowReserve.ActivateLandedArrowAt(
					target,
					position,
					rotation
				);
			}
		/*  */
			public void DeactivateArrow(){
				thisNockedArrow.Deactivate();
			}
		/* Const */
			public new interface IConstArg: AbsSceneObject.IConstArg{
				int drawProcessOrder{get;}
				float fireRate{get;}

				AnimationCurve drawStrengthCurve{get;}
				float globalMinDrawStrength{get;}
				float globalMaxDrawStrength{get;}

				float globalMinArrowAttack{get;}
				float arrowAttackMultiplier{get;}
				float globalMinFlightSpeed{get;}
				float flightSpeedMultiplier{get;}
			}
			public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
				public ConstArg(
					IShootingManagerAdaptor adaptor,

					int drawProcessOrder,
					float fireRate,

					AnimationCurve drawStrengthCurve,
					float globalMinDrawStrength,
					float globalMaxDrawStrength,

					float globalMinArrowAttack,
					float arrowAttackMultiplier,
					float globalMinFlightSpeed,
					float flightSpeedMultiplier
				): base(
					adaptor
				){

					thisDrawProcessOrder = drawProcessOrder;
					thisFireRate = fireRate;

					thisDrawStrengthCurve = drawStrengthCurve;
					thisGlobalMinDrawStrength = globalMinDrawStrength;
					thisGlobalMaxDrawStrength = globalMaxDrawStrength;
					
					thisGlobalMinArrowAttack = globalMinArrowAttack;
					thisArrowAttackMultiplier = arrowAttackMultiplier;
					thisGlobalMinFlightSpeed = globalMinFlightSpeed;
					thisFlightSpeedMultiplier = flightSpeedMultiplier;
				}
				readonly int thisDrawProcessOrder;
				public int drawProcessOrder{get{return thisDrawProcessOrder;}}
				readonly float thisFireRate;
				public float fireRate{get{return thisFireRate;}}

				readonly AnimationCurve thisDrawStrengthCurve;
				public AnimationCurve drawStrengthCurve{get{return thisDrawStrengthCurve;}}
				readonly float thisGlobalMinDrawStrength;
				public float globalMinDrawStrength{get{return thisGlobalMinDrawStrength;}}
				readonly float thisGlobalMaxDrawStrength;
				public float globalMaxDrawStrength{get{return thisGlobalMaxDrawStrength;}}

				readonly float thisGlobalMinArrowAttack;
				public float globalMinArrowAttack{get{return thisGlobalMinArrowAttack;}}
				readonly float thisArrowAttackMultiplier;
				public float arrowAttackMultiplier{get{return thisArrowAttackMultiplier;}}

				readonly float thisGlobalMinFlightSpeed;
				public float globalMinFlightSpeed{get{return thisGlobalMinFlightSpeed;}}
				readonly float thisFlightSpeedMultiplier;
				public float flightSpeedMultiplier{get{return thisFlightSpeedMultiplier;}}
			}
		/*  */
	}



	public interface IShot{
		IArrow GetArrow();
	}
	public struct Shot: IShot{
		public Shot(
			IArrow arrow
		){
			thisArrow = arrow;
		}
		readonly IArrow thisArrow;
		public IArrow GetArrow(){return thisArrow;}
	}
}
