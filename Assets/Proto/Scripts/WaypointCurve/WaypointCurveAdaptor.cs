using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility.CurveUtility;

namespace AppleShooterProto{

	public interface IWaypointCurveAdaptor: IMonoBehaviourAdaptor{
		IWaypointCurve GetWaypointCurve();
		void UpdateCurve();
		ICurvePoint[] GetCurvePoints();
		float GetTotalDistance();
	}
	[ExecuteInEditMode]
	public abstract class AbsWaypointCurveAdaptor : MonoBehaviourAdaptor, IWaypointCurveAdaptor {
		public Color lineColor = new Color(.2f, 1f, 1f, 1f);
		public Color upDirColor = new Color(1f, .5f, 1f, 1f);
		public float upDirLineLength = 1f;
		public Transform controlPointsParent;
		public void UpdateCurve(){
			CheckAndUpdateControlPoints();
			UpdateCurvePoints();
		}

		int cachedResolution = 0;
		public bool drawGizmos = true;
		// #if UNITY_EDITOR
		// #endif
		public void Start(){
			UpdateCurve();
		}
		public void Update(){
			if(!Application.isPlaying){
				CheckAndUpdateControlPoints();
			}
		}
		void OnDrawGizmos(){
			if(drawGizmos){
				if(thisCurvePoints != null){
					ICurvePoint prev = null;
					int index = 0;
					foreach(CurvePoint curvePoint in thisCurvePoints){
						if(index != 0){
							Vector3 position = curvePoint.GetPosition();
							Gizmos.color = lineColor;
							Gizmos.DrawLine(position, prev.GetPosition());
							Gizmos.color = upDirColor;
							Vector3 up = curvePoint.GetUpDirection().normalized;
							Gizmos.DrawLine(position, position + up * upDirLineLength);
						}
						index ++;
						prev = curvePoint;
					}
				}
			}
		}
		/* Curve, segment, controlPoints */
			public int curveSegmentResolution = 10;

			protected IWaypointCurve thisWaypointCurve;
			public IWaypointCurve GetWaypointCurve(){
				return thisWaypointCurve;
			}
			protected ICurveControlPoint[] thisControlPoints;
			List<ICurveSegment> thisCurveSegments;

			void UpdateControlPoints(){
				thisControlPoints = GetLatestControlPoints();
				SetSelfOnAllControlPoints();
				MakeSureHeadAndTailControlPointsAreSet();
				UpdateCurveSegments();
				CreateNewCurvePointsForEachCurveSegment(curveSegmentResolution);
			}
			ICurveControlPoint[] GetLatestControlPoints(){
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
			void SetSelfOnAllControlPoints(){
				foreach(ICurveControlPoint controlPoint in thisControlPoints){
					controlPoint.SetWaypointCurveAdaptor(this);
				}
			}
			void MakeSureHeadAndTailControlPointsAreSet(){
				if(!(thisControlPoints[0] is TailCurveControlPoint))
					throw new System.InvalidOperationException(
						"the first entry of controlPoints should be TailControlPoint"
					);
				if(!(thisControlPoints[thisControlPoints.Length - 1] is HeadCurveControlPoint))
					throw new System.InvalidOperationException(
						"the last entry of controlPoints should be HeadControlPoint"
					);
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
				thisCurveSegments = result;
			}
			void CheckAndUpdateControlPoints(){
				if(thisControlPoints == null)
					UpdateControlPoints();
				else{
					ICurveControlPoint[] controlPoints = GetLatestControlPoints();
					if(!ControlPointsMatchesWithCache(controlPoints)){
						UpdateControlPoints();
					}
				}
			}
			bool ControlPointsMatchesWithCache(ICurveControlPoint[] newControlPoints){
				int cachedCount = thisControlPoints.Length;
				if(cachedCount != newControlPoints.Length){
					return false;
				}else{
					for(int i = 0 ; i < cachedCount; i ++){
						if(thisControlPoints[i] != newControlPoints[i])
							return false;
					}
					return true;
				}
			}
			public float GetTotalDistance(){
				float sum = 0f;
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
				if(cachedResolution != curveSegmentResolution){
					CreateNewCurvePointsForEachCurveSegment(curveSegmentResolution);
					cachedResolution = curveSegmentResolution;
				}else{
					UpdateCurvePointsTransformForEachCurveSegment();
				}
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
				List<ICurvePoint> result = new List<ICurvePoint>();
				foreach(ICurveSegment segment in thisCurveSegments){
					segment.SetCurveResolution(resolution);
					segment.UpdateCurvePoints();
					ICurvePoint[] curvePointsForSegment = segment.GetCurvePoints();
					result.AddRange(curvePointsForSegment);
				}
			}
			void UpdateCurvePointsTransformForEachCurveSegment(){
				foreach(ICurveSegment segment in thisCurveSegments){
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
