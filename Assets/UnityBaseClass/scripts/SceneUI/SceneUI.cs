using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityBase;

namespace UnityBase{
	public interface ISceneUI: ISceneObject, IActivationStateHandler, IActivationStateImplementor{

		void SetUIWorldPosition(Vector3 uiWorPos);
		Vector2 GetUIPosition(
			Vector3 targetWorldPosition
		);
		void UpdateUI();

		Vector2 GetRectSize();
		void SetTargetSceneObject(ISceneObject obj);
		int GetIndex();
		void SetIndex(int index);
		float GetNormalizedSqrDist();
	}
	public abstract class AbsSceneUI : AbsSceneObject, ISceneUI {
		public AbsSceneUI(
			IConstArg arg
		): base(
			arg
		){
			thisUICamera  = arg.uiCamera;
			thisMinUISize = arg.minUISize;
			thisMaxUISize = arg.maxUISize;
			thisNearUIDistance = arg.nearUIDistance;
			thisFarUIDistance = arg.farUIDistance;

			thisTypedAdaptor.SetUISize(thisMaxUISize);// and freeze size delta, use scale from now on
			thisMinScale = new Vector2(
				thisMinUISize.x/ thisMaxUISize.x,
				thisMinUISize.y/ thisMaxUISize.y
			);

			thisIndex = arg.index;

			thisActivationStateEngine = new ActivationStateEngine(this);
		}
		ISceneUIAdaptor thisTypedAdaptor{
			get{
				return (ISceneUIAdaptor)thisAdaptor;
			}
		}
		Vector3 thisUIWorldPosition;
		public void SetUIWorldPosition(Vector3 uiWorPos){
			thisUIWorldPosition = uiWorPos;
		}
		readonly Camera thisUICamera;
		Vector2 thisMinUISize;
		Vector2 thisMaxUISize;
		Vector2 thisMinScale;
		float thisFarUIDistance;
		float thisNearUIDistance;
		public void UpdateUI(){
			thisUIWorldPosition =  thisTypedAdaptor.GetTargetWorldPosition();

			Vector2 newUIScale = GetUIScale(thisUIWorldPosition);
			thisTypedAdaptor.SetUIScale(newUIScale);

			Vector3 uiScreenPos = GetUIPosition(thisUIWorldPosition);
			thisTypedAdaptor.SetUIPosition(uiScreenPos);
		}
		float thisNormalizedSqpDist;
		public float GetNormalizedSqrDist(){
			return thisNormalizedSqpDist;
		}
		protected Vector2 GetUISize(Vector3 targetWorldPosition){
			float sqrDistance = (targetWorldPosition - thisUICamera.transform.position).sqrMagnitude;
			float nearSqrDist = thisNearUIDistance * thisNearUIDistance;
			float farSqrDist = thisFarUIDistance * thisFarUIDistance;
			float normalizedSqrDist = (sqrDistance - nearSqrDist) / farSqrDist;
			thisNormalizedSqpDist = normalizedSqrDist;
			Vector2 newUISize = Vector2.Lerp(
				thisMaxUISize,
				thisMinUISize,
				normalizedSqrDist
			);
			return newUISize;
		}
		protected Vector2 GetUIScale(Vector3 targetWorldPosition){
			float sqrDistance = (targetWorldPosition - thisUICamera.transform.position).sqrMagnitude;
			float nearSqrDist = thisNearUIDistance * thisNearUIDistance;
			float farSqrDist = thisFarUIDistance * thisFarUIDistance;
			float normalizedSqrDist = (sqrDistance - nearSqrDist) / farSqrDist;

			Vector2 nweUIScale = Vector2.Lerp(
				Vector2.one,
				thisMinScale,
				normalizedSqrDist
			);
			return nweUIScale;
		}
		public Vector2 GetUIPosition(Vector3 targetWorldPosition){
			Vector2 uiScreenPos = thisUICamera.WorldToScreenPoint(thisUIWorldPosition);
			return uiScreenPos;
		}
		public void SetTargetSceneObject(ISceneObject obj){
			IMonoBehaviourAdaptor adaptor = obj.GetAdaptor();
			thisTypedAdaptor.SetTargetTransform(adaptor.GetTransform());
		}
		/* Activation */
			IActivationStateEngine thisActivationStateEngine;
			public void Activate(){
				thisActivationStateEngine.Activate();
			}
			
			public virtual void ActivateImple(){
				thisTypedAdaptor.BecomeChildToCanvas();
			}
			public void Deactivate(){
				thisActivationStateEngine.Deactivate();
			}
			public virtual void DeactivateImple(){
				Reserve();
			}
			protected abstract void Reserve();
			public bool IsActivated(){
				return thisActivationStateEngine.IsActivated();
			}
		/*  */
			
		public Vector2 GetRectSize(){
			return thisTypedAdaptor.GetRectSize();
		}
		public void SetTargetTransform(Transform targetTrans){
			thisTypedAdaptor.SetTargetTransform(targetTrans);
		}
		int thisIndex;
		public void SetIndex(int index){
			thisIndex = index;
		}
		public int GetIndex(){
			return thisIndex;
		}
	
		/* ConstArg */
			public new interface IConstArg: AbsSceneObject.IConstArg{
				Camera uiCamera{get;}
				Vector2 minUISize{get;}
				Vector2 maxUISize{get;}
				float nearUIDistance{get;}
				float farUIDistance{get;}
				int index{get;}
			}
			public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
				public ConstArg(
					ISceneUIAdaptor adaptor,
					Camera uiCamera,
					Vector2 minUISize,
					Vector2 maxUISize,
					float nearUIDistance,
					float farUIDistance,
					int index
				): base(
					adaptor
				){
					thisUICamera = uiCamera;
					thisMinUISize = minUISize;
					thisMaxUISize = maxUISize;
					thisNearUIDistance = nearUIDistance;
					thisFarUIDistance = farUIDistance;
					thisIndex = index;
				}
				readonly Camera thisUICamera;
				public Camera uiCamera{get{return thisUICamera;}}
				readonly Vector2 thisMinUISize;
				public Vector2 minUISize{get{return thisMinUISize;}}
				readonly Vector2 thisMaxUISize;
				public Vector2 maxUISize{get{return thisMaxUISize;}}
				readonly float thisNearUIDistance;
				public float nearUIDistance{get{return thisNearUIDistance;}}
				readonly float thisFarUIDistance;
				public float farUIDistance{get{return thisFarUIDistance;}}
				readonly int thisIndex;
				public int index{get{return thisIndex;}}
			}
		/*  */
	}
}
