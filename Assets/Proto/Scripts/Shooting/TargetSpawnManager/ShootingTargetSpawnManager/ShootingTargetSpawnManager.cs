using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnManager: IAppleShooterSceneObject{
		float GetSpawnValue(TargetType targetType);
	}
	public class ShootingTargetSpawnManager : AppleShooterSceneObject, IShootingTargetSpawnManager {
		public ShootingTargetSpawnManager(
			IConstArg arg
		): base(
			arg
		){
			thisStaticSpawnValue = arg.staticSpawnValue;
			thisFattySpawnValue = arg.fattySpawnValue;
			thisGliderSpawnValue = arg.gliderSpawnValue;
		}
		float thisStaticSpawnValue;
		float thisFattySpawnValue;
		float thisGliderSpawnValue;
		public float GetSpawnValue(TargetType type){
			switch(type){
				case TargetType.Static:
					return thisStaticSpawnValue;
				case TargetType.Fatty:
					return thisFattySpawnValue;
				case TargetType.Glider:
					return thisGliderSpawnValue;
				default:
					return -1f;
			}
		}

		public new interface IConstArg: AppleShooterSceneObject.IConstArg{
			float staticSpawnValue{get;}
			float fattySpawnValue{get;}
			float gliderSpawnValue{get;}
		}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IShootingTargetSpawnManagerAdaptor adaptor,
				float staticSpawnValue,
				float fattySpawnValue,
				float gliderSpawnValue
			): base(
				adaptor
			){
				thisStaticSpawnValue = staticSpawnValue;
				thisFattySpawnValue = fattySpawnValue;
				thisGliderSpawnValue = gliderSpawnValue;
			}
			readonly float thisStaticSpawnValue;
			public float staticSpawnValue{
				get{return thisStaticSpawnValue;}
			}
			readonly float thisFattySpawnValue;
			public float fattySpawnValue{
				get{return thisFattySpawnValue;}
			}
			readonly float thisGliderSpawnValue;
			public float gliderSpawnValue{
				get{return thisGliderSpawnValue;}
			}
		}
	}
}
