using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IMonoBehaviourAdaptor{
		void SetUp();
		void SetUpReference();
		Vector3 GetPosition();
		void SetPosition(Vector3 position);
		
	}
	public class MonoBehaviourAdaptor: MonoBehaviour, IMonoBehaviourAdaptor{
		void Awake(){
			thisMBAdaptorManager = FindAndSetMonoBehaviourAdaptor();
			thisMBAdaptorManager.AddAdaptor(this);

		}
		IMonoBehaviourAdaptorManager thisMBAdaptorManager;
		public IMonoBehaviourAdaptorManager FindAndSetMonoBehaviourAdaptor(){
			IMonoBehaviourAdaptorManager result = (IMonoBehaviourAdaptorManager)GameObject.Find("MonoBehaviourAdaptorManagerGO").GetComponent(typeof(IMonoBehaviourAdaptorManager));
			if(result == null)
				throw new System.InvalidOperationException(
					"MonoBehaviourManager is not found"
				);
			return result;
		}
		public Vector3 GetPosition(){
			return this.transform.position;
		}
		public void SetPosition(Vector3 position){
			this.transform.position = position;
		}
		public virtual void SetUp(){}
		public virtual void SetUpReference(){}
	}
}
