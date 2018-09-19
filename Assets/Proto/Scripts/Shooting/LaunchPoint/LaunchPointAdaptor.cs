using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILaunchPointAdaptor: IMonoBehaviourAdaptor{
		Vector3 GetWorldDirection();
		Vector3 GetWorldPosition();
		ILaunchPoint GetLaunchPoint();
	}
	public class LaunchPointAdaptor : MonoBehaviourAdaptor, ILaunchPointAdaptor {
		ILaunchPoint thisLaunchPoint;
		public override void SetUp(){
			ILaunchPointConstArg arg = new LaunchPointConstArg(this);
			thisLaunchPoint = new LaunchPoint(arg);
		}
		public ILaunchPoint GetLaunchPoint(){
			return thisLaunchPoint;
		}
		public Vector3 GetWorldDirection(){
			return this.transform.forward;
		}
		public Vector3 GetWorldPosition(){
			return this.transform.position;
		}

		void OnDrawGizmos(){
			Gizmos.color = Color.yellow;
			Vector3 worldPos = this.GetWorldPosition();
			Gizmos.DrawLine(
				worldPos,
				worldPos + GetWorldDirection() * 10f
			);
		}
	}
}
