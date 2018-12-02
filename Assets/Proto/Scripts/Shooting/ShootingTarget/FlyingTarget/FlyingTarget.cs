using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IFlyingTarget: IShootingTarget{
		void SetSmoothLooker(ISmoothLooker looker);
		void SetFlyingTargetReserve(IFlyingTargetReserve reserve);
		void SetUpWaypoints();
		IFlyingTargetWaypoint GetCurrentWaypoint();

		void ActivateAt(IFlyingTargetWaypointManager waypointManager);
		void SetUpNextWaypoint();

		int[] GetWaypointsInSequenceIndices();
		int[] GetWaypointsNotInUseIndices();
		float GetCurrentDist();
		Vector3 GetForwardDirection();

		void SetDistanceThresholdForGizmo(float thresh);
	}
	public class FlyingTarget : AbsShootingTarget, IFlyingTarget {

		public FlyingTarget(
			IConstArg arg
		): base(arg){
			thisInitialVelocity = arg.initialVelocity;
			thisDistThreshold = arg.distThreshold;
			thisWaypointsCountInSequence = arg.waypointsCountInSequence;
			thisSpeed = arg.speed;
		}
		IFlyingTargetFlightProcess thisFlightProcess;
		readonly Vector3 thisInitialVelocity;
		readonly float thisDistThreshold;
		ISmoothLooker thisSmoothLooker;
		public void SetSmoothLooker(ISmoothLooker looker){
			thisSmoothLooker = looker;
		}
		public void ActivateAt(IFlyingTargetWaypointManager waypointManager){
			Deactivate();
			SetWaypointsManager(waypointManager);
			Activate();
		}

		public override void ActivateImple(){
			base.ActivateImple();
			SetParent(thisWaypointManager);
			ResetLocalTransform();
			StartFlight();
		}
		public override void DeactivateImple(){
			base.DeactivateImple();
			StopFlight();
		}
		IFlyingTargetReserve thisFlyingTargetReserve;
		public void SetFlyingTargetReserve(IFlyingTargetReserve reserve){
			thisFlyingTargetReserve = reserve;
		}
		protected override void ReserveSelf(){
			thisFlyingTargetReserve.Reserve(this);
		}

		readonly float thisSpeed;
		void StartFlight(){
			StopFlight();
			thisFlightProcess = thisAppleShooterProcessFactory.CreateFlyingTargetFlightProcess(
				this,
				thisInitialVelocity,
				thisDistThreshold,
				thisSpeed
			);
			thisFlightProcess.Run();
			thisSmoothLooker.SetLookAtTarget(GetCurrentWaypoint().GetAdaptor());
			thisSmoothLooker.StartSmoothLook();
		}
		void StopFlight(){
			if(thisFlightProcess != null){
				thisFlightProcess.Stop();
				thisFlightProcess = null;
			}
			thisSmoothLooker.StopSmoothLook();
		}
		IFlyingTargetWaypoint[] thisAllWaypoints;
		IFlyingTargetWaypointManager thisWaypointManager;
		public void SetWaypointsManager(IFlyingTargetWaypointManager waypointManager){
			thisWaypointManager = waypointManager;
			if(waypointManager != null)
				thisAllWaypoints = waypointManager.GetWaypoints();
		}
		readonly int thisWaypointsCountInSequence;
		IFlyingTargetWaypoint[] thisWaypointsInSequence;
		int[] thisWaypointsIndicesInSequence;
		public int[] GetWaypointsInSequenceIndices(){
			return thisWaypointsIndicesInSequence;
		}
		int[] thisWaypointsIndicesNotInUse;
		public int[] GetWaypointsNotInUseIndices(){
			return thisWaypointsIndicesNotInUse;
		}
		public void SetUpWaypoints(){
			int[] randomIndices = DKUtility.Calculator.GetRandomIntegers(
				thisWaypointsCountInSequence,
				thisAllWaypoints.Length - 1
			);
			thisWaypointsIndicesInSequence = randomIndices;
			thisWaypointsInSequence = CreateWaypointsInSequence(
				thisWaypointsIndicesInSequence
			);
			thisWaypointsIndicesNotInUse = CreateWaypointsIndicesNotInUse(
				thisWaypointsIndicesInSequence
			);
		}
		public IFlyingTargetWaypoint GetCurrentWaypoint(){
			return thisWaypointsInSequence[0];
		}
		public void SetUpNextWaypoint(){
			int randomIndexInNotInUse = Random.Range(
				0, thisWaypointsIndicesNotInUse.Length
			);
			int indexOfNextWPToAdd = thisWaypointsIndicesNotInUse[randomIndexInNotInUse];
			List<int> newIndicesList = new List<int>();
			for(int i = 1; i < thisWaypointsIndicesInSequence.Length; i ++)
				newIndicesList.Add(thisWaypointsIndicesInSequence[i]);
			newIndicesList.Add(indexOfNextWPToAdd);

			thisWaypointsIndicesInSequence = newIndicesList.ToArray();
			thisWaypointsInSequence = CreateWaypointsInSequence(thisWaypointsIndicesInSequence);
			thisWaypointsIndicesNotInUse = CreateWaypointsIndicesNotInUse(thisWaypointsIndicesInSequence);

			thisSmoothLooker.SetLookAtTarget(GetCurrentWaypoint().GetAdaptor());
		}
		IFlyingTargetWaypoint[] CreateWaypointsInSequence(
			int[] indices
		){
			List<IFlyingTargetWaypoint> resultList = new List<IFlyingTargetWaypoint>();
			foreach(int i in indices){
				resultList.Add(thisAllWaypoints[i]);
			}
			return resultList.ToArray();
		}
		int[] CreateWaypointsIndicesNotInUse(
			int[] indicesInUse
		){
			List<int> resultList = new List<int>();
			for(int i = 0; i < thisAllWaypoints.Length; i ++){
				bool found = false;;
				foreach(int j in indicesInUse){
					if(i == j)
						found = true;
				}
				if(!found)
					resultList.Add(i);
			}
			return resultList.ToArray();
		}
		public float GetCurrentDist(){
			return (this.GetCurrentWaypoint().GetPosition() - this.GetPosition()).magnitude;
		}
		public Vector3 GetForwardDirection(){
			return thisAdaptor.GetForwardDirection();
		}
		IFlyingTargetAdaptor thisTypedAdaptor{
			get{return (IFlyingTargetAdaptor)thisAdaptor;}
		}
		public void SetDistanceThresholdForGizmo(float thresh){
			thisTypedAdaptor.SetDistanceThresholdForGizmo(thresh);
		}

		/*  */
		public new interface IConstArg: AbsShootingTarget.IConstArg{
			Vector3 initialVelocity{get;}
			float distThreshold{get;}
			int waypointsCountInSequence{get;}
			float speed{get;}
		}
		public new class ConstArg: AbsShootingTarget.ConstArg, IConstArg{
			public ConstArg(
				int index,
				Color defaultColor,
				UnityBase.IBellCurve healthBellCurve,
				IFlyingTargetAdaptor adaptor,
				ITargetData targetData,

				Vector3 initialVelocity,
				float distThreshold,
				int waypointsCountInSequence,
				float speed
			): base(
				index,
				defaultColor,
				healthBellCurve,
				adaptor,
				targetData
			){	
				thisInitialVelocity = initialVelocity;
				thisDistThreshold = distThreshold;
				thisWaypointsCountInSequence = waypointsCountInSequence;
				thisSpeed = speed;
			}
			readonly Vector3 thisInitialVelocity;
			public Vector3 initialVelocity{get{return thisInitialVelocity;}}
			readonly float thisDistThreshold;
			public float distThreshold{get{return thisDistThreshold;}}
			readonly int thisWaypointsCountInSequence;
			public int waypointsCountInSequence{get{return thisWaypointsCountInSequence;}}
			readonly float thisSpeed;
			public float speed{get{return thisSpeed;}}
		}
	}
}
