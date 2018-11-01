using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILaunchPointAdaptor: IMonoBehaviourAdaptor{
		ILaunchPoint GetLaunchPoint();
	}
	public class LaunchPointAdaptor : MonoBehaviourAdaptor, ILaunchPointAdaptor {
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
	}
}
