using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DKUtility{
	public interface IProcessSuite{
		void Start();
		void Stop();
		void SetConstraintValue(float value);
	}
	public class ProcessSuite: IProcessSuite{
		public ProcessSuite(
			IProcessManager processManager,
			IProcessHandler handler,
			ProcessConstraint processConstraint,
			float constraintValue
		){
			thisProcessManager = processManager;
			thisHandler = handler;
			thisProcessConstraint = processConstraint;
			thisValue = constraintValue;
		}
		readonly IProcessManager thisProcessManager;
		readonly IProcessHandler thisHandler;
		readonly ProcessConstraint thisProcessConstraint;
		float thisValue;
		public void SetConstraintValue(float value){
			thisValue = value;
		}

		public void Start(){
			Stop();
			Process.IConstArg arg = new Process.ConstArg(
				thisProcessManager,
				thisProcessConstraint,
				thisValue,
				thisHandler,
				this
			);
			thisProcess = new Process(arg);
			thisProcess.Run();
		}
		public void Stop(){
			if(thisProcess != null && thisProcess.IsRunning()){
				thisProcess.Stop();
			}
			thisProcess = null;
		}
		IProcess thisProcess;

		/* process */
			public interface IProcess: DKUtility.IProcess{
			}
			public class Process: DKUtility.AbsConstrainedProcess, IProcess{
				public Process(
					IConstArg arg
				):base(
					arg
				){
					thisHandler = arg.handler;
					thisProcessSuite = arg.suite;
				}
				readonly IProcessHandler thisHandler;
				readonly IProcessSuite thisProcessSuite;
				protected override void RunImple(){
					thisHandler.OnProcessRun(thisProcessSuite);
				}
				protected override void UpdateProcessImple(float deltaT){
					thisHandler.OnProcessUpdate(deltaT, thisNormalizedTime, thisProcessSuite);
				}
				protected override void ExpireImple(){
					thisHandler.OnProcessExpire(thisProcessSuite);
				}
				/* Const */
					public new interface IConstArg: AbsConstrainedProcess.IConstArg{
						IProcessHandler handler{get;}
						IProcessSuite suite{get;}
					}
					public new class ConstArg: AbsConstrainedProcess.ConstArg, IConstArg{
						public ConstArg(
							IProcessManager processManager,
							ProcessConstraint constraint,
							float value,
							IProcessHandler handler,
							IProcessSuite suite
						): base(
							processManager,
							constraint,
							value
						){
							thisHandler = handler;
							thisSuite = suite;
						}
						readonly IProcessHandler thisHandler;
						public IProcessHandler handler{get{return thisHandler;}}
						readonly IProcessSuite thisSuite;
						public IProcessSuite suite{get{return thisSuite;}}
						
					}
			}
	}
	public interface IProcessHandler{
		void OnProcessRun(IProcessSuite processSuite);
		void OnProcessUpdate(
			float deltaTime,
			float normalizedTime, 
			IProcessSuite processSuite
		);
		void OnProcessExpire(IProcessSuite processSuite);
	}	
}
