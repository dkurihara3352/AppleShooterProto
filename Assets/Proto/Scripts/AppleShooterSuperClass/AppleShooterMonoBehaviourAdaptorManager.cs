using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace AppleShooterProto{
	public interface IAppleShooterMonoBehaviourAdaptorManager: IMonoBehaviourAdaptorManager{
		IAppleShooterProcessFactory GetAppleShooterProcessFactory();
	}
	public class AppleShooterMonoBehaviourAdaptorManager : MonoBehaviourAdaptorManager, IAppleShooterMonoBehaviourAdaptorManager {
		void Awake(){
			thisProcessFactory = CreateProcessFactory();
		}
		IAppleShooterProcessFactory CreateProcessFactory(){
			return new AppleShooterProcessFactory(processManager);
		}
		public IAppleShooterProcessFactory GetAppleShooterProcessFactory(){
			return thisProcessFactory;
		}
		public override IUnityBaseProcessFactory GetProcessFactory(){
			return thisProcessFactory;
		}
		IAppleShooterProcessFactory thisProcessFactory;
	}
}
