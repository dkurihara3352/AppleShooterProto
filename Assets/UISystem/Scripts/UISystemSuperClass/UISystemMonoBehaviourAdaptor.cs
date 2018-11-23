using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
namespace UISystem{
	public interface IUISystemMonoBehaviourAdaptor: IMonoBehaviourAdaptor{
		IUISystemMonoBehaviourAdaptorManager GetUISystemMonoBehaviourAdaptorManager();
	}
	public class UISystemMonoBehaviourAdaptor : MonoBehaviourAdaptor, IUISystemMonoBehaviourAdaptor {

		protected IUISystemMonoBehaviourAdaptorManager thisUISystemMonoBehaviourAdaptorManager{
			get{
				return (IUISystemMonoBehaviourAdaptorManager)thisMonoBehaviourAdaptorManager;
			}
		}
		public IUISystemMonoBehaviourAdaptorManager GetUISystemMonoBehaviourAdaptorManager(){
			return thisUISystemMonoBehaviourAdaptorManager;
		}
		
		protected IUISystemProcessFactory thisUISystemProcessFactory{
			get{
				return thisUISystemMonoBehaviourAdaptorManager.GetUISystemProcessFactory();
			}
		}
	}
}
