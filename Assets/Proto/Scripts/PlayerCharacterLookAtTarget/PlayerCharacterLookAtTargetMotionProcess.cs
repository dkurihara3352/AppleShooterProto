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
	}
	public interface IPlayerCharacterLookAtTargetMotionProcessConstArg: IProcessConstArg{
		IPlayerCharacterLookAtTarget lookAtTarget{get;}
		ISmoothLooker smoothLooker{get;}
	}
	public class PlayerCharacterLookAtTargetMotionProcessConstArg: ProcessConstArg, IPlayerCharacterLookAtTargetMotionProcessConstArg{
		public PlayerCharacterLookAtTargetMotionProcessConstArg(
			IProcessManager processManager,

			IPlayerCharacterLookAtTarget lookAtTarget,
			ISmoothLooker smoothLooker
		): base(
			processManager
		){
			thisLookAtTarget = lookAtTarget;
			thisSmoothLooker = smoothLooker;
		}
		readonly IPlayerCharacterLookAtTarget thisLookAtTarget;
		public IPlayerCharacterLookAtTarget lookAtTarget{get{return thisLookAtTarget;}}
		readonly ISmoothLooker thisSmoothLooker;
		public ISmoothLooker smoothLooker{get{return thisSmoothLooker;}}
	}
}

