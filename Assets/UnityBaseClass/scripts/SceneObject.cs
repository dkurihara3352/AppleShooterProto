using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBase{
	public interface ISceneObject{
		Vector3 GetPosition();
		void SetPosition(Vector3 position);
		Vector3 GetLocalPosition();
		void SetLocalPosition(Vector3 localPos);
		Quaternion GetRotation();
		void SetRotation(Quaternion rotation);
		Quaternion GetLocalRotation();
		void SetLocalRotation(Quaternion localRotation);
		void ResetLocalTransform();

		void ResetScale();
		Vector3 GetLocalScale();
		void SetParent(ISceneObject sceneObj);
		IMonoBehaviourAdaptor GetAdaptor();
		string GetName();
	}
	public abstract class AbsSceneObject: ISceneObject{
		public AbsSceneObject(
			IConstArg arg
		){
			thisAdaptor = arg.adaptor;
			thisMonoBehaviourAdaptorManager = thisAdaptor.GetMonoBehaviourAdaptorManager();
		}
		protected readonly IMonoBehaviourAdaptor thisAdaptor;
		public IMonoBehaviourAdaptor GetAdaptor(){
			return thisAdaptor;
		}
		protected IMonoBehaviourAdaptorManager thisMonoBehaviourAdaptorManager;
		public Vector3 GetPosition(){
			return thisAdaptor.GetPosition();
		}
		public void SetPosition(Vector3 position){
			thisAdaptor.SetPosition(position);
		}
		public Vector3 GetLocalPosition(){
			return thisAdaptor.GetLocalPosition();
		}
		public void SetLocalPosition(Vector3 localPos){
			thisAdaptor.SetLocalPosition(localPos);
		}

		public Quaternion GetRotation(){
			return thisAdaptor.GetRotation();
		}
		public void SetRotation(Quaternion rotation){
			thisAdaptor.SetRotation(rotation);
		}
		public Quaternion GetLocalRotation(){
			return thisAdaptor.GetLocalRotation();
		}
		public void SetLocalRotation(Quaternion localRot){
			thisAdaptor.SetLocalRotation(localRot);
		}
		public void ResetScale(){
			thisAdaptor.ResetScale();
		}
		public Vector3 GetLocalScale(){
			return thisAdaptor.GetLocalScale();
		}
		public void SetParent(ISceneObject sceneObj){
			Transform parentTrans;
			if(sceneObj != null){
				IMonoBehaviourAdaptor parentAdaptor = sceneObj.GetAdaptor();
				parentTrans = parentAdaptor.GetTransform();
			}else{
				parentTrans = null;
			}
				thisAdaptor.SetParent(parentTrans);
		}
		public void ResetLocalTransform(){
			thisAdaptor.ResetLocalTransform();
		}
		protected IUnityBaseProcessFactory thisProcessFactory{
			get{
				return thisAdaptor.GetProcessFactory();
			}
		}
		protected DKUtility.IProcessManager thisProcessManager{
			get{
				return thisMonoBehaviourAdaptorManager.GetProcessManager();
			}
		}
		public virtual string GetName(){
			return thisAdaptor.GetName();
		}
		/* ConstArg */
			public interface IConstArg{
				IMonoBehaviourAdaptor adaptor{get;}
			}
			public class ConstArg: IConstArg{
				public ConstArg(
					IMonoBehaviourAdaptor adaptor
				){
					thisAdaptor = adaptor;
				}
				readonly IMonoBehaviourAdaptor thisAdaptor;
				public IMonoBehaviourAdaptor adaptor{get{return thisAdaptor;}}
			}
		/*  */
	}
}

