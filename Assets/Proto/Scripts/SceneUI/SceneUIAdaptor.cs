using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ISceneUIAdaptor: IMonoBehaviourAdaptor{
		void SetUISize(Vector2 size);
		void SetUIPosition(Vector2 position);
		ISceneUI GetSceneUI();
		Vector3 GetTargetWorldPosition();
	}
	[RequireComponent(typeof(RectTransform))]
	public abstract class AbsSceneUIAdaptor : MonoBehaviourAdaptor, ISceneUIAdaptor {
		protected override void Awake(){
			base.Awake();
			thisRectTransform = this.transform.GetComponent<RectTransform>();
		}
		RectTransform thisRectTransform;
		public Camera uiCamera;
		public Vector2 minUISize;
		public Vector2 maxUISize;
		public float nearUIDistance;
		public float farUIDistance;
		protected ISceneUI thisSceneUI;
		public ISceneUI GetSceneUI(){
			return thisSceneUI;
		}
		public override void SetUp(){
			thisSceneUI = CreateSceneUI();
		}
		protected abstract ISceneUI CreateSceneUI();
		public void SetUISize(Vector2 size){
			thisRectTransform.sizeDelta  = size;
		}
		public void SetUIPosition(Vector2 position){
			thisRectTransform.position = position;
		}
		public Transform targetTransform;
		public Vector3 GetTargetWorldPosition(){
			return targetTransform.position;
		}
	}
}

