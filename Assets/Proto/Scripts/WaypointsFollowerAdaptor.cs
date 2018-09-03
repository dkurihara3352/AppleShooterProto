using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IMonoBehaviourAdaptor{
		Vector3 GetPosition();
		void SetPosition(Vector3 position);
		
	}
	public interface IWaypointsFollowerAdaptor: IMonoBehaviourAdaptor{
		IWaypointsFollower GetWaypointsFollower();
	}
	public class WaypointsFollowerAdaptor : MonoBehaviour, IWaypointsFollowerAdaptor {
		void Initialize(){
			thisProcessFactory = new AppleShooterProcessFactory(
				processManager
			);
		}
		public ProcessManager processManager;
		IWaypointsFollower thisFollower;
		public IWaypointsFollower GetWaypointsFollower(){
			return thisFollower;
		}
		IAppleShooterProcessFactory thisProcessFactory;
		public float followSpeed;
		void SetUpWaypointsFollower(){
			IWaypointsFollowerConstArg arg = new WaypointsFollowerConstArg(
				this, 
				thisProcessFactory,
				followSpeed
			);
			thisFollower = new WaypointsFollower(arg);
		}
		public Vector3 GetPosition(){
			return this.transform.position;
		}
		public void SetPosition(Vector3 position){
			this.transform.position = position;
		}
	}
}
