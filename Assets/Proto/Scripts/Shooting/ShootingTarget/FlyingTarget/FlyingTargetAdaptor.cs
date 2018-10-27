using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IFlyingTargetAdaptor: ITestShootingTargetAdaptor{
		void ResetTransformAtStandBy();
	}

	public class FlyingTargetAdaptor : TestShootingTargetAdaptor, IFlyingTargetAdaptor {
		public Vector3 initialVelocity;
		public float distanceThreshold;
		public int waypointsCountInSequence;
		public float speed = 3f;
		public Transform flyingTargetStandByTransform;
		protected override void SetUpTarget(){
			FlyingTarget.IConstArg arg = new FlyingTarget.ConstArg(
				health,
				this,
				defaultColor,
				processFactory,
				fadeTime,
				initialVelocity,
				distanceThreshold,
				waypointsCountInSequence,
				speed
			);
			thisShootingTarget = new FlyingTarget(arg);
		}
		IFlyingTarget thisFlyingTarget{
			get{return (IFlyingTarget)thisShootingTarget;}
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IFlyingTargetWaypoint[] waypoints = CollectWaypoints();
			thisFlyingTarget.SetWaypoints(waypoints);

			ISmoothLookerAdaptor lookerAdaptor = this.GetComponent(typeof(ISmoothLookerAdaptor)) as ISmoothLookerAdaptor;
			ISmoothLooker looker = lookerAdaptor.GetSmoothLooker();
			thisFlyingTarget.SetSmoothLooker(looker);
		}
		public Transform waypointsParent;
		IFlyingTargetWaypoint[] CollectWaypoints(){
			int childCount = waypointsParent.childCount;
			List<IFlyingTargetWaypoint> resultList = new List<IFlyingTargetWaypoint>();
			for(int i = 0 ; i< childCount; i ++){
				Transform child = waypointsParent.GetChild(i);
				IFlyingTargetWaypointAdaptor adaptor = (IFlyingTargetWaypointAdaptor)child.GetComponent(typeof(IFlyingTargetWaypointAdaptor));
				IFlyingTargetWaypoint waypoint = adaptor.GetWaypoint();
				resultList.Add(waypoint);
			}
			return resultList.ToArray();
		}
		public void ResetTransformAtStandBy(){
			Vector3 standByPosition = flyingTargetStandByTransform.position;
			Quaternion standByRotation = flyingTargetStandByTransform.rotation;
			SetPosition(standByPosition);
			SetRotation(standByRotation);
		}
		public void OnDrawGizmos(){
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(
				GetPosition(),
				distanceThreshold
			);
		}
	}
}
