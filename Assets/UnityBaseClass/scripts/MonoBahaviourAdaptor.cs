using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace UnityBase{
	public interface IMonoBehaviourAdaptor{
		void SetUp();
		void SetUpReference();
		void FinalizeSetUp();

		IMonoBehaviourAdaptorManager GetMonoBehaviourAdaptorManager();

		Vector3 GetPosition();
		void SetPosition(Vector3 position);
		Quaternion GetRotation();
		void SetRotation(Quaternion rotation);
		Vector3 GetLocalPosition();
		void SetLocalPosition(Vector3 localPosition);
		Quaternion GetLocalRotation();
		void SetLocalRotation(Quaternion localRotation);
		void Rotate(Vector3 euler);
		void Rotate(float angleOnAxis, int axis);
		void SetLookRotation(Vector3 forward, Vector3 up);
		void SetLookRotation(Vector3 forward);

		Transform GetTransform();
		void SetParent(Transform parent);
		void ResetLocalTransform();
		Vector3 GetForwardDirection();
		bool IsEnabled();

		IUnityBaseProcessFactory GetProcessFactory();
	}
	public abstract class MonoBehaviourAdaptor: MonoBehaviour, IMonoBehaviourAdaptor{
		protected virtual void Awake(){
			thisMonoBehaviourAdaptorManager = FindAndSetMonoBehaviourAdaptor();
			thisMonoBehaviourAdaptorManager.AddAdaptor(this);
		}
		protected IMonoBehaviourAdaptorManager thisMonoBehaviourAdaptorManager;
		public IMonoBehaviourAdaptorManager FindAndSetMonoBehaviourAdaptor(){
			IMonoBehaviourAdaptorManager result = (IMonoBehaviourAdaptorManager)GameObject.Find("MonoBehaviourAdaptorManagerGO").GetComponent(typeof(IMonoBehaviourAdaptorManager));
			if(result == null)
				throw new System.InvalidOperationException(
					"MonoBehaviourManager is not found"
				);
			return result;
		}
		public IMonoBehaviourAdaptorManager GetMonoBehaviourAdaptorManager(){
			return thisMonoBehaviourAdaptorManager;
		}
		protected IProcessManager processManager{
			get{
				return thisMonoBehaviourAdaptorManager.GetProcessManager();
			}
		}
		protected IUnityBaseProcessFactory thisProcessFactory{
			get{
				return thisMonoBehaviourAdaptorManager.GetProcessFactory();
			}
		}
		public IUnityBaseProcessFactory GetProcessFactory(){

			return thisProcessFactory;
		}

		public virtual void SetUp(){}
		public virtual void SetUpReference(){}
		public virtual void FinalizeSetUp(){}
		
		public Vector3 GetPosition(){
			return this.transform.position;
		}
		public void SetPosition(Vector3 position){
			this.transform.position = position;
		}
		public Quaternion GetRotation(){
			return this.transform.rotation;
		}
		public void SetRotation(Quaternion rotation){
			this.transform.rotation = rotation;
		}
		public Vector3 GetLocalPosition(){
			return this.transform.localPosition;
		}
		public void SetLocalPosition(Vector3 localPosition){
			this.transform.localPosition  =localPosition;
		}
		public Quaternion GetLocalRotation(){
			return this.transform.localRotation;
		}
		public void SetLocalRotation(Quaternion localRotation){
			this.transform.localRotation = localRotation;
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
		public void SetLookRotation(
			Vector3 forward,
			Vector3 up
		){
			if(forward != Vector3.zero)
				this.transform.rotation = Quaternion.LookRotation(
					forward,
					up
				);
		}
		public void SetLookRotation(Vector3 forward){
			this.SetLookRotation(
				forward,
				this.transform.up
			);
		}
		public Transform GetTransform(){
			return this.transform;
		}
		public void SetParent(Transform parent){
			this.transform.SetParent(parent, true);
		}
		public void ResetLocalTransform(){
			this.transform.localPosition = Vector3.zero;
			this.transform.localRotation = Quaternion.identity;
		}
		public Vector3 GetForwardDirection(){
			return this.transform.forward;
		}
		public bool IsEnabled(){
			return this.enabled;
		}
	}
}
