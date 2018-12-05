using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
namespace UnityBase{
	public interface IFrostGlass: ISceneObject, IProcessHandler{
		void Frost();
		void Defrost();
	}
	public class FrostGlass : AbsSceneObject, IFrostGlass {

		public FrostGlass(
			IConstArg arg
		): base(arg){
			thisFrostProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisTypedAdaptor.GetProcessTime()
			);
			thisDefrostProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisTypedAdaptor.GetProcessTime()
			);
		}
		IFrostGlassAdaptor thisTypedAdaptor{
			get{
				return (IFrostGlassAdaptor)thisAdaptor;
			}
		}
		IProcessSuite thisFrostProcessSuite;
		IProcessSuite thisDefrostProcessSuite;

		public void Frost(){
			thisDefrostProcessSuite.Stop();
			thisFrostProcessSuite.Start();
		}
		public void Defrost(){
			thisFrostProcessSuite.Stop();
			thisDefrostProcessSuite.Start();
		}
		public void OnProcessRun(IProcessSuite suite){
			thisInitialFrostValue = thisTypedAdaptor.GetFrostValue();
			if(suite == thisFrostProcessSuite)
				thisTargetFrostValue = 1f;
			else
				thisTargetFrostValue = 0f;

			Debug.Log(
				"ini: " + thisInitialFrostValue.ToString() + ", " +
				"tar: " + thisTargetFrostValue.ToString()
			);
		}
		float thisInitialFrostValue;
		float thisTargetFrostValue;
		public void OnProcessExpire(IProcessSuite suite){
			OnProcessUpdate(
				0f,
				1f,
				suite
			);
		}
		public void OnProcessUpdate(float deltaTime, float normalizedTime, IProcessSuite suite){
			float newFrostValue = Mathf.Lerp(
				thisInitialFrostValue,
				thisTargetFrostValue,
				normalizedTime
			);
			thisTypedAdaptor.SetFrostValue(newFrostValue);
		}

		public new interface IConstArg: AbsSceneObject.IConstArg{}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IFrostGlassAdaptor adaptor
			): base(
				adaptor
			){}
		}
	}
}
