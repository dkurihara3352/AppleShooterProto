using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace UISystem{
	public interface IUISystemMonoBehaviourAdaptorManager: IMonoBehaviourAdaptorManager{
		IUISystemProcessFactory GetUISystemProcessFactory();
		IUIManager GetUIManager();
	}
	public class UISystemMonoBehaviourAdaptorManager : MonoBehaviourAdaptorManager, IUISystemMonoBehaviourAdaptorManager {

		void Awake(){
			thisProcessFactory = CreateProcessFactory();
		}
		IUISystemProcessFactory thisProcessFactory;
		public IUIManager uiManager;
		IUISystemProcessFactory CreateProcessFactory(){
			return new UISystemProcessFactory(
				processManager,
				this
			);
		}
		public IUISystemProcessFactory GetUISystemProcessFactory(){
			return thisProcessFactory;
		}
		public override IUnityBaseProcessFactory GetProcessFactory(){
			return thisProcessFactory;
		}
		public UIManagerAdaptor uiManagerAdaptor;
		public IUIManager GetUIManager(){
			return uiManagerAdaptor.GetUIManager();
		}
	}
}
