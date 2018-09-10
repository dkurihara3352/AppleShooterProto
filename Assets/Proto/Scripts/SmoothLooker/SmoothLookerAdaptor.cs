using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface ISmoothLookerAdaptor: IMonoBehaviourAdaptor{
		ISmoothLooker GetSmoothLooker();
		void LookAt(Vector3 position);
		void SetReady();
	}
	public class SmoothLookerAdaptor: MonoBehaviourAdaptor, ISmoothLookerAdaptor{
		public ProcessManager processManager;
		public ISmoothLooker GetSmoothLooker(){
			return thisSmoothLooker;
		}
		ISmoothLooker thisSmoothLooker;
		public float smoothCoefficient = 1f;
		public MonoBehaviourAdaptor lookAtTarget;
		public override void SetUp(){
			IAppleShooterProcessFactory processFactory = new AppleShooterProcessFactory(
				processManager
			);
			ISmoothLookerConstArg arg = new SmoothLookerConstArg(
				this,
				processFactory,
				smoothCoefficient
			);
			thisSmoothLooker = new SmoothLooker(arg);
		}
		public void LookAt(Vector3 position){
			this.transform.LookAt(position, Vector3.up);
		}
		void OnDrawGizmosSelected(){
			DrawLookAtPosition();
		}
		public void SetReady(){
			thisIsReady = true;
		}
		bool thisIsReady = false;
		void DrawLookAtPosition(){
			if(thisIsReady){
				Vector3 lookAtPosition = thisSmoothLooker.GetLookAtPosition();
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(lookAtPosition, 1f);
			}
		}
		public override void SetUpReference(){
			ISmoothLooker looker = GetSmoothLooker();
			looker.SetLookAtTarget(lookAtTarget);
			LookAt(lookAtTarget.GetPosition());
		}
	}
}

