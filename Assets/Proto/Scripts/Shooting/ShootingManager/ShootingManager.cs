using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingManager: IAppleShooterSceneObject{

		void SetInputManager(IPlayerInputManager inputManager);
		void SetLaunchPoint(ILaunchPoint launchPoint);
		void SetTrajectory(ITrajectory trajectory);
		void SetLandedArrowReserve(ILandedArrowReserve reserve);
		void SetArrowReserve(IArrowReserve reserve);
		void SetArrowTrailReserve(IArrowTrailReserve reserve);
		void SetCriticalFlash(ICriticalFlash flash);
		void SetShootingDataManager(IShootingDataManager manager);

		void NockArrow();
		void SetNockedArrow(IArrow arrow);
		void CheckAndClearNockedArrow(IArrow arrow);

		void StartDraw();
		void DrawImple(float deltaTime);
		void HoldDraw();
		void StopDraw();

		float GetDrawElapsedTime();
		float GetDrawStrength();
		// float GetGlobalDrawStrength();
		float GetArrowAttack();

		void Release();
		
		bool AcceptsNewShot();
		void RegisterShot(IArrow arrow);

		IShot GetShotInBuffer();
		void ClearShotBuffer();
		void ClearAndDeactivateShotInBuffer();
		void ClearShootingProcess();


		float GetFlightSpeed();
		Vector3 GetFlightDirection();
		float GetFlightGravity();
		Vector3 GetLauncherVelocity();
		float GetFlightTime();

		void SpawnLandedArrowOn(
			IShootingTarget target,
			Vector3 position,
			Quaternion rotation
		);

		void DeactivateArrow();

		float GetCriticalMultiplier();
		void Flash();

		string GetDebugString();
	}
	public class ShootingManager : AppleShooterSceneObject, IShootingManager {
		/* SetUp */
			public ShootingManager(
				IConstArg arg
			): base(
				arg
			){
				thisDrawProcessOrder = arg.drawProcessOrder;

				thisBowDrawProfileCurve = arg.bowDrawProfileCurve;
				thisGlobalMinDrawStrength = arg.globalMinDrawStrength;
				thisGlobalMaxDrawStrength = arg.globalMaxDrawStrength;
				
				thisGlobalMinAttack = arg.globalMinArrowAttack;
				thisGlobalMaxAttack = arg.globalMaxArrowAttack;
				thisGlobalMinFlightSpeed = arg.globalMinFlightSpeed;
				thisGlobalMaxFlightSpeed = arg.globalMaxFlightSpeed;

				thisFlightTime = arg.flightTime;

			}
			IShootingManagerAdaptor thisTypedAdaptor{
				get{
					return (IShootingManagerAdaptor)thisAdaptor;
				}
			}
			readonly int thisDrawProcessOrder;

			IShootingDataManager thisShootingDataManager;
			public void SetShootingDataManager(IShootingDataManager manager){
				thisShootingDataManager = manager;
			}
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
			IArrowTrailReserve thisArrowTrailReserve;
			public void SetArrowTrailReserve(IArrowTrailReserve reserve){
				thisArrowTrailReserve = reserve;
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
				thisDrawProcess = thisAppleShooterProcessFactory.CreateDrawProcess(
					this,
					thisDrawProcessOrder
				);
				thisDrawProcess.Run();
			}
			public void DrawImple(float deltaTime){
				if(thisDrawElapsedTime < thisShootingDataManager.GetDrawTime()){
					thisDrawElapsedTime += deltaTime;
					float normalizedDrawTime = GetNormalizedDrawTime();
					thisDrawStrength = CalculateDrawStrength(normalizedDrawTime);

					thisInputManager.Zoom(thisDrawStrength);
					thisFlightSpeed = CalculateFlightSpeed();
					thisArrowAttack = CalculateArrowAttack(thisDrawStrength);
				}
				DrawTrajectory();
			}
			/* DrawStrength */
				AnimationCurve thisBowDrawProfileCurve;

				float CalculateDrawStrength(float normalizedDrawTime){
					float normalizedCurveOutput = thisBowDrawProfileCurve.Evaluate(normalizedDrawTime);
					float scaledCurveOutput = Mathf.Lerp(

						thisShootingDataManager.GetMinDrawStrength(),
						thisShootingDataManager.GetMaxDrawStrength(),
						normalizedCurveOutput

					);

					return Mathf.Lerp(
						thisGlobalMinDrawStrength,
						thisGlobalMaxDrawStrength,
						scaledCurveOutput
					);
				}
				float thisDrawStrength;
				public float GetDrawStrength(){
					return thisDrawStrength;
				}
				float thisGlobalMinDrawStrength;
				float thisGlobalMaxDrawStrength;

			/* ArrowAttack */
				float thisArrowAttack;
				public float GetArrowAttack(){
					return thisArrowAttack;
				}
				float thisGlobalMinAttack;
				float thisGlobalMaxAttack;
				float CalculateArrowAttack(float drawStrength){
					float result = Mathf.Lerp(
						thisGlobalMinAttack,
						thisGlobalMaxAttack,
						drawStrength
					);
					return result;
				}
				// float thisCriticalMultiplier;
				public float GetCriticalMultiplier(){
					return thisShootingDataManager.GetCriticalMultiplier();
				}
			/* FlightSpeed */
				float thisFlightSpeed;			
				public virtual float GetFlightSpeed(){
					return thisFlightSpeed;
				}
				float thisGlobalMinFlightSpeed;
				float thisGlobalMaxFlightSpeed;
				float CalculateFlightSpeed(){
					float result = Mathf.Lerp(
						thisGlobalMinFlightSpeed,
						thisGlobalMaxFlightSpeed,
						thisDrawStrength
					);
					return result;
				}
			/*  */
				readonly float thisFlightTime;
				public float GetFlightTime(){
					return thisFlightTime;
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
				thisDrawElapsedTime = 0f;
				thisDrawStrength = CalculateDrawStrength(0f);
				thisFlightSpeed = CalculateFlightSpeed();
				thisArrowAttack = CalculateArrowAttack(thisDrawStrength);
				thisTrajectory.Clear();
			}
			float maxZoom{
				get{return thisInputManager.GetMaxZoom();}
			}
			float thisDrawElapsedTime = 0f;
			public float GetDrawElapsedTime(){
				return thisDrawElapsedTime;
			}
			// float thisMaxDrawTime{
			// 	get{return thisTypedAdaptor.GetMaxDrawTime();}
			// }
			float GetNormalizedDrawTime(){
				float result = thisDrawElapsedTime/ thisShootingDataManager.GetDrawTime();
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

				thisArrowTrailReserve.ActivateTrailAt(arrow);
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
			float thisFireRate{
				get{
					return thisShootingDataManager.GetFireRate();
				}
			}
			public virtual void RegisterShot(IArrow arrow){
				if(thisShotInBuffer == null){

					thisShotInBuffer = new Shot(arrow);
					if(thisShootingProcess == null){
						thisShootingProcess = thisAppleShooterProcessFactory.CreateShootingProcess(
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
			public void ClearAndDeactivateShotInBuffer(){
				if(thisShotInBuffer != null){
					IArrow arrow = thisShotInBuffer.GetArrow();
					arrow.Deactivate();
					thisShotInBuffer = null;
				}
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
			ICriticalFlash thisFlash;
			public void SetCriticalFlash(ICriticalFlash flash){
				thisFlash = flash;
			}
			public void Flash(){
				thisFlash.Flash();
			}
			float thisAssumedCritRate = .3f;
			public string GetDebugString(){
				string result = "";
				float fireRate = thisShootingDataManager.GetFireRate();
				float drawTime = thisShootingDataManager.GetDrawTime();

				result += "fireRate: " + fireRate.ToString() + "\n";

				float critical = thisShootingDataManager.GetCriticalMultiplier();

				result += "critMult: " + critical.ToString() + "\n";
				
				float minDrawStrength = thisShootingDataManager.GetMinDrawStrength();
				float minAttack = CalculateArrowAttack(minDrawStrength);
				float maxDrawStrength = thisShootingDataManager.GetMaxDrawStrength();
				float maxAttack = CalculateArrowAttack(maxDrawStrength);
				result += "atk: " + minAttack.ToString("N0") + " to " + maxAttack.ToString("N0") + " ";
				result += "(dps: " + (minAttack * 1f/fireRate).ToString("N0") + " to " + (maxAttack * 1f/(fireRate + drawTime)).ToString("N0") + ")\n";
				float critMin = minAttack * critical;
				float critMax = maxAttack * critical;
				result += "crit: " + critMin.ToString("N0") + " to " + critMax.ToString("N0") + " ";
				result += "(dps: " + (critMin * 1f/fireRate).ToString("N0") + " to " + (critMax * 1f/(fireRate + drawTime)).ToString("N0") + ")\n";
				result += "assumedCritRate: " + thisAssumedCritRate.ToString("N2") + "\n ";
				float assumedNormalRate = 1f - thisAssumedCritRate;
				float minCor = (assumedNormalRate * minAttack) + (thisAssumedCritRate * critMin);
				float maxCor = (assumedNormalRate * maxAttack) + (thisAssumedCritRate * critMax);
				result += "corrected atk: " + minCor.ToString("N0") + " to " + maxCor.ToString("N0") + " ";
				result += "(dps: " + (minCor * 1f/fireRate).ToString("N0") + " to " + (maxCor * 1f/(fireRate + drawTime)).ToString("N0") + ")\n";
				return result;
			}
		/* Const */
			public new interface IConstArg: AppleShooterSceneObject.IConstArg{
				int drawProcessOrder{get;}

				AnimationCurve bowDrawProfileCurve{get;}
				float globalMinDrawStrength{get;}
				float globalMaxDrawStrength{get;}

				float globalMinArrowAttack{get;}
				float globalMaxArrowAttack{get;}
				float globalMinFlightSpeed{get;}
				float globalMaxFlightSpeed{get;}

				float flightTime{get;}
			}
			public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
				public ConstArg(
					IShootingManagerAdaptor adaptor,

					int drawProcessOrder,

					AnimationCurve drawStrengthCurve,

					float globalMinDrawStrength,
					float globalMaxDrawStrength,

					float globalMinArrowAttack,
					float globalMaxArrowAttack,
					float globalMinFlightSpeed,
					float globalMaxFlightSpeed,

					float flightTime
				): base(
					adaptor
				){

					thisDrawProcessOrder = drawProcessOrder;

					thisDrawStrengthCurve = drawStrengthCurve;

					thisGlobalMinDrawStrength = globalMinDrawStrength;
					thisGlobalMaxDrawStrength = globalMaxDrawStrength;
					
					thisGlobalMinArrowAttack = globalMinArrowAttack;
					thisGlobalMaxArrowAttack = globalMaxArrowAttack;
					thisGlobalMinFlightSpeed = globalMinFlightSpeed;
					thisGlobalMaxFlightSpeed = globalMaxFlightSpeed;

					thisFlightTime = flightTime;
				}
				readonly int thisDrawProcessOrder;
				public int drawProcessOrder{get{return thisDrawProcessOrder;}}

				readonly AnimationCurve thisDrawStrengthCurve;
				public AnimationCurve bowDrawProfileCurve{get{return thisDrawStrengthCurve;}}


				readonly float thisGlobalMinDrawStrength;
				public float globalMinDrawStrength{get{return thisGlobalMinDrawStrength;}}
				readonly float thisGlobalMaxDrawStrength;
				public float globalMaxDrawStrength{get{return thisGlobalMaxDrawStrength;}}

				readonly float thisGlobalMinArrowAttack;
				public float globalMinArrowAttack{get{return thisGlobalMinArrowAttack;}}
				readonly float thisGlobalMaxArrowAttack;
				public float globalMaxArrowAttack{get{return thisGlobalMaxArrowAttack;}}

				readonly float thisGlobalMinFlightSpeed;
				public float globalMinFlightSpeed{get{return thisGlobalMinFlightSpeed;}}
				readonly float thisGlobalMaxFlightSpeed;
				public float globalMaxFlightSpeed{get{return thisGlobalMaxFlightSpeed;}}

				readonly float thisFlightTime;
				public float flightTime{get{return thisFlightTime;}}
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
