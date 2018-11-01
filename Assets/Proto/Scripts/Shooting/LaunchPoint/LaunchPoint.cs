using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILaunchPoint: ISceneObject{
		Vector3 GetForwardDirection();
	}
	public class LaunchPoint : AbsSceneObject, ILaunchPoint {
		public LaunchPoint(
			IConstArg arg
		): base(arg){
		}
		public Vector3 GetForwardDirection(){
			return thisAdaptor.GetForwardDirection();
		}
		public new interface IConstArg: AbsSceneObject.IConstArg{
		}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				ILaunchPointAdaptor adaptor
			): base(
				adaptor
			){
			}
		}
	}
}
