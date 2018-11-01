using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IDestroyedTargetParticleProcess: IProcess{

	}
	public class DestroyedTargetParticleProcess: AbsConstrainedProcess, IDestroyedTargetParticleProcess{
		public DestroyedTargetParticleProcess(
			IConstArg arg
		): base(
			arg
		){
			thisAdaptor = arg.adaptor;
		}
		IDestroyedTargetAdaptor thisAdaptor;
		protected override void RunImple(){
			thisAdaptor.PlayParticleSystem();
		}
		protected override void StopImple(){
			thisAdaptor.StopParticleSystem();
		}
		/*  */
		public interface IConstArg: IConstrainedProcessConstArg{
			IDestroyedTargetAdaptor adaptor{get;}
		}
		public class ConstArg: ConstrainedProcessConstArg, IConstArg{
			public ConstArg(
				IDestroyedTargetAdaptor adaptor,
				IProcessManager processManager,
				float particleSystemTotalDuration
			): base(
				processManager,
				ProcessConstraint.ExpireTime,
				particleSystemTotalDuration
			){
				thisAdaptor = adaptor;
			}
			readonly IDestroyedTargetAdaptor thisAdaptor;
			public IDestroyedTargetAdaptor adaptor{get{return thisAdaptor;}}
		}
	}
}

