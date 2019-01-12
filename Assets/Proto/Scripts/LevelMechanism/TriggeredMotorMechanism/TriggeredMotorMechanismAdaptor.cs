using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface ITriggeredMotorMechanismAdaptor: ILevelMechanismAdaptor{
		ITriggeredMotorMechanism GetTriggeredMotorMechanism();
		float GetProcessTime();
		bool UpdatesPosition();
		bool UpdatesRotation();
		Vector3 GetInitialPosition();
		Vector3 GetTargetPosition();
		void ApplyAngleOnEachAxis(float deltaTime);
		void SetMechanismLocalPosition(Vector3 position);
		void ResetAngle();
	}
	public class TriggeredMotorMechanismAdaptor: SlickBowShootingMonoBehaviourAdaptor, ITriggeredMotorMechanismAdaptor{
		public override void SetUp(){
			thisTriggeredMotorMechanism = CreateTriggeredMotorMechanism();
			thisInitialRotation = motorTransform.localRotation;
		}
		ITriggeredMotorMechanism thisTriggeredMotorMechanism;
		public ITriggeredMotorMechanism GetTriggeredMotorMechanism(){
			return thisTriggeredMotorMechanism;
		}
		ITriggeredMotorMechanism CreateTriggeredMotorMechanism(){
			TriggeredMotorMechanism.IConstArg arg = new TriggeredMotorMechanism.ConstArg(this);
			return new TriggeredMotorMechanism(arg);
		}
		public ILevelMechanism GetLevelMechanism(){
			return thisTriggeredMotorMechanism;
		}
		public float processTime = 10f;
		public float GetProcessTime(){
			return processTime;
		}
		public bool updatesPosition = true;
		public bool UpdatesPosition(){
			return updatesPosition;
		}
		public bool updatesRotation = false;
		public bool UpdatesRotation(){
			return updatesRotation;
		}
		public Transform initialLocalPositionTrans;
		public Vector3 GetInitialPosition(){
			return initialLocalPositionTrans.localPosition;
		}
		public Transform targetLocalPositionTrans;
		public Vector3 GetTargetPosition(){
			return targetLocalPositionTrans.localPosition;
		}

		public void SetMechanismLocalPosition(Vector3 position){
			motorTransform.localPosition = position;
		}
		public Transform motorTransform;
		public void ResetAngle(){
			motorTransform.localRotation = thisInitialRotation;
		}
		Quaternion thisInitialRotation;

		public void ApplyAngleOnEachAxis(float deltaTime){
			Vector3 deltaAngles = anglesPerSecond * deltaTime;
			motorTransform.Rotate(Vector3.right, deltaAngles.x, Space.Self);
			motorTransform.Rotate(Vector3.up, deltaAngles.y, Space.Self);
			motorTransform.Rotate(Vector3.forward, deltaAngles.z, Space.Self);
		}
		public Vector3 anglesPerSecond;
	}	
}

