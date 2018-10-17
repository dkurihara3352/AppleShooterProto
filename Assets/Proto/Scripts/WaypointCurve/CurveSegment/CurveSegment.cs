using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility.CurveUtility;

namespace AppleShooterProto{
	public interface ICurveSegment{
		void UpdateCurvePoints();
		ICurvePoint[] GetCurvePoints();
		void SetCurveResolution(int resolution);
		float GetLength();
	}
	public class CurveSegment: ICurveSegment{
		public CurveSegment(
			IWaypointCurveAdaptor waypointCurveAdaptor,
			ICurveControlPoint tailControlPoint,
			ICurveControlPoint headControlPoint
		){
			thisWaypointCurveAdaptor = waypointCurveAdaptor;
			thisControlPointPair = new ICurveControlPoint[]{
				tailControlPoint,
				headControlPoint
			};
		}
		readonly IWaypointCurveAdaptor thisWaypointCurveAdaptor;
		readonly ICurveControlPoint[] thisControlPointPair;
		public ICurveControlPoint[] GetControlPointPair(){
			return thisControlPointPair;
		}
		ICurveControlPoint tailControlPoint{
			get{return thisControlPointPair[0];}
		}
		ICurveControlPoint headControlPoint{
			get{return thisControlPointPair[1];}
		}
		Transform foreHandle{
			get{return tailControlPoint.GetForeHandle();}
		}
		Transform backHandle{
			get{return headControlPoint.GetBackHandle();}
		}
		/* CurvePoints */
		ICurvePoint[] thisCurvePoints;
		int thisCurveResolution;
		public void SetCurveResolution(int resolution){
			thisCurveResolution = resolution;
		}
		public void ObsCreateCurvePoints(int resolution){
			List<ICurvePoint> result = new List<ICurvePoint>(resolution);
			for(int i = 0; i < resolution; i ++){
				float normalizedT = i/ ((resolution -1) * 1f);
				ICurvePoint curvePoint = CreateCurvePoint(normalizedT);
				result.Add(curvePoint);
			}
			thisCurvePoints = result.ToArray();
			UpdateCurvePoints();
		}
		ICurvePoint[] CreateCurvePoints(){
			sum = 0f;
			int resolution = thisCurveResolution;
			List<ICurvePoint> result = new List<ICurvePoint>(resolution);
			for(int i = 0; i < resolution; i ++){
				float normalizedT;
				if(i == 0)
					normalizedT = 0f;
				else
					normalizedT = i/ ((resolution -1) * 1f);
				ICurvePoint curvePoint = CreateCurvePoint(normalizedT);
				result.Add(curvePoint);
			}
			return result.ToArray();
		}
		Vector3 thisPrevPosition;
		float sum = 0f;
		ICurvePoint CreateCurvePoint(float normalizedT){
			Vector3 thisPrev = thisPrevPosition;
			Vector3 position = CalculatePointPosition(normalizedT);
			thisPrevPosition = position;

			Quaternion rotation = CalculatePointRotation(normalizedT);

			float delta;
			if(normalizedT == 0f)
				delta = 0f;
			else
				delta = (position - thisPrev).magnitude;
			sum += delta;

			return new CurvePoint(
				position,
				rotation,
				delta,
				sum
			);
		}
		public ICurvePoint[] GetCurvePoints(){
			return thisCurvePoints;
		}
		float CalculateNormalizedT(int index){
			float normalizedT;
			if(index != 0)
				normalizedT = index/ ((thisCurvePoints.Length - 1) * 1f);
			else
				normalizedT = 0f;
			return normalizedT;
		}
		Vector3 CalculatePointPosition(float normalizedT){
			Vector3 position = Bezier.CubicBezierV3(
				tailControlPoint.GetPosition(),
				foreHandle.position,
				backHandle.position,
				headControlPoint.GetPosition(),
				normalizedT
			);
			return position;
		}
		Quaternion CalculatePointRotation(float normalizedT){
			Quaternion rotation = Quaternion.Slerp(
				tailControlPoint.GetRotation(),
				headControlPoint.GetRotation(),
				normalizedT
			);
			return rotation;
		}
		public void UpdateCurvePoints(){
			thisCurvePoints = CreateCurvePoints();
		}
		public float GetLength(){
			return thisCurvePoints[thisCurvePoints.Length -1].GetDistanceUpToPointOnSegment();
		}
	}
}
