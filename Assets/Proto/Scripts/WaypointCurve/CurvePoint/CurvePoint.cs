using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ICurvePoint{
		Vector3 GetPosition();
		Quaternion GetRotation();
		float GetDelta();
		float GetDistanceUpToPointOnSegment();
		float GetDistanceUpToPointOnCurve();
		void SetDistanceUpToPointOnCurve(float dist);
	}
	public struct CurvePoint: ICurvePoint{
		public CurvePoint(
			Vector3 position,
			Quaternion rotation,
			float delta,
			float distanceUpToPointOnSegment
		){
			thisPosition = position;
			thisRotation = rotation;
			thisDelta = delta;
			thisDistanceUpToPointOnSegment = distanceUpToPointOnSegment;
			thisDistanceUpToPointOnCurve = thisDistanceUpToPointOnSegment;
		}
		readonly Vector3 thisPosition;
		public Vector3 GetPosition(){
			return thisPosition;
		}
		readonly Quaternion thisRotation;
		public Quaternion GetRotation(){
			return thisRotation;
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
	}
}
