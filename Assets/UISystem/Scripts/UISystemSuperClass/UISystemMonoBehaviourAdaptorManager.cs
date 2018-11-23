using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace UISystem{
	public interface IUISystemMonoBehaviourAdaptorManager: IMonoBehaviourAdaptorManager{
		IUISystemProcessFactory GetUISystemProcessFactory();
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
				uiManager
			);
		}
		public IUISystemProcessFactory GetUISystemProcessFactory(){
			return thisProcessFactory;
		}
		public override IUnityBaseProcessFactory GetProcessFactory(){
			return thisProcessFactory;
		}
	}
}
