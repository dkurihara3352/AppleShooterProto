using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace AppleShooterProto{
	public interface IAppleShooterSceneObject: ISceneObject{
	}
	public abstract class AppleShooterSceneObject : AbsSceneObject, IAppleShooterSceneObject {

		public AppleShooterSceneObject(
			IConstArg arg
		): base(arg){}
		protected IAppleShooterMonoBehaviourAdaptorManager thisAppleShooterMonoBehaviourAdaptorManager{
			get{
				return (IAppleShooterMonoBehaviourAdaptorManager)thisMonoBehaviourAdaptorManager;
			}
		}
		protected IAppleShooterProcessFactory thisAppleShooterProcessFactory{
			get{
				return thisAppleShooterMonoBehaviourAdaptorManager.GetAppleShooterProcessFactory();
			}
		}
		public new interface IConstArg: AbsSceneObject.IConstArg{}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IAppleShooterMonoBehaviourAdaptor adaptor
			): base(adaptor){}
		}
	}
}
