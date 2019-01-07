using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace SlickBowShooting{
	public interface ISlickBowShootingSceneObject: ISceneObject{
	}
	public abstract class SlickBowShootingSceneObject : AbsSceneObject, ISlickBowShootingSceneObject {

		public SlickBowShootingSceneObject(
			IConstArg arg
		): base(arg){}
		protected ISlickBowShootingMonoBehaviourAdaptorManager thisSlickBowShootingMonoBehaviourAdaptorManager{
			get{
				return (ISlickBowShootingMonoBehaviourAdaptorManager)thisMonoBehaviourAdaptorManager;
			}
		}
		protected ISlickBowShootingProcessFactory thisSlickBowShootingProcessFactory{
			get{
				return thisSlickBowShootingMonoBehaviourAdaptorManager.GetSlickBowShootingProcessFactory();
			}
		}
		public new interface IConstArg: AbsSceneObject.IConstArg{}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				ISlickBowShootingMonoBehaviourAdaptor adaptor
			): base(adaptor){}
		}
	}
}
