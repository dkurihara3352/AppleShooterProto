using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
namespace AppleShooterProto{
	public interface ILookAtTargetProcess: IProcess{}
	public class LookAtTargetProcess : AbsProcess, ILookAtTargetProcess{
		public LookAtTargetProcess(
			ILookAtTargetProcessConstArg arg
		): base(
			arg
		){
			thisTarget = arg.target;
			thisSmoothLooker = arg.smoothLooker;
		}
		readonly ILookAtTarget thisTarget;
		readonly ISmoothLooker thisSmoothLooker;
		protected override void UpdateProcessImple(float deltaT){
			thisSmoothLooker.LookAt(thisTarget.GetPosition());
		}
	}
	public interface ILookAtTargetProcessConstArg: IProcessConstArg{
		ILookAtTarget target{get;}
		ISmoothLooker smoothLooker{get;}
	}
	public class LookAtTargetProcessConstArg: ProcessConstArg, ILookAtTargetProcessConstArg{
		public LookAtTargetProcessConstArg(
			IProcessManager processManager,

			ILookAtTarget target,
			ISmoothLooker looker
		): base(
			processManager
		){
			thisTarget = target;
			thisSmoothLooker = looker;
		}
		readonly ILookAtTarget thisTarget;
		public ILookAtTarget target{get{return thisTarget;}}
		readonly ISmoothLooker thisSmoothLooker;
		public ISmoothLooker smoothLooker{get{return thisSmoothLooker;}}
	}
}
