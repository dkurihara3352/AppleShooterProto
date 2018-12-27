using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILaunchPointAdaptor: IAppleShooterMonoBehaviourAdaptor{
		ILaunchPoint GetLaunchPoint();
		Vector3 GetMinDrawPosition();
		Vector3 GetMaxDrawPosition();
	}
	public class LaunchPointAdaptor : AppleShooterMonoBehaviourAdaptor, ILaunchPointAdaptor {
		ILaunchPoint thisLaunchPoint;
		public override void SetUp(){
			thisLaunchPoint = CreateLaunchPoint();
		}
		ILaunchPoint CreateLaunchPoint(){
			LaunchPoint.IConstArg arg = new LaunchPoint.ConstArg(
				this
			);
			return new LaunchPoint(arg);
		}
		public ILaunchPoint GetLaunchPoint(){
			return thisLaunchPoint;
		}
		public Transform minDrawPositionTransform;
		public Vector3 GetMinDrawPosition(){
			return  minDrawPositionTransform.localPosition;
		}
		public Transform maxDrawPositionTransform;
		public Vector3 GetMaxDrawPosition(){
			return maxDrawPositionTransform.localPosition;
		}
	}
}
