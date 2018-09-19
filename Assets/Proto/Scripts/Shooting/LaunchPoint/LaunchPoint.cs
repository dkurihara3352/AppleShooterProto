using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILaunchPoint{
		Vector3 GetWorldDirection();
		Vector3 GetWorldPosition();
	}
	public class LaunchPoint : ILaunchPoint {

		public LaunchPoint(ILaunchPointConstArg arg){
			thisAdaptor = arg.adaptor;
		}
		readonly ILaunchPointAdaptor thisAdaptor;
		public Vector3 GetWorldDirection(){
			return thisAdaptor.GetWorldDirection();
		}
		public Vector3 GetWorldPosition(){
			return thisAdaptor.GetWorldPosition();
		}
	}



	public interface ILaunchPointConstArg{
		ILaunchPointAdaptor adaptor{get;}
	}
	public struct LaunchPointConstArg: ILaunchPointConstArg{
		public LaunchPointConstArg(
			ILaunchPointAdaptor adaptor
		){
			thisAdaptor = adaptor;
		}
		readonly ILaunchPointAdaptor thisAdaptor;
		public ILaunchPointAdaptor adaptor{get{return thisAdaptor;}}
	}
}
