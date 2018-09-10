using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IMonoBehaviourAdaptor{
		void SetUp();
		void SetUpReference();
		Vector3 GetPosition();
		void SetPosition(Vector3 position);
		void SetRotation(Quaternion rotation);
		void Rotate(Vector3 euler);
		void Rotate(float angleOnAxis, int axis);
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
		public void SetRotation(Quaternion rotation){
			this.transform.rotation = rotation;
		}
		public void Rotate(Vector3 euler){
			this.transform.localEulerAngles = euler;
		}
		public void Rotate(
			float angleOnAxis,
			int axis
		){
			Vector3 original = this.transform.localEulerAngles;
			if(axis == 0)
				this.transform.localEulerAngles = new Vector3(
					angleOnAxis,
					original.y,
					original.z					
				);
			if(axis == 1)
				this.transform.localEulerAngles = new Vector3(
					original.x,
					angleOnAxis,
					original.z
				);
		}
		public virtual void SetUp(){}
		public virtual void SetUpReference(){}
	}
}
