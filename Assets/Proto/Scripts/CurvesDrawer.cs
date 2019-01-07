using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	[ExecuteInEditMode]
	public class CurvesDrawer : MonoBehaviour {

		IDrawableCurve[] curves{
			get{
				return CollectDrawableCurve();
			}
		}
		IDrawableCurve[] CollectDrawableCurve(){
			List<IDrawableCurve> resultList = new List<IDrawableCurve>();
			if(waypointsCurveAdaptors != null)
				foreach(IWaypointCurveAdaptor curveAdaptor in waypointsCurveAdaptors){
					resultList.Add((IDrawableCurve)curveAdaptor);
				}
			if(glidingTargetWaypointCurveGroupAdaptors != null){
				foreach(IGlidingTargetWaypointCurveGroupAdaptor waypointCurveGroupAdaptor in glidingTargetWaypointCurveGroupAdaptors){
					if(waypointCurveGroupAdaptor != null)
						resultList.AddRange(waypointCurveGroupAdaptor.GetCurveAdaptors());
				}
			}
			return resultList.ToArray();
		}
		public AbsWaypointCurveAdaptor[] waypointsCurveAdaptors;
		public GlidingTargetWaypointCurveGroupAdaptor[] glidingTargetWaypointCurveGroupAdaptors;
		void OnDrawGizmos(){
			if(curves != null)
				foreach(IDrawableCurve curve in curves){
					if(curve != null)
						DrawCurve(curve);
				}
		}
		public bool draws = true;
		void DrawCurve(IDrawableCurve curve){
			if(draws){
				curve.Calculate();

				ICurveControlPoint[] controlPoints = curve.GetCurveControlPoints();
				foreach(ICurveControlPoint controlPoint in controlPoints){

					Vector3 controlPointPosition = controlPoint.GetPosition();
					Color handleColor = curve.GetHandleColor();
					Gizmos.color = handleColor;
					Vector3 foreHandlePosition = controlPoint.GetForeHandle().position;
					// Gizmos.DrawCube(
					// 	foreHandlePosition,
					// 	curve.GetControlPointHandleSize()
					// );
					Gizmos.DrawLine(
						controlPointPosition,
						foreHandlePosition
					);
					Vector3 backHandlePosition = controlPoint.GetBackHandle().position;
					// Gizmos.DrawCube(
					// 	backHandlePosition,
					// 	curve.GetControlPointHandleSize()
					// );
					Gizmos.DrawLine(
						controlPointPosition,
						backHandlePosition
					);

					Gizmos.color = curve.GetControlPointColor();
					// Gizmos.DrawCube(
					// 	controlPointPosition,
					// 	curve.GetControlPointDrawSize()
					// );
				}

				ICurvePoint[] curvePoints = curve.GetCurvePoints();
				Color lineColor = curve.GetLineColor();
				Color upDirColor =  curve.GetUpDirColor();
				float upDirLength = curve.GetUpDirLineLength();

				foreach(ICurvePoint point in curvePoints){
					Vector3 pointPosition = point.GetPosition();
					if(point != curvePoints[0]){
						Vector3 prevPointPos = point.GetPrevPointPosition();
						Gizmos.color = lineColor;
						Gizmos.DrawLine(
							pointPosition, 
							prevPointPos
						);
					}
					Vector3 upDir = point.GetUpDirection().normalized;
					Gizmos.color = upDirColor;
					Gizmos.DrawLine(pointPosition, pointPosition + upDir * upDirLength);
				}
			}

		}
	}

}
