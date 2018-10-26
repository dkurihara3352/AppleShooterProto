using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IInstatiableMonoBehaviourAdaptor: IMonoBehaviourAdaptor{
		void SetMonoBehaviourAdaptorManager(IMonoBehaviourAdaptorManager manager);
	}
	public class InstatiableMonoBehaviourAdaptor: MonoBehaviourAdaptor, IInstatiableMonoBehaviourAdaptor{
		protected override sealed void Awake(){
			return;
		}
		public void SetMonoBehaviourAdaptorManager(IMonoBehaviourAdaptorManager manager){
			thisMonoBehaviourAdaptorManager = manager;
		}
	}
}

