using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface ISmoothFollowerAdaptor{
		Vector3 GetPosition();
		void SetPosition(Vector3 position);
	}
	public class SmoothFollowerAdaptor : MonoBehaviour, ISmoothFollowerAdaptor {
		public Transform targetTransform;
		public ProcessManager processManager;
		public void CreateAndSetSmoothFollower(){
			IAppleShooterProcessFactory processFactory = new AppleShooterProcessFactory(
				processManager
			);
			ISmoothFollowerConstArg arg = new SmoothFollowerConstArg(
				this,
				processFactory,
				smoothCoefficient
			);
			thisSmoothFollower = new SmoothFollower(arg);
			ISmoothFollowTargetMBAdaptor target = (ISmoothFollowTargetMBAdaptor)targetTransform.GetComponent(typeof(ISmoothFollowTargetMBAdaptor));
			thisSmoothFollower.SetFollowTarget(target);
		}
		public float smoothCoefficient;
		ISmoothFollower thisSmoothFollower;
		public ISmoothFollower GetSmoothFollower(){
			return thisSmoothFollower;
		}
		public Vector3 GetPosition(){
			return this.transform.position;
		}
		public void SetPosition(Vector3 position){
			this.transform.position = position;
		}
	}
}
