using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointsManager{

		void SetWaypointsFollower(IWaypointsFollower follower);
		void SetWaypointCurves(List<IWaypointCurve> curves);

		List<IWaypointCurve> GetWaypointCurvesInSequence();

		void PlaceWaypointCurves();
		void CycleCurve();
		int GetWaypointCurveIndex(IWaypointCurve curve);
		IWaypointsFollower GetFollower();

		IWaypointCurve GetNextCurve(IWaypointCurve currentCurve);

		/* Debug */
		int[] GetReservedCurvesIDs();
		int GetCurrentCurveIDInSequence();
	}

	public class WaypointsManager : IWaypointsManager {
		public WaypointsManager(IWaypointsManagerConstArg arg){
			thisCurveReservePointTransform = arg.curveReserveTransform;
			thisCurveCountInSequence = arg.curvesCountInSequence;
			thisInitialCurvePosition = arg.initialCurvePosition;
			thisInitialCurveRotation = arg.initialCurveRotation;
			thisIndexToStartCycle = arg.cycleStartIndex;
		}
		readonly Transform thisCurveReservePointTransform;
		Vector3 thisReservePosition{
			get{return thisCurveReservePointTransform.position;}
		}
		readonly Vector3 thisInitialCurvePosition;
		readonly Quaternion thisInitialCurveRotation;
		readonly int thisCurveCountInSequence;
		readonly int thisIndexToStartCycle;

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
				ReserveCurve(curve);
			}
		}
		void PrintReserved(){
			string result = "";
			foreach(int id in GetReservedCurvesIDs()){
				result += id.ToString() + ", ";
			}
			Debug.Log(
				"Reserved ids: " + 
				result
			);
		}
		List<IWaypointCurve> thisReservedCurves = new List<IWaypointCurve>();
		void ReserveCurve(IWaypointCurve curve){
			curve.SetPosition(thisReservePosition);
			thisReservedCurves.Add(curve);
		}
		void UnreserveCurve(IWaypointCurve curve){
			thisReservedCurves.Remove(curve);
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
					return null;
				int newCurveIndexInSequence = currentCurveIndexInSequence + 1;
				IWaypointCurve result = thisCurveSequence[newCurveIndexInSequence];

				return result;
			}
			List<IWaypointCurve> CreateSequenceOfWaypointCurves(){
				int[] indexes = GetRandomIntegers(
					thisCurveCountInSequence,
					GetAllCurveIDs()
				);
				List<IWaypointCurve> result = GetWaypointCurvesAtIndexes(indexes);
				foreach(IWaypointCurve curve in result){
					UnreserveCurve(curve);
				}
				return result;
			}
			List<IWaypointCurve> GetWaypointCurvesAtIndexes(int[] indexes){
				List<IWaypointCurve> result = new List<IWaypointCurve>();
				foreach(int index in indexes)
					result.Add(
						thisWaypointCurves[index]
					);
				return result;
			}
			int[] GetRandomIntegers(int count, int[] pool){
				int[] result = new int[count];
				int counter = 0;
				int[] unused = pool;
				while(counter < count){
					int randomInt = GetRandomInt(
						ref unused
					);
					result[counter] = randomInt;
					counter ++;
				}
				return result;
			}
			int GetRandomInt(
				ref int[] unused
			){
				int randomIndex = Random.Range(0, unused.Length);
				int result = unused[randomIndex];
				int[] newUnused = CreateNewUnused(unused, result);
				unused = newUnused;
				return result;
			}
			int[] CreateNewUnused(
				int[] unused,
				int used
			){
				List<int> result = new List<int>();
				foreach(int i in unused){
					if(i != used){
						result.Add(i);
					}
				}
				return result.ToArray();
			}
			int[] GetAllCurveIDs(){
				int totalCount = thisWaypointCurves.Count;
				int[] result = new int[totalCount];
				for(int i = 0; i < totalCount; i++){
					result[i] = i;
				}
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
				// float speed = thisFollower.GetFollowSpeed();
			}
		}
		bool thisCycleHasStarted = false;
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
			ReserveCurve(firstCurveInSequence);
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
			UnreserveCurve(newCurveToAdd);
		}
		public int[] GetReservedCurvesIDs(){
			List<int> result = new List<int>();
			foreach(IWaypointCurve curve in thisReservedCurves){
				result.Add(curve.GetIndex());
			}
			return result.ToArray();
		}
		public int GetCurrentCurveIDInSequence(){
			IWaypointCurve currentCurve = thisFollower.GetCurrentWaypointCurve();
			return thisCurveSequence.IndexOf(currentCurve);
		}
	}
	public interface IWaypointsManagerConstArg{
		Transform curveReserveTransform{get;}
		int curvesCountInSequence{get;}
		Vector3 initialCurvePosition{get;}
		Quaternion initialCurveRotation{get;}
		int cycleStartIndex{get;}
	}

	public struct WaypointsManagerConstArg: IWaypointsManagerConstArg{
		public WaypointsManagerConstArg(
			Transform curveReserveTransform,
			int curvesCountInSequence,
			Vector3 initialPosition,
			Quaternion initialRotation,
			int cycleStartIndex

		){
			thisCurveReserveTransform = curveReserveTransform;
			thisCurvesCountInSequence = curvesCountInSequence;
			thisInitialCurvePosition = initialPosition;
			thisInitialCurveRotation = initialRotation;
			thisCycleStartIndex = cycleStartIndex;
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
		readonly int thisCycleStartIndex;
		public int cycleStartIndex{get{return thisCycleStartIndex;}}
	}
}
