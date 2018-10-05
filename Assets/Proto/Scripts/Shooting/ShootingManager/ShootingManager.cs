using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingManager{
		void SetInputManager(IPlayerInputManager inputManager);
		void SetLaunchPoint(ILaunchPoint launchPoint);
		void SetTrajectory(ITrajectory trajectory);
		void SetArrows(IArrow[] arrows);
		IArrow[] GetAllArrows();

		void TryNock();
		void SetNockedArrow(IArrow arrow);
		void CheckAndClearNockedArrow(IArrow arrow);

		void StartDraw();
		void Draw(float deltaTime);
		void HoldDraw();
		void StopDraw();

		void Release();
		void ResetDraw();
		void TryResetArrow();
		
		bool AcceptsNewShot();
		void RegisterShot(IArrow arrow);
		IShot GetShotInBuffer();
		void ClearShotBuffer();
		void ClearShootingProcess();

		void AddArrowToReserve(IArrow arrow);
		void RemoveArrowFromReserve(IArrow arrow);
		void AddArrowToFlight(IArrow arrow);
		void RemoveArrowFromFlight(IArrow arrow);

		float GetFlightSpeed();
		Vector3 GetFlightDirection();
		float GetFlightGravity();
		Vector3 GetLauncherVelocity();

		int GetArrowReserveID(IArrow arrow);
		int GetFlightID(IArrow arrow);
	}
	public class ShootingManager : IShootingManager {
		/* SetUp */
			public ShootingManager(
				IShootingManagerConstArg arg
			){
				thisProcessFactory = arg.processFactory;
				thisAdaptor = arg.adaptor;
				thisDrawProcessOrder = arg.drawProcessOrder;
				thisFireRate = arg.fireRate;
			}
			readonly IAppleShooterProcessFactory thisProcessFactory;
			readonly IShootingManagerAdaptor thisAdaptor;
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
		/* Nock */
			IArrow thisNockedArrow;
			public void TryNock(){
				if(thisNockedArrow == null){
					IArrow arrowToNock;
					if(thisArrowsInReserve.Count > 0)
						arrowToNock = thisArrowsInReserve[0];
					else
						arrowToNock = thisArrowsInFlight[0];
					arrowToNock.TryNock();
				}
			}
			public void SetNockedArrow(IArrow arrow){
				thisNockedArrow = arrow;
			}
			public void CheckAndClearNockedArrow(IArrow arrow){
				if(thisNockedArrow == arrow)
					ClearNockedArrow();
			}
			void ClearNockedArrow(){
				thisNockedArrow  = null;
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
			public void Draw(float deltaTime){
				if(thisDrawElapsedTime < thisMaxDrawTime){
					thisDrawElapsedTime += deltaTime;
					float normalizedDraw = GetNormalizedDraw();
					thisInputManager.Zoom(normalizedDraw);
					this.UpdateFlightSpeed();
				}
				DrawTrajectory();
			}
			public void HoldDraw(){
				thisDrawProcess.Hold();
			}
			public void StopDraw(){
				if(thisDrawProcess != null && thisDrawProcess.IsRunning()){
					thisDrawProcess.Stop();
				}
				thisDrawProcess = null;
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
			void UpdateFlightSpeed(){
				thisFlightSpeed = CalcFlightSpeed(
					GetNormalizedDraw()
				);
			}
			float thisFlightSpeed;
			public float GetFlightSpeed(){
				return thisFlightSpeed;
			}
			float CalcFlightSpeed(
				float normalizedDraw
			){
				return Mathf.Lerp(
					initialSpeed,
					maxFlightSpeed,
					normalizedDraw
				);
			}
		/* flight & trajectory */
			public float initialSpeed{
				get{return thisAdaptor.GetInitialSpeed();}
			}
			public float maxFlightSpeed{
				get{return thisAdaptor.GetMaxFlightSpeed();}
			}
			public float gravity{
				get{return thisAdaptor.GetGravity();}
			}
			public float GetFlightGravity(){
				return gravity;
			}
			Vector3 flightDirection{
				get{return thisLaunchPoint.GetWorldDirection();}
			}
			public Vector3 GetFlightDirection(){
				return flightDirection;
			}
			void DrawTrajectory(){
				thisTrajectory.DrawTrajectory(
					flightDirection,
					thisFlightSpeed,
					gravity,
					thisLaunchPoint.GetWorldPosition()
				);
			}
		/* Arrow */
			List<IArrow> thisArrowsInReserve;
			int arrowsCount;
			public int GetArrowReserveID(IArrow arrow){
				int id = 0;
				foreach(IArrow arrowInReserve in thisArrowsInReserve){
					if(arrowInReserve == arrow)
						return id;
					id++;
				}
				return -1;
			}
			public void SetArrows(IArrow[] arrows){
				List<IArrow> arrowsList = new List<IArrow>();
				foreach(IArrow arrow in arrows){
					arrowsList.Add(arrow);
					arrow.SetShootingManager(this);
				}
				thisArrowsInReserve = arrowsList;
				arrowsCount = arrows.Length;
			}
			public IArrow[] GetAllArrows(){
				List<IArrow> list = new List<IArrow>();
				if(thisNockedArrow != null)
					list.Add(thisNockedArrow);
				foreach(IArrow arrow in thisArrowsInReserve)
					list.Add(arrow);
				foreach(IArrow arrow in thisArrowsInFlight)
					list.Add(arrow);
				
				return list.ToArray();
			}
			public void AddArrowToReserve(IArrow arrow){
				if(!thisArrowsInReserve.Contains(arrow))
					thisArrowsInReserve.Add(arrow);
			}
			public void RemoveArrowFromReserve(IArrow arrow){
				if(thisArrowsInReserve.Contains(arrow))
					thisArrowsInReserve.Remove(arrow);
			}
			List<IArrow> thisArrowsInFlight = new List<IArrow>();
			public void AddArrowToFlight(IArrow arrow){
				if(!thisArrowsInFlight.Contains(arrow))
					thisArrowsInFlight.Add(arrow);
			}
			public void RemoveArrowFromFlight(IArrow arrow){
				if(thisArrowsInFlight.Contains(arrow))
					thisArrowsInFlight.Remove(arrow);
			}
			public int GetFlightID(IArrow arrow){
				if(thisArrowsInFlight.Contains(arrow))
					return thisArrowsInFlight.IndexOf(arrow);
				throw new System.InvalidOperationException(
					"given arrow is not in flight list"
				);
			}
		/*  */
			public void Release(){
				IArrow arrow = thisNockedArrow;
				ClearNockedArrow();
				arrow.TryFire();
				ResetDraw();
			}
			public void ResetDraw(){
				// HoldDraw();
				StopDraw();
				thisDrawElapsedTime = 0f;
				UpdateFlightSpeed();
				thisTrajectory.Clear();
			}
			public void TryResetArrow(){
				if(thisNockedArrow != null)
					thisNockedArrow.TryResetArrow();
			}
		/* shooting Process */
			public bool AcceptsNewShot(){
				return thisShotInBuffer == null;
			}
			IShootingProcess thisShootingProcess;
			IShot thisShotInBuffer;
			public IShot GetShotInBuffer(){
				return thisShotInBuffer;
			}
			public void RegisterShot(IArrow arrow){
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
			public Vector3 GetLauncherVelocity(){
				return thisInputManager.GetLauncherVelocity();
			}
	}


	public interface IShootingManagerConstArg{
		IAppleShooterProcessFactory processFactory{get;}
		IShootingManagerAdaptor adaptor{get;}
		int drawProcessOrder{get;}
		float fireRate{get;}
	}
	public class ShootingManagerConstArg: IShootingManagerConstArg{
		public ShootingManagerConstArg(
			IAppleShooterProcessFactory processFactory,
			IShootingManagerAdaptor adaptor,
			int drawProcessOrder,
			float fireRate
		){
			thisProcessFactory = processFactory;
			thisAdaptor = adaptor;
			thisDrawProcessOrder = drawProcessOrder;
			thisFireRate = fireRate;
		}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
		readonly IShootingManagerAdaptor thisAdaptor;
		public IShootingManagerAdaptor adaptor{get{return thisAdaptor;}}
		readonly int thisDrawProcessOrder;
		public int drawProcessOrder{get{return thisDrawProcessOrder;}}
		readonly float thisFireRate;
		public float fireRate{get{return thisFireRate;}}
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
