using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPlayerCharacterLookAtTarget: IAppleShooterSceneObject{
		void StartLookAtTargetMotion();
		void SetDirection(Vector3 direction);
		Vector3 GetDirection();
		void SetSmoothLooker(ISmoothLooker looker);
	}
	public class PlayerCharacterLookAtTarget : AppleShooterSceneObject, IPlayerCharacterLookAtTarget {
		public PlayerCharacterLookAtTarget(
			IConstArg arg
		): base(
			arg
		){
			thisProcessOrder = arg.processOrder;
		}
		ISmoothLooker thisSmoothLooker;
		public void SetSmoothLooker(ISmoothLooker looker){
			thisSmoothLooker = looker;
		}
		readonly int thisProcessOrder;
		public void StartLookAtTargetMotion(){
			IPlayerCharacterLookAtTargetMotionProcess process = thisAppleShooterProcessFactory.CreateLookAtTargetMotionProcess(
				this,
				thisSmoothLooker,
				thisProcessOrder
			);
			process.Run();
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


		public new interface IConstArg: AppleShooterSceneObject.IConstArg{
			int processOrder{get;}
		}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IPlayerCharacterLookAtTargetAdaptor adaptor,
				int processOrder
			): base(
				adaptor
			){
				thisProcessOrder = processOrder;
			}
			readonly int thisProcessOrder;
			public int processOrder{get{return thisProcessOrder;}}
		}
	}

}
