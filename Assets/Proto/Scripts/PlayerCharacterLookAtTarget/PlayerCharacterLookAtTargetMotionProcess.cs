using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IPlayerCharacterLookAtTargetMotionProcess: IProcess{}
	public class PlayerCharacterLookAtTargetMotionProcess: AbsProcess, IPlayerCharacterLookAtTargetMotionProcess{
		public PlayerCharacterLookAtTargetMotionProcess(
			IPlayerCharacterLookAtTargetMotionProcessConstArg arg
		):base(
			arg
		){
			thisSmoothLooker = arg.smoothLooker;
			thisLookAtTarget = arg.lookAtTarget;
			thisProcessOrder = arg.processOrder;
		}
		readonly ISmoothLooker thisSmoothLooker;
		readonly IPlayerCharacterLookAtTarget thisLookAtTarget;
		Vector3 prevPosition;
		protected override void UpdateProcessImple(float deltaT){
			Vector3 lookerCurPosition = thisSmoothLooker.GetPosition();
			Vector3 smoothLookerMovingDirection = (lookerCurPosition - prevPosition).normalized;
			Vector3 newDirection = smoothLookerMovingDirection;
			thisLookAtTarget.SetDirection(newDirection);
			prevPosition = lookerCurPosition;
		}
		readonly int thisProcessOrder;
		public override int GetProcessOrder(){
			return thisProcessOrder;
		}
	}
	public interface IPlayerCharacterLookAtTargetMotionProcessConstArg: IProcessConstArg{
		IPlayerCharacterLookAtTarget lookAtTarget{get;}
		ISmoothLooker smoothLooker{get;}
		int processOrder{get;}
	}
	public class PlayerCharacterLookAtTargetMotionProcessConstArg: ProcessConstArg, IPlayerCharacterLookAtTargetMotionProcessConstArg{
		public PlayerCharacterLookAtTargetMotionProcessConstArg(
			IProcessManager processManager,

			IPlayerCharacterLookAtTarget lookAtTarget,
			ISmoothLooker smoothLooker,
			int processOrder
		): base(
			processManager
		){
			thisLookAtTarget = lookAtTarget;
			thisSmoothLooker = smoothLooker;
			thisProcessOrder = processOrder;
		}
		readonly IPlayerCharacterLookAtTarget thisLookAtTarget;
		public IPlayerCharacterLookAtTarget lookAtTarget{get{return thisLookAtTarget;}}
		readonly ISmoothLooker thisSmoothLooker;
		public ISmoothLooker smoothLooker{get{return thisSmoothLooker;}}
		readonly int thisProcessOrder;
		public int processOrder{get{return thisProcessOrder;}}
	}
}

