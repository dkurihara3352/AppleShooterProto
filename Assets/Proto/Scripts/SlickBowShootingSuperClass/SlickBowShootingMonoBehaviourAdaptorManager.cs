using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
using UISystem;

namespace SlickBowShooting{
	public interface ISlickBowShootingMonoBehaviourAdaptorManager: IUISystemMonoBehaviourAdaptorManager{
		ISlickBowShootingProcessFactory GetSlickBowShootingProcessFactory();
	}
	public class SlickBowShootingMonoBehaviourAdaptorManager : MonoBehaviourAdaptorManager, ISlickBowShootingMonoBehaviourAdaptorManager {
		void Awake(){
			thisProcessFactory = CreateProcessFactory();
		}
		ISlickBowShootingProcessFactory CreateProcessFactory(){
			return new SlickBowShootingProcessFactory(
				processManager,
				this
			);
		}
		ISlickBowShootingProcessFactory thisProcessFactory;
		public ISlickBowShootingProcessFactory GetSlickBowShootingProcessFactory(){
			return thisProcessFactory;
		}
		public override IUnityBaseProcessFactory GetProcessFactory(){
			return thisProcessFactory;
		}
		public IUISystemProcessFactory GetUISystemProcessFactory(){
			return thisProcessFactory;
		}
		public UIManagerAdaptor uiManagerAdaptor;
		public IUIManager GetUIManager(){
			return uiManagerAdaptor.GetUIManager();
		}
	}
}
