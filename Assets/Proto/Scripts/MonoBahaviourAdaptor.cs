using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IMonoBehaviourAdaptor{
		Vector3 GetPosition();
		void SetPosition(Vector3 position);
		
	}
	public class MonoBehaviourAdaptor: MonoBehaviour, IMonoBehaviourAdaptor{
		public Vector3 GetPosition(){
			return this.transform.position;
		}
		public void SetPosition(Vector3 position){
			this.transform.position = position;
		}
	}
}
