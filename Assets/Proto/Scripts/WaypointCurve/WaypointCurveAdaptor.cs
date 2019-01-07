using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility.CurveUtility;

namespace SlickBowShooting{

	public interface IWaypointCurveAdaptor: ISlickBowShootingMonoBehaviourAdaptor, IDrawableCurve{
		IWaypointCurve GetWaypointCurve();
		float GetTotalDistance();
		Vector3 GetPositionOnCurve(float normalizedPosition);
	}
	public abstract class AbsWaypointCurveAdaptor : SlickBowShootingMonoBehaviourAdaptor, IWaypointCurveAdaptor{
		public Color lineColor = new Color(.2f, 1f, 1f, 1f);
		public Color GetLineColor(){
			return lineColor;
		}
		public Color upDirColor = new Color(1f, .5f, 1f, 1f);
		public Color GetUpDirColor(){
			return upDirColor;
		}
		public Vector3 controlPointDrawSize = Vector3.one;
		public Vector3 GetControlPointDrawSize(){
			return controlPointDrawSize;
		}
		public Vector3 controlPointHandleSize = Vector3.one;
		public Vector3 GetControlPointHandleSize(){
			return controlPointHandleSize;
		}
		public Color controlPointColor = Color.red;
		public Color GetControlPointColor(){
			return controlPointColor;
		}
		public Color handleColor = Color.yellow;
		public Color GetHandleColor(){
			return handleColor;
		}
		public float upDirLineLength = 1f;
		public float GetUpDirLineLength(){
			return upDirLineLength;
		}
		public Transform controlPointsParent;

		public void Calculate(){
			UpdateControlPoints();
		}
		/* Curve, segment, controlPoints */
			public int curveSegmentResolution = 10;

			protected IWaypointCurve thisWaypointCurve;
			public IWaypointCurve GetWaypointCurve(){
				return thisWaypointCurve;
			}
			protected ICurveControlPoint[] thisControlPoints;
			public ICurveControlPoint[] GetCurveControlPoints(){
				return thisControlPoints;
			}
			ICurveSegment[] thisCurveSegments;

			void UpdateControlPoints(){
				thisControlPoints = CollectControlPoints();
				UpdateCurveSegments();
				UpdateCurvePoints();

				CreateNewCurvePointsForEachCurveSegment(curveSegmentResolution);
			}
			ICurveControlPoint[] CollectControlPoints(){
				List<ICurveControlPoint> result = new List<ICurveControlPoint>();
				int controlPointCount = controlPointsParent.childCount;
				for(int i = 0; i < controlPointCount; i ++){
					Transform child = controlPointsParent.GetChild(i);
					ICurveControlPoint controlPoint = (ICurveControlPoint)child.GetComponent(typeof(ICurveControlPoint));
					if(controlPoint != null){
						result.Add(controlPoint);
					}
				}
				return result.ToArray();
			}

			void UpdateCurveSegments(){
				List<ICurveSegment> result = new List<ICurveSegment>();
				ICurveControlPoint prev = null;
				foreach(ICurveControlPoint controlPoint in thisControlPoints){
					if(prev != null){
						ICurveSegment segment = new CurveSegment(
							this,
							prev,
							controlPoint
						);
						result.Add(segment);
					}
					prev = controlPoint;
				}
				thisCurveSegments = result.ToArray();
			}
			public float GetTotalDistance(){
				float sum = 0f;
				if(thisCurveSegments != null)
					foreach(ICurveSegment segment in thisCurveSegments){
						sum += segment.GetLength();
					}
				return sum;
			}
		/* CurvePoints */
			protected ICurvePoint[] thisCurvePoints;
			public ICurvePoint[] GetCurvePoints(){
				return thisCurvePoints;
			}
			void UpdateCurvePoints(){

				CreateNewCurvePointsForEachCurveSegment(curveSegmentResolution);

				SetTotalDistanceInCurveOnAllCurvePoints();
				SetPrevPointPositionOnAllCurvePoints();
				thisCurvePoints = CollectCurvePointsFromAllSegments();
			}
			ICurvePoint[] CollectCurvePointsFromAllSegments(){
				List<ICurvePoint> result = new List<ICurvePoint>();
				foreach(ICurveSegment segment in thisCurveSegments){
					result.AddRange(segment.GetCurvePoints());
				}
				return result.ToArray();
			}	
			void CreateNewCurvePointsForEachCurveSegment(int resolution){
			
				foreach(ICurveSegment segment in thisCurveSegments){
					segment.SetCurveResolution(resolution);
					segment.UpdateCurvePoints();
			
				}
			}

			void SetTotalDistanceInCurveOnAllCurvePoints(){
				float sumOfAllPrevSegmentsLength = 0f;
				foreach(ICurveSegment segment in thisCurveSegments){
					foreach(ICurvePoint curvePoint in segment.GetCurvePoints()){
						float distanceUpToPointOnSegment = curvePoint.GetDistanceUpToPointOnSegment();
						float distanceUpToPointOnCurve = distanceUpToPointOnSegment + sumOfAllPrevSegmentsLength;
						curvePoint.SetDistanceUpToPointOnCurve(distanceUpToPointOnCurve);
					}
					float segmentLength = segment.GetLength();
					sumOfAllPrevSegmentsLength += segmentLength;
				}
			}
			void SetPrevPointPositionOnAllCurvePoints(){
				ICurveSegment prevSegment = null;
				foreach(ICurveSegment segment in thisCurveSegments){
					Vector3 prevPosition = new Vector3();
					bool first = true;
					foreach(ICurvePoint curvePoint in segment.GetCurvePoints()){
						Vector3 curvePosition = curvePoint.GetPosition();
						if(first){
							if(prevSegment == null)
								prevPosition = curvePosition;
							else{
								prevPosition = prevSegment.GetLastCurvePointPrevPosition();
							}
							first = false;
						}
						curvePoint.SetPrevPointPosition(prevPosition);
						prevPosition = curvePosition;
					}
					prevSegment = segment;
				}
			}
			ICurvePoint[] SetPrevPositionOnAllCurvePoints(ICurvePoint[] source){
				ICurvePoint[] result = new ICurvePoint[source.Length];
				Vector3 prevPosition = new Vector3();
				int index = 0;
				foreach(ICurvePoint curvePoint in source){
					Vector3 curvePosition = curvePoint.GetPosition();
					if(index == 0)
						curvePoint.SetPrevPointPosition(curvePosition);
					else{
						curvePoint.SetPrevPointPosition(prevPosition);
					}
					prevPosition = curvePosition;
					result[index] = curvePoint;
					index ++;
				}
				return result;
			}
		/*  below is for event point drawing
			overlap with curve imple
		*/
		public Vector3 GetPositionOnCurve(float normalizedPosition){
			float totalDistance = GetTotalDistance();
			float totalDistanceCoveredInCurve = normalizedPosition * totalDistance;
			int ceilingIndex = GetIndexOfCeilingCurvePoint(totalDistanceCoveredInCurve);
			if(ceilingIndex != -1){
				float normalizedPositionBetweenPoints = GetNormalizedPositionBetweenPoints(
					ceilingIndex,
					totalDistanceCoveredInCurve
				);
				return CalculatePositionOnCurve(
					ceilingIndex,
					normalizedPositionBetweenPoints
				);
			}else
				return Vector3.zero;
		}
		int GetIndexOfCeilingCurvePoint(float totalDistInCurve){
			/*  totalDist must be less than thisTotalDistnce
				(cannot be even equal to it)
				must be checked before this

				never returns 0
			*/

			// if(thisCurvePoints == null)
			// 	UpdateCurve();
			if(thisCurvePoints != null)
				for(int i = 0; i < thisCurvePoints.Length; i ++){
					if(thisCurvePoints[i].GetDistanceUpToPointOnCurve() > totalDistInCurve){
						return i;
					}
				}
			return -1;
		}
		float GetNormalizedPositionBetweenPoints(
			int ceilingIndex,
			float totalDistanceCoveredInCurve
		){
			if(ceilingIndex == 0)
				return 0f;
			else{
				int floorIndex = ceilingIndex -1;
				float distToFloor = thisCurvePoints[floorIndex].GetDistanceUpToPointOnCurve();
				float residualDist = totalDistanceCoveredInCurve - distToFloor;
				float lengthBetweenPoints = thisCurvePoints[ceilingIndex].GetDelta();
				
				return residualDist/ lengthBetweenPoints;
			}
		}
		Vector3 CalculatePositionOnCurve(
			int ceilingIndex,
			float normalizedPositionBetweenPoints
		){
			ICurvePoint ceiling = thisCurvePoints[ceilingIndex];
			ICurvePoint floor = thisCurvePoints[ceilingIndex -1];
			return Vector3.Lerp(
				floor.GetPosition(),
				ceiling.GetPosition(),
				normalizedPositionBetweenPoints
			);
		}
		/*  */
		public override void SetUpReference(){
			IWaypointEvent[] waypointEvents = CollectWaypointEvents();
			thisWaypointCurve.SetWaypointEvents(waypointEvents);
		}
		IWaypointEvent[] CollectWaypointEvents(){
			List<IWaypointEvent> resultList = new List<IWaypointEvent>();
			List<IWaypointEventAdaptor> adaptors = new List<IWaypointEventAdaptor>();
			Component[] components = this.transform.GetComponents(typeof(Component));
			foreach(Component component in components){
				if(component is IWaypointEventAdaptor)
					adaptors.Add((IWaypointEventAdaptor)component);
			}

			if(adaptors != null){
				foreach(IWaypointEventAdaptor adaptor in adaptors)
					resultList.Add(adaptor.GetWaypointEvent());
				IWaypointEventComparer comparer = new WaypointEventComparer();
				resultList.Sort(comparer);
			}
			return resultList.ToArray();
		}
	}
	public interface IWaypointEventComparer: IComparer<IWaypointEvent>{
	}
	public class WaypointEventComparer: IWaypointEventComparer{
		public int Compare(IWaypointEvent a, IWaypointEvent b){
			return a.GetEventPoint().CompareTo(b.GetEventPoint());
		}
	}
	
}
