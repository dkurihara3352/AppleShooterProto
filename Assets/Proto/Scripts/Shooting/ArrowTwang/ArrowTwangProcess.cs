using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface IArrowTwangProcess: IProcess{}
	public class ArrowTwangProcess: AbsConstrainedProcess, IArrowTwangProcess{
		public ArrowTwangProcess(
			IConstArg arg
		): base(
			arg
		){
			thisAdaptor = arg.adaptor;
		}
		IArrowTwangAdaptor thisAdaptor;
		protected override void RunImple(){
			thisAdaptor.StartTwangAnimation();
		}
		protected override void UpdateProcessImple(float deltaT){
			float twangMagnitude = thisAdaptor.GetTwangMagnitude(
				thisNormalizedTime
			);
			thisAdaptor.SetTwangMagnitude(twangMagnitude);
		}
		protected override void StopImple(){
			thisAdaptor.StopTwangAnimation();
		}
		/*  */
		public new interface IConstArg: AbsConstrainedProcess.IConstArg{
			IArrowTwangAdaptor adaptor{get;}
		}
		public new class ConstArg: AbsConstrainedProcess.ConstArg, IConstArg{
			public ConstArg(
				IProcessManager processManager,
				float twangTime,
				IArrowTwangAdaptor adaptor
			): base(
				processManager,
				ProcessConstraint.ExpireTime,
				twangTime
			){
				thisAdaptor = adaptor;
			}
			readonly IArrowTwangAdaptor thisAdaptor;
			public IArrowTwangAdaptor adaptor{get{return thisAdaptor;}}
		}

		
	}
}
