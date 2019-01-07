using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IGlidingTarget: IShootingTarget{
		void SetWaypointsFollower(IWaypointsFollower follower);
		void SetGlidingTargetReserve(IGlidingTargetReserve reserve);
		IGlidingTargetWaypointCurve GetGlidingTargetWaypointCurve();
		void ActivateAt(IGlidingTargetSpawnPoint point);
	}
	public class GlidingTarget: AbsShootingTarget, IGlidingTarget{
		public GlidingTarget(
			IConstArg arg
		): base(arg){}

		public void ActivateAt(IGlidingTargetSpawnPoint point){
			Deactivate();
			SetSpawnPoint(point);
			Activate();
		}
		IGlidingTargetSpawnPoint thisSpawnPoint;
		void SetSpawnPoint(IGlidingTargetSpawnPoint point){
			thisSpawnPoint = point;
			point.SetTarget(this);
			IGlidingTargetWaypointCurve curve = point.GetGlidingTargetWaypointCurve();
			SetWaypointCurve(curve);
		}
		public override void DeactivateImple(){
			base.DeactivateImple();
			ClearSpawnPoint();
			StopGlide();
			thisWaypointsFollower.ResetFollower();
			// Debug.Log(DKUtility.DebugHelper.StringInColor(GetName() + " is deac'd", Color.red));
		}
		void ClearSpawnPoint(){
			if(thisSpawnPoint != null)
				thisSpawnPoint.CheckAndClearTarget(this);
			thisSpawnPoint = null;
			thisWaypointCurve = null;
			thisWaypointsFollower.SetWaypointCurve(null);
		}
		public override void ActivateImple(){
			// Debug.Log(
			// 	DKUtility.DebugHelper.StringInColor(GetName() + "'s ActivateImple is called", Color.green)
			// );
			base.ActivateImple();
			StartGlide();
		}
		IWaypointsFollower thisWaypointsFollower;
		public void SetWaypointsFollower(IWaypointsFollower follower){
			thisWaypointsFollower = follower;
		}
		void StartGlide(){
			StopGlide();
			thisWaypointsFollower.StartFollowing();
		}
		void StopGlide(){
			thisWaypointsFollower.StopFollowing();
		}
		
		IGlidingTargetReserve thisGlidingTargetReserve;
		public void SetGlidingTargetReserve(IGlidingTargetReserve reserve){
			thisGlidingTargetReserve = reserve;
		}
		protected override void ReserveSelf(){
			thisGlidingTargetReserve.Reserve(this);
		}
		IGlidingTargetWaypointCurve thisWaypointCurve;
		public void SetWaypointCurve(IGlidingTargetWaypointCurve curve){
			thisWaypointCurve = curve;
			thisWaypointsFollower.SetWaypointCurve(curve);
		}
		public IGlidingTargetWaypointCurve GetGlidingTargetWaypointCurve(){
			return thisWaypointCurve;
		}

		public new interface IConstArg: AbsShootingTarget.IConstArg{}
		public new class ConstArg: AbsShootingTarget.ConstArg, IConstArg{
			public ConstArg(
				int index,
				// Color defaultColor,
				UnityBase.IBellCurve healthBellCurve,
				IGlidingTargetAdaptor adaptor,
				ITargetData targetData
			): base(
				index,
				// defaultColor,
				healthBellCurve,
				adaptor,
				targetData
			){}
		}
	}
}

