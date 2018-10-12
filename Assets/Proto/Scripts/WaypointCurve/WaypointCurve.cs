using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility.CurveUtility;

namespace AppleShooterProto{
	public interface IWaypointCurve{
		IWaypointCurveAdaptor GetAdaptor();
		int GetIndex();
		void SetIndex(int i);
		void SetPosition(Vector3 position);
		void SetRotation(Quaternion rotation);
		void Connect(IWaypointCurve prevCurve);
		Vector3 GetConnectionPosition();
		Quaternion GetConnectionRotation();
		int GetIndexOfCeilingCurvePoint(float totalDistInCurve);
		float GetTotalDistance();
		float GetNormalizedPositionOnSegment(
			int ceilingIndex,
			float totalDistanceCoveredInCurve
		);
		Vector3 CalculateTargetPosition(
			int ceilingIndex,
			float normalizedPositionOnSegment
		);
		Quaternion CalculateTargetRotation(
			int ceilingIndex,
			float normalizedPositionOnSegment
		);
		Vector3 GetPosition();
		Quaternion GetRotation();
		List<IWaypointEvent> GetWaypointEvents();
		void SetWaypointEvents(List<IWaypointEvent> events);
		void SetTargetSpawnManager(ITargetSpawnManager targetSpawnManager);
		void SpawnTargets();
		void DespawnTargets();

		int[] GetSpawnIndices();
		ITestShootingTarget[] GetSpawnedShootingTargets();
	}
	public class WaypointCurve: IWaypointCurve{
		public WaypointCurve(
			IConstArg arg
		){
			thisAdaptor = arg.adaptor;
			thisCurvePoints = arg.curvePoints;
			thisControlPoints = arg.controlPoints;
			thisDeltaTable = CalculateDeltaTable();
			thisDistanceTable = CalculateDistanceTable();
			thisTotalDistance = CalculateTotalDistance();
		}
		readonly IWaypointCurveAdaptor thisAdaptor;
		public IWaypointCurveAdaptor GetAdaptor(){
			return thisAdaptor;
		}
		readonly List<ICurvePoint> thisCurvePoints;
		readonly List<ICurveControlPoint> thisControlPoints;
		ICurveControlPoint thisLastControlPoint{
			get{return thisControlPoints[thisControlPoints.Count -1];}
		}
		readonly List<float> thisDeltaTable;
		readonly List<float> thisDistanceTable;
		List<float> CalculateDeltaTable(){
			List<float> result = new List<float>();
			Vector3 prevCurvePointPosition = new Vector3();
			foreach(CurvePoint curvePoint in thisCurvePoints){
				if(result.Count == 0){
					result.Add(0f);
				}else{
					Vector3 delta = curvePoint.position - prevCurvePointPosition;
					result.Add(delta.magnitude);
				}
				prevCurvePointPosition = curvePoint.position;
			}
			return result;
		}
		List<float> CalculateDistanceTable(){
			List<float> result = new List<float>();
			float sum = 0f;
			foreach(float delta in thisDeltaTable){
				sum += delta;
				result.Add(sum);
			}
			return result;
		}
		public float GetTotalDistance(){
			return thisTotalDistance;
		}
		readonly float thisTotalDistance;
		
		int thisIndex;
		public int GetIndex(){
			return thisIndex;
		}
		public void SetIndex(int i){
			thisIndex = i;
		}

		float CalculateTotalDistance(){
			return thisDistanceTable[thisDistanceTable.Count - 1];
		}
		public void SetPosition(Vector3 position){
			thisAdaptor.SetPosition(position);
		}
		public void SetRotation(Quaternion rotation){
			thisAdaptor.SetRotation(rotation);
		}
		public void Connect(IWaypointCurve prevCurve){
			Vector3 position = prevCurve.GetConnectionPosition();
			Quaternion rotation = prevCurve.GetConnectionRotation();
			SetPosition(position);
			SetRotation(rotation);
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
			for(int i = 0; i < thisDistanceTable.Count; i ++){
				if(thisDistanceTable[i] > totalDistInCurve)
					return i;
			}
			return -1;
		}
		public float GetNormalizedPositionOnSegment(
			int ceilingIndex,
			float totalDistanceCoveredInCurve
		){
			int floorIndex = ceilingIndex -1;
			float distToFloor = thisDistanceTable[floorIndex];
			float residualDist = totalDistanceCoveredInCurve - distToFloor;
			float currentCurveSegmentLength = thisDeltaTable[ceilingIndex];
			
			return residualDist/ currentCurveSegmentLength;
		}
		public Vector3 CalculateTargetPosition(
			int ceilingIndex,
			float normalizedPositionOnSegment
		){
			ICurvePoint ceiling = thisCurvePoints[ceilingIndex];
			ICurvePoint floor = thisCurvePoints[ceilingIndex -1];
			return Vector3.Lerp(
				floor.GetPosition(),
				ceiling.GetPosition(),
				normalizedPositionOnSegment
			);
		}
		public Quaternion CalculateTargetRotation(
			int ceilingIndex,
			float normalizedPositionOnSegment
		){
			/*  ceilingIndex cannot be 0
				returned value does not have to be normalized
			*/
			ICurvePoint ceiling = thisCurvePoints[ceilingIndex];
			ICurvePoint floor = thisCurvePoints[ceilingIndex - 1];
			return Quaternion.Slerp(
				floor.GetRotation(),
				ceiling.GetRotation(),
				normalizedPositionOnSegment
			);
		}
		public Vector3 GetPosition(){
			return thisAdaptor.GetPosition();
		}
		public Quaternion GetRotation(){
			return thisAdaptor.GetRotation();
		}
		/*  */
		public List<IWaypointEvent> GetWaypointEvents(){
			return thisWaypointEvents;
		}
		List<IWaypointEvent> thisWaypointEvents;
		public void SetWaypointEvents(List<IWaypointEvent> events){
			thisWaypointEvents = events;
		}
		/* target Spawn */
			ITargetSpawnManager thisTargetSpawnManager;
			public void SetTargetSpawnManager(ITargetSpawnManager manager){
				thisTargetSpawnManager = manager;
			}
			public void SpawnTargets(){
				thisTargetSpawnManager.Spawn();
			}
			public void DespawnTargets(){
				thisTargetSpawnManager.Despawn();
			}
		/*  */
			public int[] GetSpawnIndices(){
				return thisTargetSpawnManager.GetSpawnPointIndices();
			}
			public ITestShootingTarget[] GetSpawnedShootingTargets(){
				ITestTargetSpawnManager typedManager = (ITestTargetSpawnManager)thisTargetSpawnManager;
				return typedManager.GetSpawnedTestShootingTargets();
			}
		/* Const */
			public interface IConstArg{
				IWaypointCurveAdaptor adaptor{get;}
				List<ICurveControlPoint> controlPoints{get;}
				List<ICurvePoint> curvePoints{get;}
			}
			public struct ConstArg: IConstArg{
				public ConstArg(
					IWaypointCurveAdaptor adaptor,
					List<ICurveControlPoint> controlPoints,
					List<ICurvePoint> curvePoints
				){
					thisAdaptor = adaptor;
					thisControlPoints = controlPoints;
					thisCurvePoints = curvePoints;
				}
				readonly IWaypointCurveAdaptor thisAdaptor;
				public IWaypointCurveAdaptor adaptor{get{return thisAdaptor;}}
				readonly List<ICurveControlPoint> thisControlPoints;
				public List<ICurveControlPoint> controlPoints{get{return thisControlPoints;}}
				readonly List<ICurvePoint> thisCurvePoints;
				public List<ICurvePoint> curvePoints{get{return thisCurvePoints;}}
			}
		}
	/*  */
}
