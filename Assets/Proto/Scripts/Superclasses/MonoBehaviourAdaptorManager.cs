using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
namespace AppleShooterProto{
	public interface IMonoBehaviourAdaptorManager{
		IProcessManager GetProcessManager();
		IAppleShooterProcessFactory GetProcessFactory();
		void SetUpAllMonoBehaviourAdaptors();
		void SetUpAdaptorReference();
		void FinalizeSetUp();
		void AddAdaptor(IMonoBehaviourAdaptor adaptor);
	}
	public class MonoBehaviourAdaptorManager: MonoBehaviour, IMonoBehaviourAdaptorManager{
		void Awake(){
			thisProcessFactory = new AppleShooterProcessFactory(
				processManager
			);
		}
		public ProcessManager processManager;
		public IProcessManager GetProcessManager(){
			return processManager;
		}
		IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory GetProcessFactory(){
			return thisProcessFactory;
		}

		List<IMonoBehaviourAdaptor> thisMonoBehaviourAdaptors = new List<IMonoBehaviourAdaptor>();
		public void AddAdaptor(IMonoBehaviourAdaptor adaptor){
			thisMonoBehaviourAdaptors.Add(adaptor);
		}
		public void SetUpAllMonoBehaviourAdaptors(){
			IMonoBehaviourAdaptor[] temp = thisMonoBehaviourAdaptors.ToArray();
			foreach(IMonoBehaviourAdaptor adaptor in temp)
				adaptor.SetUp();
		}
		public void SetUpAdaptorReference(){
			IMonoBehaviourAdaptor[] temp = thisMonoBehaviourAdaptors.ToArray();
			foreach(IMonoBehaviourAdaptor adaptor in temp)
				adaptor.SetUpReference();
		}
		public void FinalizeSetUp(){
			IMonoBehaviourAdaptor[] temp = thisMonoBehaviourAdaptors.ToArray();
			foreach(IMonoBehaviourAdaptor adaptor in temp)
				adaptor.FinalizeSetUp();
		}
	}
}

