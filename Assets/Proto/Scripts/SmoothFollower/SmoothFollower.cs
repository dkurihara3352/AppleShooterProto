using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ISmoothFollower{
		void SetFollowTarget(ISmoothFollowTargetMBAdaptor target);
		void StartFollow();
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
		}
		readonly ISmoothFollowerAdaptor thisAdaptor;
		readonly IAppleShooterProcessFactory thisProcessFactory;
		readonly float thisSmoothCoefficient;
		ISmoothFollowTargetMBAdaptor thisTarget;
		public void SetFollowTarget(ISmoothFollowTargetMBAdaptor target){
			thisTarget = target;
		}
		public void StartFollow(){
			ISmoothFollowTargetProcess process = thisProcessFactory.CreateSmoothFollowTargetProcess(
				this,
				thisTarget,
				thisSmoothCoefficient
			);
			process.Run();
		}
		public void SetPosition(Vector3 position){
			thisAdaptor.SetPosition(position);
		}
		public Vector3 GetPosition(){
			return thisAdaptor.GetPosition();
		}
	}


	public interface ISmoothFollowerConstArg{
		IAppleShooterProcessFactory processFactory{get;}
		ISmoothFollowerAdaptor adaptor{get;}
		float smoothCoefficient{get;}
	}
	public struct SmoothFollowerConstArg: ISmoothFollowerConstArg{
		public SmoothFollowerConstArg(
			ISmoothFollowerAdaptor adaptor,
			IAppleShooterProcessFactory processFactory,
			float smoothCoefficient
		){
			thisAdaptor = adaptor;
			thisProcessFactory = processFactory;
			thisSmoothCoefficient = smoothCoefficient;
		}
		readonly ISmoothFollowerAdaptor thisAdaptor;
		public ISmoothFollowerAdaptor adaptor{get{return thisAdaptor;}}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
		readonly float thisSmoothCoefficient;
		public float smoothCoefficient{get{return thisSmoothCoefficient;}}
	}
	public interface ISmoothFollowTargetMBAdaptor: IMonoBehaviourAdaptor{
	}
}
