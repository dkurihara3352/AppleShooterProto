using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ISceneUI{
		void SetUIWorldPosition(Vector3 uiWorPos);
		void UpdateUI();
		void Activate();
		void Deactivate();
	}
	public abstract class AbsSceneUI : ISceneUI {
		public AbsSceneUI(
			IConstArg arg
		){
			thisUICamera  = arg.uiCamera;
			thisAdaptor = arg.adaptor;
			thisMinUISize = arg.minUISize;
			thisMaxUISize = arg.maxUISize;
			thisNearUIDistance = arg.nearUIDistance;
			thisFarUIDistance = arg.farUIDistance;

		}
		Vector3 thisUIWorldPosition;
		public void SetUIWorldPosition(Vector3 uiWorPos){
			thisUIWorldPosition = uiWorPos;
		}
		readonly Camera thisUICamera;
		Vector2 thisMinUISize;
		Vector2 thisMaxUISize;
		float thisFarUIDistance;
		float thisNearUIDistance;
		public void UpdateUI(){
			thisUIWorldPosition =  thisAdaptor.GetTargetWorldPosition();
			Vector2 newUISize = GetUISize(thisUIWorldPosition);
			thisAdaptor.SetUISize(newUISize);

			Vector3 uiScreenPos = GetUIPosition(thisUIWorldPosition);
			thisAdaptor.SetUIPosition(uiScreenPos);
		}
		protected Vector2 GetUISize(Vector3 targetWorldPosition){
			float sqrDistance = (targetWorldPosition - thisUICamera.transform.position).sqrMagnitude;
			float nearSqrDist = thisNearUIDistance * thisNearUIDistance;
			float farSqrDist = thisFarUIDistance * thisFarUIDistance;
			float normalizedSqrDist = (sqrDistance - nearSqrDist) / farSqrDist;
			
			Vector2 newUISize = Vector2.Lerp(
				thisMaxUISize,
				thisMinUISize,
				normalizedSqrDist
			);
			return newUISize;
		}
		protected Vector2 GetUIPosition(Vector3 targetWorldPosition){
			Vector2 uiScreenPos = thisUICamera.WorldToScreenPoint(thisUIWorldPosition);
			return uiScreenPos;
		}
		readonly protected ISceneUIAdaptor thisAdaptor;
		bool thisIsActivated = false;
		public virtual void Activate(){
			if(thisIsActivated)
				return;
			thisIsActivated = true;
		}
		bool thisActivationIsInitialized = false;
		public virtual void Deactivate(){
			if(
				thisActivationIsInitialized && 
				!thisIsActivated
			)
				return;
			thisIsActivated = false;
			if(!thisActivationIsInitialized)
				thisActivationIsInitialized = true;
		}
		/* ConstArg */
			public interface IConstArg{
				Camera uiCamera{get;}
				ISceneUIAdaptor adaptor{get;}
				Vector2 minUISize{get;}
				Vector2 maxUISize{get;}
				float nearUIDistance{get;}
				float farUIDistance{get;}
			}
			public struct ConstArg: IConstArg{
				public ConstArg(
					Camera uiCamera,
					ISceneUIAdaptor adaptor,
					Vector2 minUISize,
					Vector2 maxUISize,
					float nearUIDistance,
					float farUIDistance
				){
					thisUICamera = uiCamera;
					thisAdaptor = adaptor;
					thisMinUISize = minUISize;
					thisMaxUISize = maxUISize;
					thisNearUIDistance = nearUIDistance;
					thisFarUIDistance = farUIDistance;
				}
				readonly Camera thisUICamera;
				public Camera uiCamera{get{return thisUICamera;}}
				readonly ISceneUIAdaptor thisAdaptor;
				public ISceneUIAdaptor adaptor{get{return thisAdaptor;}}
				readonly Vector2 thisMinUISize;
				public Vector2 minUISize{get{return thisMinUISize;}}
				readonly Vector2 thisMaxUISize;
				public Vector2 maxUISize{get{return thisMaxUISize;}}
				readonly float thisNearUIDistance;
				public float nearUIDistance{get{return thisNearUIDistance;}}
				readonly float thisFarUIDistance;
				public float farUIDistance{get{return thisFarUIDistance;}}

			}
		/*  */
	}
}
