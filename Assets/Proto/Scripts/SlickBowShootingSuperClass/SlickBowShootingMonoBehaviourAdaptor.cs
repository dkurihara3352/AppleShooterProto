using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace SlickBowShooting{
	public interface ISlickBowShootingMonoBehaviourAdaptor: IMonoBehaviourAdaptor{
		ISlickBowShootingMonoBehaviourAdaptorManager GetSlickBowShootingMonoBehaviourAdaptorManager();
	}
	public abstract class SlickBowShootingMonoBehaviourAdaptor : MonoBehaviourAdaptor, ISlickBowShootingMonoBehaviourAdaptor {

		protected ISlickBowShootingProcessFactory thisSlickBowShootingProcessFactory{
			get{
				return thisSlickBowShootingMonoBehaviourAdaptorManager.GetSlickBowShootingProcessFactory();
			}
		}
		protected ISlickBowShootingMonoBehaviourAdaptorManager thisSlickBowShootingMonoBehaviourAdaptorManager{
			get{
				return (ISlickBowShootingMonoBehaviourAdaptorManager)thisMonoBehaviourAdaptorManager;
			}
		}
		public ISlickBowShootingMonoBehaviourAdaptorManager GetSlickBowShootingMonoBehaviourAdaptorManager(){
			return thisSlickBowShootingMonoBehaviourAdaptorManager;
		}
	}
}

