using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBase{
	public interface ISceneUIAdaptor: IMonoBehaviourAdaptor{
		void SetCamera(Camera camera);
		void SetCanvas(Canvas canvas);
		void SetUISize(Vector2 size);
		void SetUIScale(Vector2 scale);
		void SetUIPosition(Vector2 position);
		ISceneUI GetSceneUI();
		void SetTargetTransform(Transform targetTrans);
		Vector3 GetTargetWorldPosition();
		void BecomeChildToCanvas();
		Vector2 GetRectSize();
		Vector2 GetUINormalizedScreenPosition();
	}
	[RequireComponent(typeof(RectTransform))]
	public abstract class AbsSceneUIAdaptor : MonoBehaviourAdaptor, ISceneUIAdaptor {
		protected RectTransform thisRectTransform;
		protected RectTransform CollectRectTransform(){
			return this.transform.GetComponent<RectTransform>();
		}
		protected Camera thisCamera;
		public void SetCamera(Camera camera){
			thisCamera = camera;
		}
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
			// thisCanvas  = CollectCanvas();
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
		Vector2 GetUIScreenPosition(){
			Vector3 targetWorldPosition = GetTargetWorldPosition();
			return thisCamera.WorldToScreenPoint(targetWorldPosition);
		}
		public Vector2 GetUINormalizedScreenPosition(){
			Vector2 uiScreenPos = GetUIScreenPosition();
			Vector2 result = new Vector2(uiScreenPos.x/ Screen.width,  uiScreenPos.y/ Screen.height);
			return result;
		}
		protected Canvas thisCanvas;
		public void SetCanvas(Canvas canvas){
			thisCanvas = canvas;
		}
		public void BecomeChildToCanvas(){
			SetParent(thisCanvas.transform);
		}
		public Vector2 GetRectSize(){
			return thisRectTransform.sizeDelta;
		}
	}
}

