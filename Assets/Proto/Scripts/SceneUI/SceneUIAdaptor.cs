﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ISceneUIAdaptor: IMonoBehaviourAdaptor{
		void SetUISize(Vector2 size);
		void SetUIScale(Vector2 scale);
		void SetUIPosition(Vector2 position);
		ISceneUI GetSceneUI();
		void SetTargetTransform(Transform targetTrans);
		Vector3 GetTargetWorldPosition();
		void ResetAtReserve();
		void BecomeChildToCanvas();
		Vector2 GetRectSize();
	}
	[RequireComponent(typeof(RectTransform))]
	public abstract class AbsSceneUIAdaptor : MonoBehaviourAdaptor, ISceneUIAdaptor {
		protected RectTransform thisRectTransform;
		protected RectTransform CollectRectTransform(){
			return this.transform.GetComponent<RectTransform>();
		}
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
			thisRectTransform = CollectRectTransform();
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
		public void SetTargetTransform(Transform targetTrans){
			targetTransform = targetTrans;
		}
		public Vector3 GetTargetWorldPosition(){
			return targetTransform.position;
		}
		public RectTransform reserveRectTransform;
		public virtual void ResetAtReserve(){
			SetParent(reserveRectTransform);
			ResetLocalTransform();
			OnResetAtReserve();
		}
		protected abstract void OnResetAtReserve();
		protected Canvas thisCanvas;
		Canvas CollectCanvas(){
			return this.transform.GetComponentInParent<Canvas>();
		}
		public void BecomeChildToCanvas(){
			SetParent(thisCanvas.transform);
		}
		public Vector2 GetRectSize(){
			return thisRectTransform.sizeDelta;
		}
	}
}

