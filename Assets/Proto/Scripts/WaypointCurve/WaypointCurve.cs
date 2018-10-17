﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility.CurveUtility;

namespace AppleShooterProto{
	public interface IWaypointCurve{
		IWaypointCurveAdaptor GetAdaptor();

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
		Quaternion CalculateRotationOnCurve(
			int ceilingIndex,
			float normalizedPositionBetweenPoints
		);
		void OnReserve();
		void OnUnreserve();


		int GetIndex();
		void SetIndex(int i);
		void SetPosition(Vector3 position);
		void SetRotation(Quaternion rotation);
		Vector3 GetPosition();
		Quaternion GetRotation();
		/* Events */
			List<IWaypointEvent> GetWaypointEvents();
			void SetWaypointEvents(List<IWaypointEvent> events);
		/*  */
	}
	public abstract class AbsWaypointCurve: IWaypointCurve{
		/* SetUp */
			public AbsWaypointCurve(
				IConstArg arg
			){
				thisAdaptor = arg.adaptor;
				thisControlPoints = arg.controlPoints;
				CalculateCurve();
			}
			readonly IWaypointCurveAdaptor thisAdaptor;
			public IWaypointCurveAdaptor GetAdaptor(){
				return thisAdaptor;
			}
			readonly ICurveControlPoint[] thisControlPoints;
			ICurveControlPoint thisLastControlPoint{
				get{return thisControlPoints[thisControlPoints.Length -1];}
			}
		/* Curve Calculation */
			public void CalculateCurve(){
				thisCurvePoints = CreateCurvePoints();
				thisTotalDistance = CalculateTotalDistance();
				PrintCurve();
			}
			void PrintCurve(){
				DKUtility.DebugHelper.PrintInBlue(
					"WaypointCurve: " + GetIndex().ToString() + ", " +
					"curvePointCount: " + thisCurvePoints.Length.ToString() + ", " +
					"totalDist: " + CalculateTotalDistance().ToString()
				);
				int index = 0;
				foreach(ICurvePoint point in thisCurvePoints){
					Debug.Log(
						index.ToString() + ": " + 
						"position: " + point.GetPosition().ToString() + ", " + 
						"rotation: " + point.GetRotation().ToString() + ", " +
						"delta: " + point.GetDelta().ToString() + ", " + 
						"sum: " + point.GetDistanceUpToPointOnSegment().ToString()
					);
				}
			}
			ICurvePoint[] thisCurvePoints;
			ICurvePoint[] CreateCurvePoints(){
				thisAdaptor.UpdateCurvePoints();
				return thisAdaptor.GetCurvePoints();
			}
			float thisTotalDistance;
			float CalculateTotalDistance(){
				return thisAdaptor.GetTotalDistance();
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
			}
			public Vector3 GetConnectionPosition(){
				return thisLastControlPoint.GetPosition();
			}
			public Quaternion GetConnectionRotation(){
				return thisLastControlPoint.GetRotation();
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
			public Quaternion CalculateRotationOnCurve(
				int ceilingIndex,
				float normalizedPositionBetweenPoints
			){
				Quaternion ceilingRotation = thisCurvePoints[ceilingIndex].GetRotation();
				Quaternion floorRotation =  thisCurvePoints[ceilingIndex -1].GetRotation();
				return Quaternion.Slerp(
					floorRotation,
					ceilingRotation,
					normalizedPositionBetweenPoints
				);
			}
		/* Misc */
			public Vector3 GetPosition(){
				return thisAdaptor.GetPosition();
			}
			public Quaternion GetRotation(){
				return thisAdaptor.GetRotation();
			}
			public void SetPosition(Vector3 position){
				thisAdaptor.SetPosition(position);
			}
			public void SetRotation(Quaternion rotation){
				thisAdaptor.SetRotation(rotation);
			}
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
				// this.CalculateCurve();
			}
		/* Const */
			public interface IConstArg{
				IWaypointCurveAdaptor adaptor{get;}
				ICurveControlPoint[] controlPoints{get;}
				ICurvePoint[] curvePoints{get;}
			}
			public struct ConstArg: IConstArg{
				public ConstArg(
					IWaypointCurveAdaptor adaptor,
					ICurveControlPoint[] controlPoints,
					ICurvePoint[] curvePoints
				){
					thisAdaptor = adaptor;
					thisControlPoints = controlPoints;
					thisCurvePoints = curvePoints;
				}
				readonly IWaypointCurveAdaptor thisAdaptor;
				public IWaypointCurveAdaptor adaptor{get{return thisAdaptor;}}
				readonly ICurveControlPoint[] thisControlPoints;
				public ICurveControlPoint[] controlPoints{get{return thisControlPoints;}}
				readonly ICurvePoint[] thisCurvePoints;
				public ICurvePoint[] curvePoints{get{return thisCurvePoints;}}
			}
		}
	/*  */
}
