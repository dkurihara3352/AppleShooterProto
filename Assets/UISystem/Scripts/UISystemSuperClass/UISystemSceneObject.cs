using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace UISystem{
	public interface IUISystemSceneObject: ISceneObject{}
	public class UISystemSceneObject : AbsSceneObject, IUISystemSceneObject {
		public UISystemSceneObject(
			IConstArg arg
		): base(arg){

		}

		protected IUISystemMonoBehaviourAdaptorManager thisUISystemMonoBehaviourAdaptorManager{
			get{
				return (IUISystemMonoBehaviourAdaptorManager)thisMonoBehaviourAdaptorManager;
			}
		}
		protected IUISystemProcessFactory thisUISystemProcessFactory{
			get{
				return thisUISystemMonoBehaviourAdaptorManager.GetUISystemProcessFactory();
			}
		}


		public new interface IConstArg: AbsSceneObject.IConstArg{}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IUISystemMonoBehaviourAdaptor adaptor
			): base(adaptor){

			}
		}
	}
}
