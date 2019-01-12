using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface ITriggeredMotorMechanism: ILevelMechanism, IProcessHandler{
		void Trigger();
	}
	public class TriggeredMotorMechanism: SlickBowShootingSceneObject, ITriggeredMotorMechanism{
		public TriggeredMotorMechanism(IConstArg arg): base(arg){
			thisMotorProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisTriggeredMotorMechanismAdaptor.GetProcessTime()
			);
		}
		IProcessSuite thisMotorProcessSuite;
		public ITriggeredMotorMechanismAdaptor thisTriggeredMotorMechanismAdaptor{
			get{
				return (ITriggeredMotorMechanismAdaptor)thisAdaptor;
			}
		}
		public void Trigger(){
			thisMotorProcessSuite.Start();
		}
		public void OnLevelActivate(){
			return;
		}
		public void OnLevelDeactivate(){
			StopMotorProcess();
			ResetMotor();
		}
		void ResetMotor(){
			UpdatePosition(0f);
			ResetAngle();
		}
		public void OnProcessRun(IProcessSuite suite){
			return;
		}
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisMotorProcessSuite){
				if(thisUpdatesPosition)
					UpdatePosition(normalizedTime);
				if(thisUpdatesRotation)
					ApplyAngle(deltaTime);
			}
		}
		public void OnProcessExpire(
			IProcessSuite suite
		){
			if(suite == thisMotorProcessSuite){
				UpdatePosition(1f);
			}
		}
		void StopMotorProcess(){
			if(thisMotorProcessSuite.IsRunning())
				thisMotorProcessSuite.Stop();
		}
		
		bool thisUpdatesPosition{
			get{
				return thisTriggeredMotorMechanismAdaptor.UpdatesPosition();
			}
		}
		bool thisUpdatesRotation{
			get{
				return thisTriggeredMotorMechanismAdaptor.UpdatesRotation();
			}
		}
		void UpdatePosition(float normalizedTime){
			Vector3 initialPosition = thisTriggeredMotorMechanismAdaptor.GetInitialPosition();
			Vector3 targetPosition = thisTriggeredMotorMechanismAdaptor.GetTargetPosition();
			Vector3 newPosition = Vector3.Lerp(
				initialPosition,
				targetPosition,
				normalizedTime
			);
			thisTriggeredMotorMechanismAdaptor.SetMechanismLocalPosition(newPosition);
		}
		void ApplyAngle(float deltaTime){
			thisTriggeredMotorMechanismAdaptor.ApplyAngleOnEachAxis(deltaTime);
		}
		void ResetAngle(){
			thisTriggeredMotorMechanismAdaptor.ResetAngle();
		}
	}
}

