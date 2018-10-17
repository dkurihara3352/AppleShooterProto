using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility.CurveUtility;

namespace AppleShooterProto{

	public interface IWaypointCurveAdaptor: IMonoBehaviourAdaptor{
		IWaypointCurve GetWaypointCurve();
		// Transform GetCurvePointsParentTransform();
		void UpdateCurvePoints();
		ICurvePoint[] GetCurvePoints();
		float GetTotalDistance();
	}
	[ExecuteInEditMode]
	public abstract class AbsWaypointCurveAdaptor : MonoBehaviourAdaptor, IWaypointCurveAdaptor {
		public Color lineColor = new Color(.2f, 1f, 1f, 1f);
		public Color upDirColor = new Color(1f, .5f, 1f, 1f);
		public float upDirLineLength = 1f;
		public Transform controlPointsParent;
		#if UNITY_EDITOR
			int cachedResolution = 0;
			public void Start(){
				UpdateControlPoints();
			}
			public void Update (){
				/*  need to check if child has changed, and perform UpdateControlPoints if changed
				*/
				CheckAndUpdateControlPoints();
				UpdateCurvePoints();
			}
			public bool drawGizmos = true;
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
								Quaternion rotation = curvePoint.GetRotation();
								Vector3 upDirection = rotation * Vector3.up;
								Gizmos.DrawLine(position, position + upDirection * upDirLineLength);
							}
							index ++;
							prev = curvePoint;
						}
					}
				}
			}
		#endif
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
					// ICurvePoint[] curvePoints = segment.GetCurvePoints();
					// ICurvePoint lastCurvePoint = curvePoints[curvePoints.Length-1];
					// sum += lastCurvePoint.GetDistanceUpToPointOnSegment();
					sum += segment.GetLength();
				}
				return sum;
			}
		/* CurvePoints */
			protected ICurvePoint[] thisCurvePoints;
			public ICurvePoint[] GetCurvePoints(){
				return thisCurvePoints;
			}
			void ObsUpdateCurvePoints(){
				if(cachedResolution != curveSegmentResolution){
					CreateNewCurvePointsForEachCurveSegment(curveSegmentResolution);
					cachedResolution = curveSegmentResolution;
				}else{
					UpdateCurvePointsTransformForEachCurveSegment();
				}
			}
			public void UpdateCurvePoints(){
				if(cachedResolution != curveSegmentResolution){
					CreateNewCurvePointsForEachCurveSegment(curveSegmentResolution);
					cachedResolution = curveSegmentResolution;
				}else{
					UpdateCurvePointsTransformForEachCurveSegment();
				}
				SetTotalDistanceInCurveOnAllCurvePoints();
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
		/*  */
		public override void SetUpReference(){
			List<IWaypointEvent> waypointEvents = CollectWaypointEvents();
			thisWaypointCurve.SetWaypointEvents(waypointEvents);
		}
		List<IWaypointEvent> CollectWaypointEvents(){
			List<IWaypointEvent> result = new List<IWaypointEvent>();
			List<IWaypointEventAdaptor> adaptors = new List<IWaypointEventAdaptor>();
			Component[] components = this.transform.GetComponents(typeof(Component));
			foreach(Component component in components){
				if(component is IWaypointEventAdaptor)
					adaptors.Add((IWaypointEventAdaptor)component);
			}
			Debug.Log(
				this.transform.name + "'s components count: " + components.Length.ToString()
			);
			if(adaptors != null){
				foreach(IWaypointEventAdaptor adaptor in adaptors)
					result.Add(adaptor.GetWaypointEvent());
				IWaypointEventComparer comparer = new WaypointEventComparer();
				result.Sort(comparer);
			}
			Debug.Log(
				this.transform.name +  "'s eventCount: " + result.Count.ToString()
			);
			return result;
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
