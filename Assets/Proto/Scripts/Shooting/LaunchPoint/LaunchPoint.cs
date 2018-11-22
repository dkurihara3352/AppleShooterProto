using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILaunchPoint: IAppleShooterSceneObject{
		Vector3 GetForwardDirection();
	}
	public class LaunchPoint : AppleShooterSceneObject, ILaunchPoint {
		public LaunchPoint(
			IConstArg arg
		): base(arg){
		}
		public Vector3 GetForwardDirection(){
			return thisAdaptor.GetForwardDirection();
		}
		public new interface IConstArg: AppleShooterSceneObject.IConstArg{
		}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				ILaunchPointAdaptor adaptor
			): base(
				adaptor
			){
			}
		}
	}
}
