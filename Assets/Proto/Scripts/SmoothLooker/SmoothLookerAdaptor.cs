using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
using UnityBase;

namespace AppleShooterProto{
	public interface ISmoothLookerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		ISmoothLooker GetSmoothLooker();
		void LookAt(Vector3 position);
	}
	public class SmoothLookerAdaptor: AppleShooterMonoBehaviourAdaptor, ISmoothLookerAdaptor{
		public float smoothCoefficient = 1f;
		public MonoBehaviourAdaptor lookAtTarget;
		public int processOrder = 100;
		public override void SetUp(){
			thisSmoothLooker = CreateSmoothLooker();
		}
		public ISmoothLooker GetSmoothLooker(){
			return thisSmoothLooker;
		}
		ISmoothLooker thisSmoothLooker;
		ISmoothLooker CreateSmoothLooker(){
			SmoothLooker.IConstArg arg = new SmoothLooker.ConstArg(
				this,
				smoothCoefficient,
				processOrder
			);
			return new SmoothLooker(arg);
		}

		public void LookAt(Vector3 position){
			this.transform.LookAt(position, Vector3.up);
		}
		// public void SetReady(){
		// 	thisIsReady = true;
		// }
		// bool thisIsReady = false;
		public override void SetUpReference(){
			ISmoothLooker looker = GetSmoothLooker();
			if(lookAtTarget != null){
				looker.SetLookAtTarget(lookAtTarget);
				LookAt(lookAtTarget.GetPosition());
			}
		}
	}
}

