﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ISmoothFollower{
		void SetFollowTarget(IMonoBehaviourAdaptor target);
		void StartFollow();
		void StopFollow();
		Vector3 GetPosition();
		void SetPosition(Vector3 position);
	}
	public class SmoothFollower : ISmoothFollower{
		public SmoothFollower(
			ISmoothFollowerConstArg arg
		){
			thisAdaptor = arg.adaptor;
			thisProcessFactory = arg.processFactory;
			thisSmoothCoefficient = arg.smoothCoefficient;
			thisFollowTarget = arg.followTarget;
			thisProcessOrder = arg.processOrder;
		}
		readonly ISmoothFollowerAdaptor thisAdaptor;
		readonly IAppleShooterProcessFactory thisProcessFactory;
		readonly float thisSmoothCoefficient;
		IMonoBehaviourAdaptor thisFollowTarget;
		public void SetFollowTarget(IMonoBehaviourAdaptor target){
			thisFollowTarget = target;
		}
		ISmoothFollowTargetProcess thisProcess;
		readonly  int thisProcessOrder;
		public void StartFollow(){
			thisProcess = thisProcessFactory.CreateSmoothFollowTargetProcess(
				this,
				thisFollowTarget,
				thisSmoothCoefficient,
				thisProcessOrder
			);
			thisProcess.Run();
		}
		public void SetPosition(Vector3 position){
			thisAdaptor.SetPosition(position);
		}
		public Vector3 GetPosition(){
			return thisAdaptor.GetPosition();
		}
		public void StopFollow(){
			if(thisProcess.IsRunning())
				thisProcess.Stop();
		}
	}


	public interface ISmoothFollowerConstArg{
		IAppleShooterProcessFactory processFactory{get;}
		ISmoothFollowerAdaptor adaptor{get;}
		float smoothCoefficient{get;}
		IMonoBehaviourAdaptor followTarget{get;}
		int processOrder{get;}
	}
	public struct SmoothFollowerConstArg: ISmoothFollowerConstArg{
		public SmoothFollowerConstArg(
			ISmoothFollowerAdaptor adaptor,
			IAppleShooterProcessFactory processFactory,
			float smoothCoefficient,
			IMonoBehaviourAdaptor followTarget,
			int processOrder
		){
			thisAdaptor = adaptor;
			thisProcessFactory = processFactory;
			thisSmoothCoefficient = smoothCoefficient;
			thisFollowTarget = followTarget;
			thisProcessOrder = processOrder;
		}
		readonly ISmoothFollowerAdaptor thisAdaptor;
		public ISmoothFollowerAdaptor adaptor{get{return thisAdaptor;}}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
		readonly float thisSmoothCoefficient;
		public float smoothCoefficient{get{return thisSmoothCoefficient;}}
		readonly IMonoBehaviourAdaptor thisFollowTarget;
		public IMonoBehaviourAdaptor followTarget{get{return thisFollowTarget;}}
		readonly int thisProcessOrder;
		public int processOrder{get{return thisProcessOrder;}}
	}
}
