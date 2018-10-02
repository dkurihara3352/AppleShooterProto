using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointsManager{

		void SetWaypointsFollower(IWaypointsFollower follower);
		void SetWaypointCurves(List<IWaypointCurve> curves);

		List<IWaypointCurve> GetWaypointCurvesInSequence();

		void PlaceWaypointCurves();
		IWaypointCurve GetNextWaypointCurve(
			IWaypointCurve currentGroup
		);
		void CycleCurve();
		int GetWaypointCurveIndex(IWaypointCurve curve);
		IWaypointsFollower GetFollower();

		IWaypointCurve GetNextCurve(IWaypointCurve currentCurve);
	}

	public class WaypointsManager : IWaypointsManager {
		public WaypointsManager(IWaypointsManagerConstArg arg){
			thisCurveReservePointTransform = arg.curveReserveTransform;
			thisCurveCountInSequence = arg.curvesCountInSequence;
			thisInitialCurvePosition = arg.initialCurvePosition;
			thisInitialCurveRotation = arg.initialCurveRotation;
		}
		readonly Transform thisCurveReservePointTransform;
		Vector3 thisReservePosition{
			get{return thisCurveReservePointTransform.position;}
		}
		readonly Vector3 thisInitialCurvePosition;
		readonly Quaternion thisInitialCurveRotation;
		readonly int thisCurveCountInSequence;


		IWaypointsFollower thisFollower;
		public IWaypointsFollower GetFollower(){
			return thisFollower;
		}
		public void SetWaypointsFollower(IWaypointsFollower follower){
			thisFollower = follower;
		}
		public float GetSpeed(){return thisFollower.GetFollowSpeed();}
		List<IWaypointCurve> thisWaypointCurves;
		public void SetWaypointCurves(List<IWaypointCurve> curves){
			thisWaypointCurves = curves;
		}
		public int GetWaypointCurveIndex(IWaypointCurve curve){
			return thisWaypointCurves.IndexOf(curve);
		}

		public void PlaceWaypointCurves(){
			PlaceAllWaypointCurvesAtReserve();
			thisCurveSequence = CreateSequenceOfWaypointCurves();
			ConnectWaypointCurveSequence();
		}
		void PlaceAllWaypointCurvesAtReserve(){
			foreach(IWaypointCurve curve in thisWaypointCurves){
				curve.SetPosition(thisReservePosition);
			}
		}
		
		/* Creating sequence */
			List<IWaypointCurve> thisCurveSequence;
			public List<IWaypointCurve> GetWaypointCurvesInSequence(){
				return thisCurveSequence;
			}
			public IWaypointCurve GetNextCurve(IWaypointCurve currentCurve){
				if(!thisCurveSequence.Contains(currentCurve))
					throw new System.InvalidOperationException(
						"currentCurve is not in the sequence!"
					);
				int currentCurveIndexInSequence = thisCurveSequence.IndexOf(currentCurve);
				if(currentCurveIndexInSequence == thisCurveSequence.Count - 1)
					// throw new System.InvalidOperationException(
					// 	"currentCurve is the last in the sequence, there's no next one"
					// );
					return null;
				return thisCurveSequence[currentCurveIndexInSequence + 1];
			}
			List<IWaypointCurve> CreateSequenceOfWaypointCurves(){
				int[] indexes = new int[thisCurveCountInSequence];
				List<int> used = new List<int>();
				for(int i = 0; i < thisCurveCountInSequence; i ++){
					int index = GetRandomInt(
						thisCurveCountInSequence,
						used
					);
					used.Add(index);
					indexes[i] = index;
				}
				return GetWaypointCurvesAtIndexes(indexes);
			}
			int GetRandomInt(
				int max,
				List<int> usedInt
			){
				int nonUsedIndexCount  = max - usedInt.Count + 1;
				if(nonUsedIndexCount == 0)
					throw new System.InvalidOperationException(
						"there's no more unused index in source"
					);
				List<int> nonUsedIndexes = new List<int>();
				for(int i = 0; i < max; i ++){
					if(!usedInt.Contains(i))
						nonUsedIndexes.Add(i);
				}
				int randomIndex = Random.Range(0, nonUsedIndexes.Count);
				return nonUsedIndexes[randomIndex];
			}
			List<IWaypointCurve> GetWaypointCurvesAtIndexes(int[] indexes){
				List<IWaypointCurve> result = new List<IWaypointCurve>();
				foreach(int index in indexes)
					result.Add(
						thisWaypointCurves[index]
					);
				return result;
			}
		/*  */
		void ConnectWaypointCurveSequence(){
			IWaypointCurve prevWaypointCurve = null;
			foreach(IWaypointCurve curve in thisCurveSequence){
				if(prevWaypointCurve == null)
					ConnectCurveToCurvesOrigin(curve);
				else
					curve.Connect(prevWaypointCurve);
				prevWaypointCurve = curve;
			}
		}

		void ConnectCurveToCurvesOrigin(IWaypointCurve curve){
			curve.SetPosition(thisInitialCurvePosition);
			curve.SetRotation(thisInitialCurveRotation);
		}
		public IWaypointCurve GetNextWaypointCurve(IWaypointCurve curve){
			int indexOfCurve = thisCurveSequence.IndexOf(curve);
			if(indexOfCurve != thisCurveSequence.Count -1){
				return thisCurveSequence[indexOfCurve + 1];
			}
				return null;
		}

		public void CycleCurve(){
			/*  move first group to reserve 
				get random one from reserve and place it
			*/
			if(ShouldCycle()){
				IWaypointCurve lastWaypointCurve = thisCurveSequence[thisCurveSequence.Count - 1];
				RemoveFirstWaypointCurveToReserve();
				IWaypointCurve newCurveToAdd = GetNewWaypointCurveToAddToSequence();
				AddCurveToSequence(newCurveToAdd);
				newCurveToAdd.Connect(lastWaypointCurve);
				float speed = thisFollower.GetFollowSpeed();

			}
		}
		bool thisCycleHasStarted = false;
		int thisIndexToStartCycle = 1;
		bool ShouldCycle(){
			if(thisCycleHasStarted){
				return true;
			}else{
				IWaypointCurve currentCurve = thisFollower.GetCurrentWaypointCurve();
				int indexOfCurCurve = thisCurveSequence.IndexOf(currentCurve);
				if(indexOfCurCurve == thisIndexToStartCycle){
					thisCycleHasStarted = true;
					return true;
				}
			}
				return false;
		}
		void RemoveFirstWaypointCurveToReserve(){
			IWaypointCurve firstCurveInSequence = thisCurveSequence[0];
			firstCurveInSequence.SetPosition(thisReservePosition);
			List<IWaypointCurve> reducedSequence = new List<IWaypointCurve>(thisCurveSequence);
			reducedSequence.Remove(firstCurveInSequence);
			thisCurveSequence = reducedSequence;
		}
		IWaypointCurve GetNewWaypointCurveToAddToSequence(){
			List<IWaypointCurve> curvesInReserve = new List<IWaypointCurve>();
			foreach(IWaypointCurve curve in thisWaypointCurves)
				if(!thisCurveSequence.Contains(curve))
					curvesInReserve.Add(curve);
			int randomIndex = Random.Range(0, curvesInReserve.Count);
			return curvesInReserve[randomIndex];
		}
		void AddCurveToSequence(
			IWaypointCurve newCurveToAdd
		){
			thisCurveSequence.Add(newCurveToAdd);
		}
	}
	public interface IWaypointsManagerConstArg{
		Transform curveReserveTransform{get;}
		int curvesCountInSequence{get;}
		Vector3 initialCurvePosition{get;}
		Quaternion initialCurveRotation{get;}
	}

	public struct WaypointsManagerConstArg: IWaypointsManagerConstArg{
		public WaypointsManagerConstArg(
			Transform curveReserveTransform,
			int curvesCountInSequence,
			Vector3 initialPosition,
			Quaternion initialRotation

		){
			thisCurveReserveTransform = curveReserveTransform;
			thisCurvesCountInSequence = curvesCountInSequence;
			thisInitialCurvePosition = initialPosition;
			thisInitialCurveRotation = initialRotation;
		}
		readonly Transform thisCurveReserveTransform;
		public Transform curveReserveTransform{
			get{return thisCurveReserveTransform;}
		}
		readonly int thisCurvesCountInSequence;
		public int curvesCountInSequence{get{return thisCurvesCountInSequence;}}
		readonly Vector3 thisInitialCurvePosition;
		public Vector3 initialCurvePosition{
			get{return thisInitialCurvePosition;}
		}
		readonly Quaternion thisInitialCurveRotation;
		public Quaternion initialCurveRotation{
			get{return thisInitialCurveRotation;}
		}
	}
}
