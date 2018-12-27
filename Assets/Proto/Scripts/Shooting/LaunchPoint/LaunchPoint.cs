using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILaunchPoint: IAppleShooterSceneObject{
		Vector3 GetForwardDirection();
		void SetDrawPosition(float drawAmount);
	}
	public class LaunchPoint : AppleShooterSceneObject, ILaunchPoint {
		public LaunchPoint(
			IConstArg arg
		): base(arg){
		}
		ILaunchPointAdaptor thisLaunchPointAdaptor{
			get{
				return (ILaunchPointAdaptor)thisAdaptor;
			}
		}
		public Vector3 GetForwardDirection(){
			return thisAdaptor.GetForwardDirection();
		}
		public void SetDrawPosition(float drawAmount){
			Vector3 minDrawPosition = thisLaunchPointAdaptor.GetMinDrawPosition();
			Vector3 maxDrawPosition = thisLaunchPointAdaptor.GetMaxDrawPosition();

			Vector3 newDrawPosition = Vector3.Lerp(
				minDrawPosition,
				maxDrawPosition,
				drawAmount
			);
			this.SetLocalPosition(newDrawPosition);
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
