using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IArrowFlightProcess: IProcess{}
	public class ArrowFlightProcess : AbsConstrainedProcess, IArrowFlightProcess {

		public ArrowFlightProcess(
			IArrowFlightProcessConstArg arg
		): base(arg){
			thisArrow = arg.arrow;
			thisFlightSpeed = arg.flightSpeed;
			thisFlightDirection = arg.flightDirection;
			thisFlightGravity = arg.flightGravity;
			thisLauncherVelocity = arg.launcherVelocity;
			thisLaunchPosition = arg.launchPosition;
		}
		readonly IArrow thisArrow;
		readonly float thisFlightSpeed;
		readonly Vector3 thisFlightDirection;
		readonly float thisFlightGravity;
		readonly Vector3 thisLauncherVelocity;
		readonly Vector3 thisLaunchPosition;
		protected override void RunImple(){
			thisArrow.BecomeChildToReserve();
			CacheInitialVelocity();
			Debug.Log(
				"flightSpeed: " + thisFlightSpeed.ToString() + ", "+
				"launcherVelocity: "+ thisLauncherVelocity.ToString()
			);
		}
		void CacheInitialVelocity(){
			thisInitialVelocity = thisFlightDirection * thisFlightSpeed;
			thisInitialVelocity += thisLauncherVelocity;
		}
		Vector3 thisInitialVelocity;
		protected override void UpdateProcessImple(float deltaT){
			Vector3 newPosition = new Vector3();
			newPosition.x = thisInitialVelocity.x * thisElapsedTime;
			newPosition.z = thisInitialVelocity.z * thisElapsedTime;
			newPosition.y = thisInitialVelocity.y * thisElapsedTime - (thisFlightGravity * thisElapsedTime * thisElapsedTime);

			newPosition += thisLaunchPosition;
			thisArrow.SetPosition(newPosition);
		}
		protected override void ExpireImple(){
			thisArrow.TryResetArrow();
		}
		public override int GetProcessOrder(){
			return 200;
		}
	}



	public interface IArrowFlightProcessConstArg: IConstrainedProcessConstArg{
		IArrow arrow{get;}
		float flightSpeed{get;}
		Vector3 flightDirection{get;}
		float flightGravity{get;}
		Vector3 launcherVelocity{get;}
		Vector3 launchPosition{get;}
	}
	public class ArrowFlightProcessConstArg: ConstrainedProcessConstArg, IArrowFlightProcessConstArg{
		public ArrowFlightProcessConstArg(
			IArrow arrow,
			float flightSpeed,
			Vector3 flightDirection,
			float flightGravity,
			Vector3 launcherVelocity,
			Vector3 launchPosition,

			IProcessManager processManager
		):base(
			processManager,
			ProcessConstraint.ExpireTime,
			processManager.GetArrowFlightExpireTime()
		){
			thisArrow = arrow;
			thisFlightSpeed = flightSpeed;
			thisFlightDirection = flightDirection;
			thisFlightGravity = flightGravity;
			thisLauncherVelocity = launcherVelocity;
			thisLaunchPosition = launchPosition;
		}
		readonly IArrow thisArrow;
		public IArrow arrow{get{return thisArrow;}}
		readonly float thisFlightSpeed;
		public float flightSpeed{get{return thisFlightSpeed;}}
		readonly Vector3 thisFlightDirection;
		public Vector3 flightDirection{get{return thisFlightDirection;}}
		readonly float thisFlightGravity;
		public float flightGravity{get{return thisFlightGravity;}}
		readonly Vector3 thisLauncherVelocity;
		public Vector3 launcherVelocity{get{return thisLauncherVelocity;}}
		readonly Vector3 thisLaunchPosition;
		public Vector3 launchPosition{get{return thisLaunchPosition;}}
	}
}
