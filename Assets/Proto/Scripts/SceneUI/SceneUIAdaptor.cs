using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ISceneUIAdaptor: IMonoBehaviourAdaptor{
		void SetUISize(Vector2 size);
		void SetUIScale(Vector2 scale);
		void SetUIPosition(Vector2 position);
		ISceneUI GetSceneUI();
		Vector3 GetTargetWorldPosition();
		void ResetAtReserve();
		void BecomeChildToCanvas();
	}
	[RequireComponent(typeof(RectTransform))]
	public abstract class AbsSceneUIAdaptor : MonoBehaviourAdaptor, ISceneUIAdaptor {
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
			thisRectTransform = this.transform.GetComponent<RectTransform>();
			thisCanvas  = CollectCanvas();
			thisSceneUI = CreateSceneUI();
		}
		protected abstract ISceneUI CreateSceneUI();
		public override void FinalizeSetUp(){
			thisSceneUI.Deactivate();
		}
		public void SetUISize(Vector2 size){
			thisRectTransform.sizeDelta  = size;
		}
		public void SetUIScale(Vector2 scale){
			thisRectTransform.localScale = scale;
		}
		public void SetUIPosition(Vector2 position){
			thisRectTransform.position = position;
		}
		public Transform targetTransform;
		public Vector3 GetTargetWorldPosition(){
			return targetTransform.position;
		}
		public RectTransform reserveRectTransform;
		public void ResetAtReserve(){
			SetParent(reserveRectTransform);
			ResetLocalTransform();
			OnResetAtReserve();
		}
		protected abstract void OnResetAtReserve();
		Canvas thisCanvas;
		Canvas CollectCanvas(){
			return this.transform.GetComponentInParent<Canvas>();
		}
		public void BecomeChildToCanvas(){
			SetParent(thisCanvas.transform);
		}
	}
}

