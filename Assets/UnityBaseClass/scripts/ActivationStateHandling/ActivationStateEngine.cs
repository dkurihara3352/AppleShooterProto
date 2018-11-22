using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBase{
	public interface IActivationStateHandler{
		void Activate();
		void Deactivate();
		bool IsActivated();
	}
	public interface IActivationStateImplementor{
		void ActivateImple();
		void DeactivateImple();
	}
	public interface IActivationStateEngine: IActivationStateHandler{}
	public class ActivationStateEngine: IActivationStateEngine{
		public ActivationStateEngine(
			IActivationStateImplementor implementor
		){
			thisImplementor = implementor;
		}
		IActivationStateImplementor thisImplementor;
		bool thisIsActivated = false;
		bool thisIsInitialized = false;
		public void Activate(){
			if(thisIsActivated)
				return;
			thisIsActivated = true;
			thisImplementor.ActivateImple();
		}
		public void Deactivate(){
			if(thisIsInitialized && !thisIsActivated)
				return;
			if(!thisIsInitialized)
				thisIsInitialized = true;
			thisIsActivated = false;
			thisImplementor.DeactivateImple();
		}
		public bool IsActivated(){
			return thisIsActivated;
		}
	}	
}

