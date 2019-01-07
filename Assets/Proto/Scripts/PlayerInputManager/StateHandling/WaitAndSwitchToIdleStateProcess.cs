using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface IWaitAndSwitchToIdleStateProcess: IProcess{

	}
	public class WaitAndSwitchToIdleStateProcess: AbsConstrainedProcess, IWaitAndSwitchToIdleStateProcess{
		public WaitAndSwitchToIdleStateProcess(
			IConstArg arg
		): base(
			arg
		){
			thisEngine = arg.engine;
		}
		IPlayerInputStateEngine thisEngine;
		protected override void ExpireImple(){
			thisEngine.SwitchToIdleState();
		}


		public new interface IConstArg: AbsConstrainedProcess.IConstArg{
			IPlayerInputStateEngine engine{get;}
		}
		public new class ConstArg: AbsConstrainedProcess.ConstArg, IConstArg{
			public ConstArg(
				IProcessManager processManager,
				float expireTime,

				IPlayerInputStateEngine engine

			): base(
				processManager,
				ProcessConstraint.ExpireTime,
				expireTime
			){
				thisEngine = engine;
			}
			readonly IPlayerInputStateEngine thisEngine;
			public IPlayerInputStateEngine engine{get{return thisEngine;}}
		}
	}
}
