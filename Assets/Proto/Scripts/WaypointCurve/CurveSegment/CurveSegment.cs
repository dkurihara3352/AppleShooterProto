using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility.CurveUtility;

namespace AppleShooterProto{
	public interface ICurveSegment{
		void CreateCurvePoints(int resolution);
		void UpdateCurvePointsTransform();
		List<ICurvePoint> GetCurvePoints();
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
		List<ICurvePoint> thisCurvePoints;
		public void CreateCurvePoints(int resolution){
			List<ICurvePoint> result = new List<ICurvePoint>(resolution);
			for(int i = 0; i < resolution; i ++){
				float normalizedT = i/ ((resolution -1) * 1f);
				ICurvePoint curvePoint = CreateCurvePoint(normalizedT);
				result.Add(curvePoint);
			}
			thisCurvePoints = result;
			UpdateCurvePointsTransform();
		}
		ICurvePoint CreateCurvePoint(float normalizedT){
			GameObject curvePointGO = new GameObject("curvePoint");
			Transform parent = thisWaypointCurveAdaptor.GetTransform();

			curvePointGO.transform.SetParent(parent);
			ICurvePointAdaptor curvePointAdaptor = (ICurvePointAdaptor)curvePointGO.AddComponent<CurvePointAdaptor>();
			curvePointAdaptor.SetNormalizedT(normalizedT);
			curvePointAdaptor.SetUp();
			ICurvePoint curvePoint = curvePointAdaptor.GetCurvePoint();
			return curvePoint;
		}
		public List<ICurvePoint> GetCurvePoints(){
			return thisCurvePoints;
		}
		float CalculateNormalizedT(int index){
			float normalizedT;
			if(index != 0)
				normalizedT = index/ ((thisCurvePoints.Count - 1) * 1f);
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
		Vector3 CalculateForwardDirection(int index){
			if(index == thisCurvePoints.Count -1)
				return thisCurvePoints[index -1].GetForwardDirection();
			else{
				ICurvePoint thisPoint = thisCurvePoints[index];
				ICurvePoint nextPoint = thisCurvePoints[index + 1];
				return nextPoint.GetPosition() - thisPoint.GetPosition();
			}
		}
		Vector3 CalculateUpwardDirection(float normalizedT){
			return Vector3.Slerp(
				tailControlPoint.GetUpDirection(),
				headControlPoint.GetUpDirection(),
				normalizedT
			);
		}
		public void UpdateCurvePointsTransform(){
			UpdateCurvePointsPosition();
			UpdateCurvePointsRotation();
		}
		void UpdateCurvePointsPosition(){
			for(int i = 0; i < thisCurvePoints.Count; i ++){
				ICurvePoint point = thisCurvePoints[i];
				float normalizedT = point.GetNormalizedT();
				Vector3 position = CalculatePointPosition(normalizedT);
				point.SetPosition(position);
			}
		}
		void UpdateCurvePointsRotation(){
			for(int i = 0; i < thisCurvePoints.Count; i ++){
				ICurvePoint point = thisCurvePoints[i];
				float normalizedT = point.GetNormalizedT();
				Vector3 forward = CalculateForwardDirection(i);
				Vector3 up = CalculateUpwardDirection(normalizedT);
				point.LookAt(
					forward,
					up
				);
			}
		}
	}
}
