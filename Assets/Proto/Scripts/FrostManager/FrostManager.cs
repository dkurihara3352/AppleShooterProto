using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface IFrostManager: ISlickBowShootingSceneObject, IProcessHandler{
		void Frost(float time);
		void Defrost(float time);
	}
	public class FrostManager: SlickBowShootingSceneObject, IFrostManager{
		public FrostManager(IConstArg arg): base(arg){
			thisProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisFrostManagerAdaptor.GetProcessTime()
			);
		}
		IFrostManagerAdaptor thisFrostManagerAdaptor{
			get{
				return (IFrostManagerAdaptor)thisAdaptor;
			}
		}
		public void Frost(float time){
			thisProcessSuite.SetConstraintValue(time);
			thisFrsots = true;
			thisProcessSuite.Start();
		}
		public void Defrost(float time){
			thisProcessSuite.SetConstraintValue(time);
			thisFrsots = false;
			thisProcessSuite.Start();
		}
		bool thisFrsots = false;
		IProcessSuite thisProcessSuite;
		public void OnProcessRun(IProcessSuite suite){
			if(suite == thisProcessSuite){
				thisInitialFrostValue = thisFrostManagerAdaptor.GetFrostValue();
			}
		}
		float thisInitialFrostValue;
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisProcessSuite){
				float targetFrostValue;
				if(thisFrsots)
					targetFrostValue = 1f;
				else
					targetFrostValue = 0f;
				
				AnimationCurve processCurve = thisFrostManagerAdaptor.GetProcessCurve();
				float processValue = processCurve.Evaluate(normalizedTime);

				float newFrostValue = Mathf.Lerp(
					thisInitialFrostValue,
					targetFrostValue,
					processValue
				);

				thisFrostManagerAdaptor.SetFrostValue(newFrostValue);
			}
		}
		public void OnProcessExpire(IProcessSuite suite){
			if(suite == thisProcessSuite)
				if(thisFrsots)
					thisFrostManagerAdaptor.SetFrostValue(1f);
				else
					thisFrostManagerAdaptor.SetFrostValue(0f);
		}
	}
}


