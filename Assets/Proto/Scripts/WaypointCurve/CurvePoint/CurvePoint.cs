using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ICurvePoint{
		Vector3 GetPosition();
		Vector3 GetUpDirection();
		float GetDelta();
		float GetDistanceUpToPointOnSegment();
		float GetDistanceUpToPointOnCurve();
		void SetDistanceUpToPointOnCurve(float dist);
		void SetPrevPointPosition(Vector3 position);
		Vector3 GetPrevPointPosition();
	}
	public struct CurvePoint: ICurvePoint{
		public CurvePoint(
			Vector3 position,
			Vector3 up,
			float delta,
			float distanceUpToPointOnSegment
		){
			thisPosition = position;
			thisUpDirection = up;
			thisDelta = delta;
			thisDistanceUpToPointOnSegment = distanceUpToPointOnSegment;
			thisDistanceUpToPointOnCurve = thisDistanceUpToPointOnSegment;
			thisPrevPointPosition = thisPosition;
		}
		readonly Vector3 thisPosition;
		public Vector3 GetPosition(){
			return thisPosition;
		}
		readonly Vector3 thisUpDirection;
		public Vector3 GetUpDirection(){
			return thisUpDirection;
		}
		readonly float thisDelta;
		public float GetDelta(){
			return thisDelta;
		}
		readonly float thisDistanceUpToPointOnSegment;
		public float GetDistanceUpToPointOnSegment(){
			return thisDistanceUpToPointOnSegment;
		}
		float thisDistanceUpToPointOnCurve;
		public float GetDistanceUpToPointOnCurve(){
			return thisDistanceUpToPointOnCurve;
		}
		public void SetDistanceUpToPointOnCurve(float dist){
			thisDistanceUpToPointOnCurve = dist;
		}
		Vector3 thisPrevPointPosition;
		public Vector3 GetPrevPointPosition(){
			return thisPrevPointPosition;	
		}
		public void SetPrevPointPosition(Vector3 prevPos){
			thisPrevPointPosition = prevPos;
		}
	}
}
