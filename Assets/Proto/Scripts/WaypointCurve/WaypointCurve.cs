﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility.CurveUtility;

namespace AppleShooterProto{
	public interface IWaypointCurve: ISceneObject{
		// IWaypointCurveAdaptor GetAdaptor();

		void CalculateCurve();


		void Connect(IWaypointCurve prevCurve);
		Vector3 GetConnectionPosition();
		Quaternion GetConnectionRotation();
		int GetIndexOfCeilingCurvePoint(float totalDistInCurve);
		float GetTotalDistance();
		float GetNormalizedPositionBetweenPoints(
			int ceilingIndex,
			float totalDistanceCoveredInCurve
		);
		Vector3 CalculatePositionOnCurve(
			int ceilingIndex,
			float normalizedPositionBetweenPoints
		);
		Vector3 CalculateForwardDirectionOnCurve(
			int ceilingIndex,
			float normalizedPositionBetweenPoints
		);
		Vector3 CalculateUpDirectionOnCurve(
			int ceilingIndex,
			float normalizedPositionBetweenPoints
		);
		Vector3 GetLastCurvePointPrevPosition();
		void OnReserve();
		void OnUnreserve();


		int GetIndex();
		void SetIndex(int i);

		/* Events */
			List<IWaypointEvent> GetWaypointEvents();
			void SetWaypointEvents(List<IWaypointEvent> events);
		/*  */
			void PrintCurve();
	}
	public abstract class AbsWaypointCurve: AbsSceneObject, IWaypointCurve{
		/* SetUp */
			public AbsWaypointCurve(
				IConstArg arg
			): base(
				arg
			){
				thisControlPoints = arg.controlPoints;
				CalculateCurve();
			}
			protected IWaypointCurveAdaptor thisTypedAdaptor{
				get{return (IWaypointCurveAdaptor)thisAdaptor;}
			}
			readonly ICurveControlPoint[] thisControlPoints;
			ICurveControlPoint thisLastControlPoint{
				get{
					return thisControlPoints[thisControlPoints.Length -1];
				}
			}
		/* Curve Calculation */
			public virtual void CalculateCurve(){
				thisCurvePoints = CreateCurvePoints();
				thisTotalDistance = CalculateTotalDistance();
			}
			public void PrintCurve(){
				Debug.Log(
					DKUtility.DebugHelper.BlueString(
						"WaypointCurve: " + GetIndex().ToString() + ", " +
						"curvePointCount: " + thisCurvePoints.Length.ToString() + ", " +
						"totalDist: " + CalculateTotalDistance().ToString()
					)
				);
				int index = 0;
				foreach(ICurvePoint point in thisCurvePoints){
					Debug.Log(
						index.ToString() + ": " + 
						"position: " + point.GetPosition().ToString() + ", " + 
						"up: " + point.GetUpDirection().ToString() + ", " +
						"delta: " + point.GetDelta().ToString() + ", " + 
						"sumInSeg: " + point.GetDistanceUpToPointOnSegment().ToString() + ", " +
						"sumInCuv: " + point.GetDistanceUpToPointOnCurve().ToString() + ", " + 
						"prevPos: " + point.GetPrevPointPosition().ToString()
					);
					index ++;
				}
			}
			ICurvePoint[] thisCurvePoints;
			ICurvePoint[] CreateCurvePoints(){
				thisTypedAdaptor.UpdateCurve();
				return thisTypedAdaptor.GetCurvePoints();
			}
			float thisTotalDistance;
			float CalculateTotalDistance(){
				return thisTypedAdaptor.GetTotalDistance();
			}
			public float GetTotalDistance(){
				return thisTotalDistance;
			}
		/* Curve other */
			public void Connect(IWaypointCurve prevCurve){
				Vector3 position = prevCurve.GetConnectionPosition();
				Quaternion rotation = prevCurve.GetConnectionRotation();
				SetPosition(position);
				SetRotation(rotation);
				CalculateCurve();
				SetPrevPointPosOnFirstCurvePointOnConnection(prevCurve);
			}
			public Vector3 GetConnectionPosition(){
				return thisLastControlPoint.GetPosition();
			}
			public Quaternion GetConnectionRotation(){
				return thisLastControlPoint.GetRotation();
			}
			void SetPrevPointPosOnFirstCurvePointOnConnection(
				IWaypointCurve prevCurve
			){
				Vector3 prevCurveLastCurvePointPrevPosition =  prevCurve.GetLastCurvePointPrevPosition();
				ICurvePoint firstPointOfThisCurve = thisCurvePoints[0];
				firstPointOfThisCurve.SetPrevPointPosition(prevCurveLastCurvePointPrevPosition);
 			}
			public int GetIndexOfCeilingCurvePoint(float totalDistInCurve){
				/*  totalDist must be less than thisTotalDistnce
					(cannot be even equal to it)
					must be checked before this

					never returns 0
				*/
				for(int i = 0; i < thisCurvePoints.Length; i ++){
					if(thisCurvePoints[i].GetDistanceUpToPointOnCurve() > totalDistInCurve){
						return i;
					}
				}
				return -1;
			}
			public float GetNormalizedPositionBetweenPoints(
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
			public Vector3 CalculatePositionOnCurve(
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
			public Vector3 CalculateForwardDirectionOnCurve(
				int ceilingIndex,
				float normalizedPositionBetweenPoints
			){
				ICurvePoint ceiling = thisCurvePoints[ceilingIndex];
				ICurvePoint floor = thisCurvePoints[ceilingIndex -1];

				Vector3 ceilingForward = ceiling.GetPosition() - floor.GetPosition();
				Vector3 floorForward = floor.GetPosition() - floor.GetPrevPointPosition();
				
				return Vector3.Lerp(
					floorForward,
					ceilingForward,
					normalizedPositionBetweenPoints
				);
			}
			public Vector3 CalculateUpDirectionOnCurve(
				int ceilingIndex,
				float normalizedPositionBetweenPoints
			){
				Vector3 ceilingUp = thisCurvePoints[ceilingIndex].GetUpDirection();
				Vector3 floorUp = thisCurvePoints[ceilingIndex -1].GetUpDirection();
				return Vector3.Lerp(
					floorUp,
					ceilingUp,
					normalizedPositionBetweenPoints
				);
			}
			public Vector3 GetLastCurvePointPrevPosition(){
				ICurvePoint lastCurvePoint = thisCurvePoints[thisCurvePoints.Length -1];
				return lastCurvePoint.GetPrevPointPosition();
			}
		/* Misc */
			int thisIndex;
			public int GetIndex(){
				return thisIndex;
			}
			public void SetIndex(int i){
				thisIndex = i;
			}
		/* Events */
			public List<IWaypointEvent> GetWaypointEvents(){
				return thisWaypointEvents;
			}
			List<IWaypointEvent> thisWaypointEvents;
			public void SetWaypointEvents(List<IWaypointEvent> events){
				thisWaypointEvents = events;
			}
		/* abs */
			public abstract void OnReserve();
			public virtual void OnUnreserve(){
			}
		/* Const */
			public new interface IConstArg: AbsSceneObject.IConstArg{
				ICurveControlPoint[] controlPoints{get;}
				ICurvePoint[] curvePoints{get;}
			}
			public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
				public ConstArg(
					IWaypointCurveAdaptor adaptor,
					ICurveControlPoint[] controlPoints,
					ICurvePoint[] curvePoints
				): base(
					adaptor
				){
					thisControlPoints = controlPoints;
					thisCurvePoints = curvePoints;
				}
				readonly ICurveControlPoint[] thisControlPoints;
				public ICurveControlPoint[] controlPoints{get{return thisControlPoints;}}
				readonly ICurvePoint[] thisCurvePoints;
				public ICurvePoint[] curvePoints{get{return thisCurvePoints;}}
			}
		}
	/*  */
}
