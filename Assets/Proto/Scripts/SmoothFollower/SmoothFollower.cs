using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace SlickBowShooting{
	public interface ISmoothFollower: ISlickBowShootingSceneObject{
		void SetFollowTarget(IMonoBehaviourAdaptor target);
		void StartFollow();
		void StopFollow();
		Vector3 GetDeltaPosition();
		void SetVelocity(Vector3 velocity);
		Vector3 GetVelocity();
	}
	public class SmoothFollower : SlickBowShootingSceneObject, ISmoothFollower{
		public SmoothFollower(
			IConstArg arg
		): base(
			arg
		){
			thisSmoothCoefficient = arg.smoothCoefficient;
			thisFollowTarget = arg.followTarget;
			thisProcessOrder = arg.processOrder;

			thisPrevPosition = GetPosition();
		}
		readonly float thisSmoothCoefficient;
		IMonoBehaviourAdaptor thisFollowTarget;
		public void SetFollowTarget(IMonoBehaviourAdaptor target){
			thisFollowTarget = target;
			if(thisProcess != null && thisProcess.IsRunning())
				thisProcess.UpdateFollowTarget(target);
		}
		ISmoothFollowTargetProcess thisProcess;
		readonly  int thisProcessOrder;
		public void StartFollow(){
			thisProcess = thisSlickBowShootingProcessFactory.CreateSmoothFollowTargetProcess(
				this,
				thisFollowTarget,
				thisSmoothCoefficient,
				thisProcessOrder
			);
			thisProcess.Run();
		}
		public void StopFollow(){
			if(thisProcess.IsRunning())
				thisProcess.Stop();
		}
		Vector3 thisPrevPosition;
		public Vector3 GetDeltaPosition(){
			return GetPosition() - thisPrevPosition;
		}
		Vector3 thisVelocity;
		public void SetVelocity(Vector3 velocity){
			thisVelocity = velocity;
		}
		public Vector3 GetVelocity(){
			return thisVelocity;
		}

		public new interface IConstArg: SlickBowShootingSceneObject.IConstArg{
			float smoothCoefficient{get;}
			IMonoBehaviourAdaptor followTarget{get;}
			int processOrder{get;}
		}
		public new class ConstArg: SlickBowShootingSceneObject.ConstArg, IConstArg{
			public ConstArg(
				ISmoothFollowerAdaptor adaptor,
				float smoothCoefficient,
				IMonoBehaviourAdaptor followTarget,
				int processOrder
			): base(
				adaptor
			){
				thisSmoothCoefficient = smoothCoefficient;
				thisFollowTarget = followTarget;
				thisProcessOrder = processOrder;
			}
			readonly float thisSmoothCoefficient;
			public float smoothCoefficient{get{return thisSmoothCoefficient;}}
			readonly IMonoBehaviourAdaptor thisFollowTarget;
			public IMonoBehaviourAdaptor followTarget{get{return thisFollowTarget;}}
			readonly int thisProcessOrder;
			public int processOrder{get{return thisProcessOrder;}}
		}
	}


}
