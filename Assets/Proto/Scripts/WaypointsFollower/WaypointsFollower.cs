using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IWaypointsFollower{
		void SetWaypointsManager(IWaypointCurveCycleManager wayointsManager);
		float GetFollowSpeed();
		void StartFollowing();
		void StopFollowing();
		void SetWaypointCurve(IWaypointCurve group);

		IWaypointCurve GetCurrentWaypointCurve();

		void SetPosition(Vector3 position);
		Vector3 GetPosition();
		void SetRotation(Quaternion rotation);
		Vector3 GetForwardDirection();
		void LookAt(
			Vector3 foward,
			Vector3 up
		);

		int GetCurrentWaypointCurveIndex();
		float GetNormalizedPositionInCurve();

		void SmoothStart();
		void SmoothStop();
		void SetElapsedTimeOnCurrentCurve(float elapsedTime);
	}
	public class WaypointsFollower: IWaypointsFollower{
		public WaypointsFollower(
			IWaypointsFollowerConstArg arg
		){
			thisAdaptor = arg.adaptor;
			thisProcessFactory = arg.processFactory;
			thisFollowSpeed = arg.followSpeed;
			thisProcessOrder = arg.processOrder;
		}
		readonly IWaypointsFollowerAdaptor thisAdaptor;
		readonly IAppleShooterProcessFactory thisProcessFactory;
		readonly float thisFollowSpeed;

		public void SetWaypointsManager(IWaypointCurveCycleManager waypointsManager){
			thisWaypointsManager = waypointsManager;
		}
		IWaypointCurveCycleManager thisWaypointsManager;

		public float GetFollowSpeed(){
			return thisFollowSpeed;
		}
		IFollowWaypointProcess thisProcess;
		readonly int thisProcessOrder;
		public void StartFollowing(){
			StopFollowing();
			thisProcess = CreateFollowProcess();
			thisProcess.Run();
		}
		IFollowWaypointProcess CreateFollowProcess(){
			return thisProcessFactory.CreateFollowWaypointProcess(
				this,
				thisFollowSpeed,
				thisProcessOrder,
				thisCurrentWaypointCurve,
				thisWaypointsManager,
				thisElapsedTimeOnCurrentCurve
			);
		}
		public void StopFollowing(){
			if(thisProcess != null && thisProcess.IsRunning())
				thisProcess.Stop();
			thisProcess = null;
		}
		public void SetPosition(
			Vector3 position
		){
			thisAdaptor.SetPosition(position);
		}
		public Vector3 GetPosition(){
			return thisAdaptor.GetPosition();
		}
		public void SetRotation(Quaternion rotation){
			thisAdaptor.SetRotation(rotation);
		}
		public Vector3 GetForwardDirection(){
			return thisAdaptor.GetForwardDirection();
		}
		public void LookAt(
			Vector3 forward,
			Vector3 up
		){
			thisAdaptor.SetLookRotation(
				forward,
				up
			);
		}
		/* curve access */
		IWaypointCurve thisCurrentWaypointCurve;
		public void SetWaypointCurve(IWaypointCurve curve){
			thisCurrentWaypointCurve = curve;
		}
		public IWaypointCurve GetCurrentWaypointCurve(){
			return thisCurrentWaypointCurve;
		}

		/* Debug */
		public int GetCurrentWaypointCurveIndex(){
			return thisCurrentWaypointCurve.GetIndex();
		}
		public float GetNormalizedPositionInCurve(){
			if(thisProcess != null)
				return thisProcess.GetNormalizedPositionOnCurve();
			else
				return 0f;
		}
		IWaypointsFollowerChangeSpeedProcess thisChangeSpeedProcess;
		IWaypointsFollowerAdaptor thisTypedAdaptor{
			get{
				return (IWaypointsFollowerAdaptor)thisAdaptor;
			}
		}
		public void SmoothStart(){
			StopFollowing();
			StopChangeSpeed();
			thisProcess = CreateFollowProcess();
			thisProcess.SetTimeScale(0f);
			thisProcess.Run();
			thisChangeSpeedProcess = thisProcessFactory.CreateWaypointsFollowerChangeSpeedProcess(
				thisProcess,
				thisTypedAdaptor.GetSmoothStartTime(),
				thisTypedAdaptor.GetSmoothStartCurve()
			);
			thisChangeSpeedProcess.Run();
		}
		void StopChangeSpeed(){
			if(thisChangeSpeedProcess != null && thisChangeSpeedProcess.IsRunning())
				thisChangeSpeedProcess.Stop();
			thisChangeSpeedProcess = null;
		}

		public void SmoothStop(){
			StopChangeSpeed();
			thisChangeSpeedProcess = thisProcessFactory.CreateWaypointsFollowerChangeSpeedProcess(
				thisProcess,
				thisTypedAdaptor.GetSmoothStopTime(),
				thisTypedAdaptor.GetSmoothStopCurve()
			);
			thisChangeSpeedProcess.Run();
		}
		float thisElapsedTimeOnCurrentCurve = 0f;
		public void SetElapsedTimeOnCurrentCurve(float elapsedTime){
			thisElapsedTimeOnCurrentCurve = elapsedTime;
		}
	}


	public interface IWaypointsFollowerConstArg{
		IWaypointsFollowerAdaptor adaptor{get;}
		IAppleShooterProcessFactory processFactory{get;}
		float followSpeed{get;}
		int processOrder{get;}
	}
	public struct WaypointsFollowerConstArg: IWaypointsFollowerConstArg{
		public WaypointsFollowerConstArg(
			IWaypointsFollowerAdaptor adaptor,
			IAppleShooterProcessFactory processFactory,
			float followSpeed,
			int processOrder
		){
			thisAdaptor = adaptor;
			thisProcessFactory = processFactory;
			thisFollowSpeed = followSpeed;
			thisProcessOrder = processOrder;
		}
		readonly IWaypointsFollowerAdaptor thisAdaptor;
		public IWaypointsFollowerAdaptor adaptor{get{return thisAdaptor;}}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
		readonly float thisFollowSpeed;
		public float followSpeed{get{return thisFollowSpeed;}}
		readonly int thisProcessOrder;
		public int processOrder{get{return thisProcessOrder;}}
	}
}

