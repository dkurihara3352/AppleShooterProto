using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility.CurveUtility;

namespace AppleShooterProto{

	public interface IWaypointCurveAdaptor: IMonoBehaviourAdaptor{
		IWaypointCurve GetWaypointCurve();
	}
	[ExecuteInEditMode]
	public class WaypointCurveAdaptor : MonoBehaviourAdaptor, IWaypointCurveAdaptor {
		public Color lineColor = new Color(.2f, 1f, 1f, 1f);
		public Color upDirColor = new Color(1f, .5f, 1f, 1f);
		public float upDirLineLength = 1f;
		
		#if UNITY_EDITOR
		int cachedResolution = 0;
		public void Start(){
			UpdateControlPoints();
			CreateNewCurvePointsForEachCurveSegment(curveSegmentResolution);
		}
		public void Update (){
			/*  need to check if child has changed, and perform UpdateControlPoints if changed
			*/
			UpdateCurvePoints();
		}
		void OnDrawGizmos(){
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
			Debug.Log("controlPoints updated");
			List<ICurveControlPoint> result = new List<ICurveControlPoint>();
			for(int i = 0; i < transform.childCount; i ++){
				Transform child = transform.GetChild(i);
				ICurveControlPoint controlPoint = (ICurveControlPoint)child.GetComponent(typeof(ICurveControlPoint));
				if(controlPoint != null){
					result.Add(controlPoint);
				}
			}
			thisControlPoints = result;
			MakeSureHeadAndTailControlPointsAreSet();
			UpdateCurveSegments();
			// CreateNewCurvePointsForEachCurveSegment(curveSegmentResolution);
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
	}
}
