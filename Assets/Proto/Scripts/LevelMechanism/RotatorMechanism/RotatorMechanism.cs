using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface IRotatorMechanism: ILevelMechanism, IProcessHandler{}
	public class RotatorMechanism: SlickBowShootingSceneObject, IRotatorMechanism{
		public RotatorMechanism(IConstArg arg): base(arg){
			thisRotateProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.None,
				0f
			);
		}
		IRotatorMechanismAdaptor thisRotatorMechanismAdaptor{
			get{
				return (IRotatorMechanismAdaptor)thisAdaptor;
			}
		}
		IProcessSuite thisRotateProcessSuite;
		public void OnLevelActivate(){
			StartRotation();
		}
		void StartRotation(){
			thisRotateProcessSuite.Start();
		}
		public void OnProcessRun(IProcessSuite suite){
			if(suite == thisRotateProcessSuite){
				if(thisRotatorMechanismAdaptor.RandomizesInitialRotation()){
					float randomRotation = Random.Range(0f, 1f) * 360f;
					thisRotatorMechanismAdaptor.SetEulerAngleOnRotationAxis(randomRotation);
				}
			}
		}
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisRotateProcessSuite)
				thisRotatorMechanismAdaptor.RotateMechanism(deltaTime);
		}
		public void OnProcessExpire(IProcessSuite suite){
			return;
		}
		public void OnLevelDeactivate(){
			StopRotation();
		}
		void StopRotation(){
			if(thisRotateProcessSuite.IsRunning())
				thisRotateProcessSuite.Stop();
		}
	}
}

