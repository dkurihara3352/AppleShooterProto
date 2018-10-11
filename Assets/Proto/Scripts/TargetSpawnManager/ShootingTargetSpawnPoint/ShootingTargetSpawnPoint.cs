using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnPoint{
		Vector3 GetPosition();
		Quaternion GetRotation();
		Transform GetTransform();
	}
	public class ShootingTargetSpawnPoint: IShootingTargetSpawnPoint {
		public ShootingTargetSpawnPoint(
			IConstArg arg
		){
			thisAdaptor = arg.adaptor;
		}
		readonly IShootingTargetSpawnPointAdaptor thisAdaptor;
		public Vector3 GetPosition(){
			return thisAdaptor.GetPosition();
		}
		public Quaternion GetRotation(){
			return thisAdaptor.GetRotation();
		}
		public Transform GetTransform(){
			return thisAdaptor.GetTransform();
		}
		/*  */
		public interface IConstArg{
			IShootingTargetSpawnPointAdaptor adaptor{get;}
		}
		public struct ConstArg: IConstArg{
			public ConstArg(
				IShootingTargetSpawnPointAdaptor adaptor
			){
				thisAdaptor = adaptor;
			}
			readonly IShootingTargetSpawnPointAdaptor thisAdaptor;
			public IShootingTargetSpawnPointAdaptor adaptor{get{return thisAdaptor;}}
		}
	}
}
