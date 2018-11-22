using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace AppleShooterProto{
	public interface IAppleShooterMonoBehaviourAdaptor: IMonoBehaviourAdaptor{
		IAppleShooterMonoBehaviourAdaptorManager GetAppleShooterMonoBehaviourAdaptorManager();
	}
	public abstract class AppleShooterMonoBehaviourAdaptor : MonoBehaviourAdaptor, IAppleShooterMonoBehaviourAdaptor {

		protected IAppleShooterProcessFactory thisAppleShooterProcessFactory{
			get{
				return thisAppleShooterMonoBehaviourAdaptorManager.GetAppleShooterProcessFactory();
			}
		}
		protected IAppleShooterMonoBehaviourAdaptorManager thisAppleShooterMonoBehaviourAdaptorManager{
			get{
				return (IAppleShooterMonoBehaviourAdaptorManager)thisMonoBehaviourAdaptorManager;
			}
		}
		public IAppleShooterMonoBehaviourAdaptorManager GetAppleShooterMonoBehaviourAdaptorManager(){
			return thisAppleShooterMonoBehaviourAdaptorManager;
		}
	}
}

