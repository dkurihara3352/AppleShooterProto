using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IFlyingTarget: ITestShootingTarget{
		void SetWaypoints(IFlyingTargetWaypoint[] waypoints);
		void SetSmoothLooker(ISmoothLooker looker);
		void SetUpWaypoints();
		IFlyingTargetWaypoint GetCurrentWaypoint();
		void SetUpNextWaypoint();

		int[] GetWaypointsInSequenceIndices();
		int[] GetWaypointsNotInUseIndices();
		float GetCurrentDist();
		Vector3 GetForwardDirection();
	}
	public class FlyingTarget : TestShootingTarget, IFlyingTarget {

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

		public override void ActivateImple(){
			base.ActivateImple();
			StartFlight();
		}
		public override void DeactivateImple(){
			base.DeactivateImple();
			ResetFlight();
		}


		readonly float thisSpeed;
		void StartFlight(){
			this.ResetFlight();
			thisFlightProcess = thisProcessFactory.CreateFlyingTargetFlightProcess(
				this,
				thisInitialVelocity,
				thisDistThreshold,
				thisSpeed
			);
			thisFlightProcess.Run();
			thisSmoothLooker.SetLookAtTarget(GetCurrentWaypoint().GetAdaptor());
			thisSmoothLooker.StartSmoothLook();
		}
		void ResetFlight(){
			StopFlight();
			ResetTransformAtReserve();
		}
		protected override void DestroyTarget(){
			base.DestroyTarget();
			StopFlight();
		}
		void StopFlight(){
			if(thisFlightProcess != null){
				thisFlightProcess.Stop();
				thisFlightProcess = null;
			}
		}
		IFlyingTargetWaypoint[] thisAllWaypoints;
		public void SetWaypoints(IFlyingTargetWaypoint[] waypoints){
			thisAllWaypoints  = waypoints;
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
		/*  */
		public new interface IConstArg: TestShootingTarget.IConstArg{
			Vector3 initialVelocity{get;}
			float distThreshold{get;}
			int waypointsCountInSequence{get;}
			float speed{get;}
		}
		public new struct ConstArg: IConstArg{
			public ConstArg(
				float health,
				IFlyingTargetAdaptor adaptor,
				Color defaultColor,
				IAppleShooterProcessFactory processFactory,
				float fadeTime,
				Vector3 initialVelocity,
				float distThreshold,
				int waypointsCountInSequence,
				float speed
			){	
				thisHealth = health;
				thisAdaptor  = adaptor;
				thisDefaultColor = defaultColor;
				thisProcessFactory = processFactory;
				thisFadeTime = fadeTime;
				thisInitialVelocity = initialVelocity;
				thisDistThreshold = distThreshold;
				thisWaypointsCountInSequence = waypointsCountInSequence;
				thisSpeed = speed;
			}
			readonly float thisHealth;
			public float health{get{return thisHealth;}}
			readonly IFlyingTargetAdaptor thisAdaptor;
			public IShootingTargetAdaptor adaptor{get{return thisAdaptor;}}
			readonly Color thisDefaultColor;
			public Color defaultColor{get{return thisDefaultColor;}}
			readonly IAppleShooterProcessFactory thisProcessFactory;
			public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
			readonly float thisFadeTime;
			public float fadeTime{get{return thisFadeTime;}}
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
