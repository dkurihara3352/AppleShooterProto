using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILookAtTarget{
		void StartLookAtTargetMotion();
		void SetPosition(Vector3 position);
		void SetDirection(Vector3 direction);
		Vector3 GetDirection();
		Vector3 GetPosition();
	}
	public class LookAtTarget : ILookAtTarget {
		public LookAtTarget(
			ILookAtTargetConstArg arg
		){
			thisAdaptor = arg.adaptor;
			thisSmoothLooker = arg.smoothLooker;
			thisProcessFactory = arg.processFactory;
			thisSmoothCoefficient = arg.smoothCoefficient;
			SetDirection(new Vector3(0f, 0f, 1f));
		}
		readonly ILookAtTargetAdaptor thisAdaptor;
		readonly ISmoothLooker thisSmoothLooker;
		readonly IAppleShooterProcessFactory thisProcessFactory;
		readonly float thisSmoothCoefficient;
		public void StartLookAtTargetMotion(){
			ILookAtTargetMotionProcess process = thisProcessFactory.CreateLookAtTargetMotionProcess(
				this,
				thisSmoothLooker,
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
		float thisAnchorLength = 5f;
		Vector3 thisAnchorPosition{
			get{return thisSmoothLooker.GetPosition();}
		}
		Vector3 thisDirection;
		public Vector3 GetDirection(){
			return thisDirection;
		}
		public void SetDirection(Vector3 direction){
			Vector3 newPosition = thisAnchorPosition + direction * thisAnchorLength;
			thisDirection = direction;
			SetPosition(newPosition);
		}
	}

	public interface ILookAtTargetConstArg{
		ILookAtTargetAdaptor adaptor{get;}
		ISmoothLooker smoothLooker{get;}
		IAppleShooterProcessFactory processFactory{get;}
		float smoothCoefficient{get;}
	}
	public struct LookAtTargetConstArg: ILookAtTargetConstArg{
		public LookAtTargetConstArg(
			ILookAtTargetAdaptor adaptor,
			ISmoothLooker looker,
			IAppleShooterProcessFactory processFactory,
			float smoothCoefficient
		){
			thisAdaptor = adaptor;
			thisSmoothLooker = looker;
			thisProcessFactory = processFactory;
			thisSmoothCoefficient = smoothCoefficient;
		}
		readonly ILookAtTargetAdaptor thisAdaptor;
		public ILookAtTargetAdaptor adaptor{get{return thisAdaptor;}}
		readonly ISmoothLooker thisSmoothLooker;
		public ISmoothLooker smoothLooker{get{return thisSmoothLooker;}}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
		readonly float thisSmoothCoefficient;
		public float smoothCoefficient{get{return thisSmoothCoefficient;}}
	}
}
