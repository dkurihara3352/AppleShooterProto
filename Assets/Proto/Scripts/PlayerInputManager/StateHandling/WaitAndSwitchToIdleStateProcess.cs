using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IWaitAndSwitchToIdleStateProcess: IProcess{

	}
	public class WaitAndSwitchToIdleStateProcess: AbsConstrainedProcess, IWaitAndSwitchToIdleStateProcess{
		public WaitAndSwitchToIdleStateProcess(
			IWaitAndSwitchToIdleStateProcessConstArg arg
		): base(
			arg
		){
			thisEngine = arg.engine;
		}
		IPlayerInputStateEngine thisEngine;
		protected override void ExpireImple(){
			thisEngine.SwitchToIdleState();
		}
	}

	public interface IWaitAndSwitchToIdleStateProcessConstArg: IConstrainedProcessConstArg{
		IPlayerInputStateEngine engine{get;}
	}
	public class WaitAndSwitchToIdleStateProcessCosntArg: ConstrainedProcessConstArg, IWaitAndSwitchToIdleStateProcessConstArg{
		public WaitAndSwitchToIdleStateProcessCosntArg(
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
