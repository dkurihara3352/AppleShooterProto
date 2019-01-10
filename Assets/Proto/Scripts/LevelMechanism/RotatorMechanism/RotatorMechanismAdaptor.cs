using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IRotatorMechanismAdaptor: ILevelMechanismAdaptor{
		IRotatorMechanism GetRotatorMechanism();
		bool RandomizesInitialRotation();
		void SetEulerAngleOnRotationAxis(float angle);
		void RotateMechanism(float deltaTime);
	}
	public class RotatorMechanismAdaptor: SlickBowShootingMonoBehaviourAdaptor, IRotatorMechanismAdaptor{
		public override void SetUp(){
			thisRotatorMechanism = CreateRotatorMechanism();
		}
		IRotatorMechanism thisRotatorMechanism;
		public ILevelMechanism GetLevelMechanism(){
			return thisRotatorMechanism;
		}
		public IRotatorMechanism GetRotatorMechanism(){
			return thisRotatorMechanism;
		}
		IRotatorMechanism CreateRotatorMechanism(){
			RotatorMechanism.IConstArg arg = new RotatorMechanism.ConstArg(
				this
			);
			return new RotatorMechanism(arg);
		}
		public bool RandomizesInitialRotation(){
			return randomizesInitialRotation;
		}
		public bool randomizesInitialRotation = true;
		public Transform rotationTransform;
		public void SetEulerAngleOnRotationAxis(float angle){
			Vector3 eulerAngles = rotationTransform.localEulerAngles;
			eulerAngles.y = angle;
			rotationTransform.localEulerAngles = eulerAngles;
		}
		public void RotateMechanism(float deltaTime){
			float angle = deltaTime * anglePerSecond;
			this.rotationTransform.Rotate(Vector3.up, angle, Space.Self);
		}
		public float anglePerSecond = 10f;
		
	}
}

