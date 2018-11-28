using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
using UISystem;

namespace AppleShooterProto{
	public interface IAppleShooterMonoBehaviourAdaptorManager: IUISystemMonoBehaviourAdaptorManager{
		IAppleShooterProcessFactory GetAppleShooterProcessFactory();
	}
	public class AppleShooterMonoBehaviourAdaptorManager : MonoBehaviourAdaptorManager, IAppleShooterMonoBehaviourAdaptorManager {
		void Awake(){
			thisProcessFactory = CreateProcessFactory();
		}
		IAppleShooterProcessFactory CreateProcessFactory(){
			return new AppleShooterProcessFactory(
				processManager,
				this
			);
		}
		IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory GetAppleShooterProcessFactory(){
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
