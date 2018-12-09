using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UISystem{
	public interface ICustomEventData{
		Vector2 deltaPos{get;}
		Vector2 position{get;}
		Vector2 velocity{get;}
		void SetVelocity(Vector2 velocity);
	}
	public struct CustomEventData: ICustomEventData{
		public CustomEventData(
			PointerEventData sourceData, 
			float deltaTime,
			IUIManager uiManager
		){
			/* do some conversion here */
			thisDeltaPosition = sourceData.delta;
			thisPosition = sourceData.position;
			Vector2 sourceVel = thisDeltaPosition/ deltaTime;
			thisVelocity = CalculateVelocity(uiManager, sourceVel);
		}
		public CustomEventData(
			Vector2 position, 
			Vector2 deltaP, 
			float deltaTime,
			IUIManager uiManager
		){
			thisPosition = position;
			thisDeltaPosition = deltaP;
			Vector2 sourceVel = thisDeltaPosition/ deltaTime;
			thisVelocity = CalculateVelocity(uiManager, sourceVel);
			// float maxVelocity = uiManager.GetMaxSwipeVelocity();
			// if(sourceVel.sqrMagnitude > maxVelocity * maxVelocity)
			// 	thisVelocity = Vector2.ClampMagnitude(sourceVel, maxVelocity);
			// else
			// 	thisVelocity = sourceVel;
		}
		public CustomEventData(
			Vector2 position,
			Vector2 deltaPosition,
			Vector2 velocity,
			IUIManager uiManager
		){

			thisPosition = position;
			thisDeltaPosition = deltaPosition;
			thisVelocity = CalculateVelocity(uiManager, velocity);
		}
		static Vector2 CalculateVelocity(IUIManager uiManager, Vector2 sourceVel){
			float maxVelocity = uiManager.GetMaxSwipeVelocity();
			if(sourceVel.sqrMagnitude > maxVelocity * maxVelocity)
				return Vector2.ClampMagnitude(sourceVel, maxVelocity);
			else
				return sourceVel;
		}
		// float MakeVelocityWithinLimit(float source){
		// 	if(source > thisMaxVelocity)
		// 		return thisMaxVelocity;
		// 	return source;
		// }
		// float thisMaxVelocity{
		// 	get{
		// 		return thisUIManager.GetMaxSwipeVelocity();
		// 	}
		// }
		// readonly IUIManager thisUIManager;
		public Vector2 deltaPos{get{return thisDeltaPosition;}}
		Vector2 thisDeltaPosition;

		public Vector2 position{get{return thisPosition;}}
		Vector2 thisPosition;

		public Vector2 velocity{get{return thisVelocity;}}
		Vector2 thisVelocity;
		
		public void SetVelocity(Vector2 velocity){
			thisVelocity = velocity;
		}
	}
}
