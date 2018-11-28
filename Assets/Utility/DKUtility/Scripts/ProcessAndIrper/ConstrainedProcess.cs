using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DKUtility{
	public enum ProcessConstraint{
		none,
		RateOfChange,
		ExpireTime
	}
	public abstract class AbsConstrainedProcess: AbsProcess{
		public AbsConstrainedProcess(
			IConstArg arg
		): base(
			arg
		){
			thisProcessConstraint = arg.processConstraint;
			thisConstraintValue = arg.constraintValue;
		}
		readonly ProcessConstraint thisProcessConstraint;
		readonly protected float thisConstraintValue;
		protected virtual float GetLatestInitialValueDifference(){
			if(thisProcessConstraint == ProcessConstraint.RateOfChange)
				throw new System.InvalidOperationException("Override GetLatesetInitialValueDifference if the process constraint is rate of change");
			return 0f;
		}
		public override void Run(){
			CalcAndSetConstraintValues();
			base.Run();
		}
		void CalcAndSetConstraintValues(){
			if(thisProcessConstraint == ProcessConstraint.RateOfChange){
				thisRateOfChange = thisConstraintValue;
				float valueDiff = GetLatestInitialValueDifference();
				if(valueDiff == 0f)
					thisExpireTime = 0f;
				else
					thisExpireTime = valueDiff / thisRateOfChange;
				if(thisExpireTime < 0f)
					thisExpireTime *= -1f;
			}else if(thisProcessConstraint == ProcessConstraint.ExpireTime){
				float constVal = thisConstraintValue;
				if(constVal < 0f)
					constVal *= -1f;
				thisExpireTime = constVal;
			}else{
				return;
			}
		}
		protected float thisElapsedTime;
		protected float thisExpireTime;
		protected float thisRateOfChange;
		protected float thisNormalizedTime{
			get{return thisElapsedTime / thisExpireTime;}
		}
		sealed public override void UpdateProcess(float deltaT){
			thisElapsedTime += deltaT;
			if(thisProcessConstraint != ProcessConstraint.none){
				if(thisElapsedTime >= thisExpireTime){
					Expire();
					return;
				}
			}
			UpdateProcessImple(deltaT);
		}
		public new interface IConstArg: AbsProcess.IConstArg{
			ProcessConstraint processConstraint{get;}
			float constraintValue{get;}
		}
		public new class ConstArg: AbsProcess.ConstArg, IConstArg{
			public ConstArg(
				IProcessManager processManager,
				ProcessConstraint processConstraint,
				float constraintValue
			): base(
				processManager
			){
				thisProcessConstraint = processConstraint;
				thisConstraintValue = constraintValue;
			}
			readonly ProcessConstraint thisProcessConstraint;
			public ProcessConstraint processConstraint{get{return thisProcessConstraint;}}
			readonly float thisConstraintValue;
			public float constraintValue{get{return thisConstraintValue;}}
		}
	}

	public class GenericWaitAndExpireProcess: AbsConstrainedProcess{
		public GenericWaitAndExpireProcess(
			IConstArg arg
		): base(
			arg	
		){}
		public new interface IConstArg: AbsConstrainedProcess.IConstArg{}
		public class GenericWaitAndExpireProcessConstArg: AbsProcess.ConstArg, IConstArg{
			public GenericWaitAndExpireProcessConstArg(
				IProcessManager processManager,
				float expireTime
			): base(
				processManager
			){
				thisExpireTime = expireTime;
			}
			public ProcessConstraint processConstraint{
				get{
					return ProcessConstraint.ExpireTime;
				}
			}
			readonly float thisExpireTime;
			public float constraintValue{
				get{
					return thisExpireTime;
				}
			}
		} 
	}
}

