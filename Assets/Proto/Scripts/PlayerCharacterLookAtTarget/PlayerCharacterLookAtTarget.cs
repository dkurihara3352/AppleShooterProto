using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPlayerCharacterLookAtTarget{
		void StartLookAtTargetMotion();
		void SetDirection(Vector3 direction);
		void SetPosition(Vector3 position);
		Vector3 GetDirection();
		Vector3 GetPosition();
		void SetSmoothLooker(ISmoothLooker looker);
	}
	public class PlayerCharacterLookAtTarget : IPlayerCharacterLookAtTarget {
		public PlayerCharacterLookAtTarget(
			ILookAtTargetConstArg arg
		){
			thisAdaptor = arg.adaptor;
			thisProcessFactory = arg.processFactory;
		}
		readonly IPlayerCharacterLookAtTargetAdaptor thisAdaptor;
		ISmoothLooker thisSmoothLooker;
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public void SetSmoothLooker(ISmoothLooker looker){
			thisSmoothLooker = looker;
		}
		public void StartLookAtTargetMotion(){
			IPlayerCharacterLookAtTargetMotionProcess process = thisProcessFactory.CreateLookAtTargetMotionProcess(
				this,
				thisSmoothLooker
			);
			process.Run();
		}
		public void SetPosition(Vector3 position){
			thisAdaptor.SetPosition(position);
		}
		public Vector3 GetPosition(){
			return thisAdaptor.GetPosition();
		}
		float thisAnchorLength = 20f;
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
		IPlayerCharacterLookAtTargetAdaptor adaptor{get;}
		IAppleShooterProcessFactory processFactory{get;}
	}
	public struct LookAtTargetConstArg: ILookAtTargetConstArg{
		public LookAtTargetConstArg(
			IPlayerCharacterLookAtTargetAdaptor adaptor,
			IAppleShooterProcessFactory processFactory
		){
			thisAdaptor = adaptor;
			thisProcessFactory = processFactory;
		}
		readonly IPlayerCharacterLookAtTargetAdaptor thisAdaptor;
		public IPlayerCharacterLookAtTargetAdaptor adaptor{get{return thisAdaptor;}}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
	}
}
