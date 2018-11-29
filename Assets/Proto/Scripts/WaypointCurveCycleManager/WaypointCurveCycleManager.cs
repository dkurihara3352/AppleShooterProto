using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointCurveCycleManager: IAppleShooterSceneObject{

		void SetWaypointsFollower(IWaypointsFollower follower);
		void CheckAndClearWaypointsFollower(IWaypointsFollower follower);
		void SetWaypointCurves(List<IWaypointCurve> curves);

		List<IWaypointCurve> GetWaypointCurvesInSequence();

		void PlaceWaypointCurves();
		void CycleCurve();
		int GetWaypointCurveIndex(IWaypointCurve curve);
		IWaypointsFollower GetFollower();

		IWaypointCurve GetCurrentCurve();
		IWaypointCurve GetNextCurve(IWaypointCurve currentCurve);

		List<IWaypointCurve> GetAllWaypointCurves();

		/* Debug */
		int[] GetReservedCurvesIDs();
		int GetCurrentCurveIDInSequence();
	}

	public class WaypointCurveCycleManager : AppleShooterSceneObject, IWaypointCurveCycleManager{
		/* SetUp */
			public WaypointCurveCycleManager(
				IConstArg arg
			): base(
				arg
			){
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
			public void CheckAndClearWaypointsFollower(IWaypointsFollower follower){
				if(thisFollower != null)
					if(thisFollower == follower)
						thisFollower = null;
			}
			public float GetSpeed(){return thisFollower.GetFollowSpeed();}
			List<IWaypointCurve> thisWaypointCurves;
			public List<IWaypointCurve> GetAllWaypointCurves(){
				return thisWaypointCurves;
			}
			public void SetWaypointCurves(List<IWaypointCurve> curves){
				thisWaypointCurves = curves;
			}
			public int GetWaypointCurveIndex(IWaypointCurve curve){
				return thisWaypointCurves.IndexOf(curve);
			}
		/*  */
			public void PlaceWaypointCurves(){
				PlaceAllWaypointCurvesAtReserve();
				thisCurveSequence = CreateSequenceOfWaypointCurves();
				// PrintSequence();
				// PrintReserved();
				ConnectWaypointCurveSequence();

				if(thisFollower != null)				
					thisFollower.SetWaypointCurve(thisCurveSequence[0]);
			}
			void PrintSequence(){
				string result = "curves in sequence: ";
				foreach(IWaypointCurve curve in thisCurveSequence){
					result += curve.GetIndex().ToString();
				}
				// DKUtility.DebugHelper.PrintInGreen(result);
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
				curve.OnReserve();
			}
			void UnreserveCurve(IWaypointCurve curve){
				thisReservedCurves.Remove(curve);
				curve.OnUnreserve();
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
			public IWaypointCurve GetNextCurve(){
				IWaypointCurve currentCurve  = GetCurrentCurve();
				return this.GetNextCurve(currentCurve);
			}
			List<IWaypointCurve> CreateSequenceOfWaypointCurves(){
				// Debug.Log(
				// 	"thisCurveCountInSequence: " + thisCurveCountInSequence.ToString() + ", " +
				// 	"allCurveIDs" + DKUtility.DebugHelper.GetIndicesString(GetAllCurveIDs())// nothing in the array!
				// );
				int[] indexes = DKUtility.Calculator.GetRandomIntegers(
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

			int[] GetAllCurveIDs(){
				int totalCount = thisWaypointCurves.Count;
				int[] result = new int[totalCount];
				for(int i = 0; i < totalCount; i++){
					result[i] = i;
				}
				return result;
			}
		/* Cycling */
			void ConnectWaypointCurveSequence(){
				IWaypointCurve prevWaypointCurve = null;
				foreach(IWaypointCurve curve in thisCurveSequence){
					if(prevWaypointCurve == null)
						ConnectCurveToInitialTransform(curve);
					else
						curve.Connect(prevWaypointCurve);
					prevWaypointCurve = curve;
				}
			}

			void ConnectCurveToInitialTransform(IWaypointCurve curve){
				curve.SetLocalPosition(thisInitialCurvePosition);
				curve.SetLocalRotation(thisInitialCurveRotation);
				curve.CalculateCurve();
			}

			public void CycleCurve(){
				/*  move first group to reserve 
					get random one from reserve and place it
				*/
				if(ShouldCycle()){
					IWaypointCurve lastWaypointCurve = thisCurveSequence[thisCurveSequence.Count - 1];
					IWaypointCurve newCurveToAdd = GetNewWaypointCurveToAddToSequence();
					RemoveFirstWaypointCurveToReserve();
					newCurveToAdd.Connect(lastWaypointCurve);
					AddCurveToSequence(newCurveToAdd);
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
				int randomIndex = Random.Range(0, thisReservedCurves.Count);
				return thisReservedCurves[randomIndex];
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
			public IWaypointCurve GetCurrentCurve(){
				return thisFollower.GetCurrentWaypointCurve();
			}
			public int GetCurrentCurveIDInSequence(){
				IWaypointCurve currentCurve = GetCurrentCurve();
				return thisCurveSequence.IndexOf(currentCurve);
			}
		/* Const */
			public new interface IConstArg: AppleShooterSceneObject.IConstArg{
				Transform curveReserveTransform{get;}
				int curvesCountInSequence{get;}
				Vector3 initialCurvePosition{get;}
				Quaternion initialCurveRotation{get;}
				int cycleStartIndex{get;}
			}
			public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
				public ConstArg(
					IWaypointCurveCycleManagerAdaptor adaptor,
					Transform curveReserveTransform,
					int curvesCountInSequence,
					Vector3 initialPosition,
					Quaternion initialRotation,
					int cycleStartIndex

				): base(
					adaptor
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
		/*  */

	}

}
