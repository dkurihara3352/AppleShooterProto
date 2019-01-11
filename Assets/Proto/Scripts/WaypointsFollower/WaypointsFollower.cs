using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface IWaypointsFollower: ISlickBowShootingSceneObject{
		void SetWaypointsManager(IWaypointCurveCycleManager wayointsManager);
		float GetFollowSpeed();
		void StartFollowing();
		void StopFollowing();
		void SetWaypointCurve(IWaypointCurve group);

		IWaypointCurve GetCurrentWaypointCurve();

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

		void ResetFollower();
	}
	public class WaypointsFollower: SlickBowShootingSceneObject, IWaypointsFollower{
		public WaypointsFollower(
			IConstArg arg
		): base(
			arg
		){
			thisFollowSpeed = arg.followSpeed;
			thisProcessOrder = arg.processOrder;
		}
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
			Debug.Log(GetName() + " is starting to follow, elapsedTime is " + thisElapsedTimeOnCurrentCurve.ToString());
			return thisSlickBowShootingProcessFactory.CreateFollowWaypointProcess(
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
			thisChangeSpeedProcess = thisSlickBowShootingProcessFactory.CreateWaypointsFollowerChangeSpeedProcess(
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
			thisChangeSpeedProcess = thisSlickBowShootingProcessFactory.CreateWaypointsFollowerChangeSpeedProcess(
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
		public void ResetFollower(){
			StopFollowing();
			thisElapsedTimeOnCurrentCurve = 0f;
			Debug.Log(GetName() + " is reset follower");
		}

		public new interface IConstArg: SlickBowShootingSceneObject.IConstArg{
			float followSpeed{get;}
			int processOrder{get;}
		}
		public new class ConstArg: SlickBowShootingSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IWaypointsFollowerAdaptor adaptor,
				float followSpeed,
				int processOrder
			): base(adaptor){
				thisFollowSpeed = followSpeed;
				thisProcessOrder = processOrder;
			}
			readonly float thisFollowSpeed;
			public float followSpeed{get{return thisFollowSpeed;}}
			readonly int thisProcessOrder;
			public int processOrder{get{return thisProcessOrder;}}
		}
	}
}

