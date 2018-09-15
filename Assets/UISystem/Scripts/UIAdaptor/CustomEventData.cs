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
			float deltaTime
		){
			/* do some conversion here */
			thisDeltaPosition = sourceData.delta;
			thisPosition = sourceData.position;
			Vector2 sourceVel = thisDeltaPosition/ deltaTime;
			if(sourceVel.sqrMagnitude > thisMaxVelocity * thisMaxVelocity)
				thisVelocity = Vector2.ClampMagnitude(sourceVel, thisMaxVelocity);
			else
				thisVelocity = sourceVel;
		}
		public CustomEventData(
			Vector2 position, 
			Vector2 deltaP, 
			float deltaTime
		){
			thisPosition = position;
			thisDeltaPosition = deltaP;
			Vector2 sourceVel = thisDeltaPosition/ deltaTime;
			if(sourceVel.sqrMagnitude > thisMaxVelocity * thisMaxVelocity)
				thisVelocity = Vector2.ClampMagnitude(sourceVel, thisMaxVelocity);
			else
				thisVelocity = sourceVel;
		}
		public CustomEventData(
			Vector2 position,
			Vector2 deltaPosition,
			Vector2 velocity
		){
			thisPosition = position;
			thisDeltaPosition = deltaPosition;
			thisVelocity = velocity;
		}
		float MakeVelocityWithinLimit(float source){
			if(source > thisMaxVelocity)
				return thisMaxVelocity;
			return source;
		}
		const float thisMaxVelocity = 1000f;
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
