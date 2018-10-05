using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility.CurveUtility;

namespace AppleShooterProto{

	public interface IWaypointCurveAdaptor: IMonoBehaviourAdaptor{
		IWaypointCurve GetWaypointCurve();
		Transform GetCurvePointsParentTransform();
	}
	[ExecuteInEditMode]
	public class WaypointCurveAdaptor : MonoBehaviourAdaptor, IWaypointCurveAdaptor {
		public Color lineColor = new Color(.2f, 1f, 1f, 1f);
		public Color upDirColor = new Color(1f, .5f, 1f, 1f);
		public float upDirLineLength = 1f;
		public Transform curvePointsParent;
		public Transform controlPointsParent;
		public Transform GetCurvePointsParentTransform(){
			return curvePointsParent;
		}
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
					foreach(CurvePoint curvePoint in thisCurvePoints){
						if(thisCurvePoints.IndexOf(curvePoint) != 0){
							Vector3 position = curvePoint.position;
							Gizmos.color = lineColor;
							Gizmos.DrawLine(position, prev.GetPosition());
							Gizmos.color = upDirColor;
							Gizmos.DrawLine(position, position + curvePoint.GetUpDirection() * upDirLineLength);
						}
						prev = curvePoint;
					}
				}
			}
		}
		#endif
		public int curveSegmentResolution = 10;

		IWaypointCurve thisWaypointCurve;
		public override void SetUp(){
			IWaypointCurveConstArg arg = new WaypointCurveConstArg(
				this,
				thisControlPoints,
				thisCurvePoints
			);
			thisWaypointCurve = new WaypointCurve(arg);
		}
		public IWaypointCurve GetWaypointCurve(){
			return thisWaypointCurve;
		}
		List<ICurveControlPoint> thisControlPoints;
		List<ICurveSegment> thisCurveSegments;

		void UpdateControlPoints(){
			thisControlPoints = GetLatestControlPoints();
			MakeSureHeadAndTailControlPointsAreSet();
			UpdateCurveSegments();
			CreateNewCurvePointsForEachCurveSegment(curveSegmentResolution);
		}
		List<ICurveControlPoint> GetLatestControlPoints(){
			List<ICurveControlPoint> result = new List<ICurveControlPoint>();
			int controlPointCount = controlPointsParent.childCount;
			for(int i = 0; i < controlPointCount; i ++){
				Transform child = controlPointsParent.GetChild(i);
				ICurveControlPoint controlPoint = (ICurveControlPoint)child.GetComponent(typeof(ICurveControlPoint));
				if(controlPoint != null){
					result.Add(controlPoint);
				}
			}
			return result;
		}
		void MakeSureHeadAndTailControlPointsAreSet(){
			if(!(thisControlPoints[0] is TailCurveControlPoint))
				throw new System.InvalidOperationException(
					"the first entry of controlPoints should be TailControlPoint"
				);
			if(!(thisControlPoints[thisControlPoints.Count - 1] is HeadCurveControlPoint))
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
		List<ICurvePoint> thisCurvePoints;
		void UpdateCurvePoints(){
			if(cachedResolution != curveSegmentResolution){
				CreateNewCurvePointsForEachCurveSegment(curveSegmentResolution);
				cachedResolution = curveSegmentResolution;
			}else{
				UpdateCurvePointsTransformForEachCurveSegment();
			}
		}
		void CreateNewCurvePointsForEachCurveSegment(int resolution){
			List<ICurvePoint> result = new List<ICurvePoint>();
			foreach(ICurveSegment segment in thisCurveSegments){
				segment.CreateCurvePoints(resolution);
				List<ICurvePoint> curvePointsForSegment = segment.GetCurvePoints();
				result.AddRange(curvePointsForSegment);
			}
			thisCurvePoints = result;
		}
		void UpdateCurvePointsTransformForEachCurveSegment(){
			foreach(ICurveSegment segment in thisCurveSegments){
				segment.UpdateCurvePointsTransform();
			}
		}
		void CheckAndUpdateControlPoints(){
			if(thisControlPoints == null)
				UpdateControlPoints();
			else{
				List<ICurveControlPoint> controlPoints = GetLatestControlPoints();
				if(!ControlPointsMatchesWithCache(controlPoints)){
					UpdateControlPoints();
				}
			}
		}
		bool ControlPointsMatchesWithCache(List<ICurveControlPoint> newControlPoints){
			int cachedCount = thisControlPoints.Count;
			if(cachedCount != newControlPoints.Count){
				return false;
			}else{
				for(int i = 0 ; i < cachedCount; i ++){
					if(thisControlPoints[i] != newControlPoints[i])
						return false;
				}
				return true;
			}
		}
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
