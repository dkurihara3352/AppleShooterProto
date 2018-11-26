using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace UnityBase{
	public interface IMonoBehaviourAdaptorManager{
		IProcessManager GetProcessManager();
		void SetUpAllMonoBehaviourAdaptors();
		void SetUpAdaptorReference();
		void FinalizeSetUp();
		void AddAdaptor(IMonoBehaviourAdaptor adaptor);
		IUnityBaseProcessFactory GetProcessFactory();
	}
	public abstract class MonoBehaviourAdaptorManager: MonoBehaviour, IMonoBehaviourAdaptorManager{

		public ProcessManager processManager;
		public IProcessManager GetProcessManager(){
			return processManager;
		}

		List<IMonoBehaviourAdaptor> thisMonoBehaviourAdaptors = new List<IMonoBehaviourAdaptor>();
		public void AddAdaptor(IMonoBehaviourAdaptor adaptor){
			thisMonoBehaviourAdaptors.Add(adaptor);
		}
		public void SetUpAllMonoBehaviourAdaptors(){
			IMonoBehaviourAdaptor[] temp = thisMonoBehaviourAdaptors.ToArray();
			foreach(IMonoBehaviourAdaptor adaptor in temp)
				if(adaptor.IsEnabled()){
					adaptor.SetUp();
				}
		}
		public void SetUpAdaptorReference(){
			IMonoBehaviourAdaptor[] temp = thisMonoBehaviourAdaptors.ToArray();
			foreach(IMonoBehaviourAdaptor adaptor in temp)
				if(adaptor.IsEnabled())
					adaptor.SetUpReference();
		}
		public void FinalizeSetUp(){
			IMonoBehaviourAdaptor[] temp = thisMonoBehaviourAdaptors.ToArray();
			foreach(IMonoBehaviourAdaptor adaptor in temp)
				if(adaptor.IsEnabled())
					adaptor.FinalizeSetUp();
		}

		public abstract IUnityBaseProcessFactory GetProcessFactory();
	}
}

