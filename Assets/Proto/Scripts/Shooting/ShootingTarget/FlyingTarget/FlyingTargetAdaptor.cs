using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IFlyingTargetAdaptor: IShootingTargetAdaptor{
		IFlyingTarget GetFlyingTarget();
		void SetFlyingTargetReserve(IFlyingTargetReserve reserve);

		void SetDistanceThresholdForGizmo(float thresh);
	}

	public class FlyingTargetAdaptor : AbsShootingTargetAdaptor, IFlyingTargetAdaptor {
		public Vector3 initialVelocity;
		public float distanceThreshold;
		public int waypointsCountInSequence;
		public float speed = 3f;
		protected override IShootingTarget CreateShootingTarget(){
			FlyingTarget.IConstArg arg = new FlyingTarget.ConstArg(
				thisIndex,
				thisHealth,
				thisDefaultColor,
				this,
				initialVelocity,
				distanceThreshold,
				waypointsCountInSequence,
				speed
			);
			return new FlyingTarget(arg);
		}
		IFlyingTarget thisFlyingTarget{
			get{return (IFlyingTarget)thisShootingTarget;}
		}
		public IFlyingTarget GetFlyingTarget(){
			return thisFlyingTarget;
		}
		ISmoothLookerAdaptor thisSmoothLookerAdaptor;
		ISmoothLookerAdaptor CollectSmoothLookerAdaptor(){
			return (ISmoothLookerAdaptor)this.transform.GetComponent(typeof(ISmoothLookerAdaptor));
		}
		IFlyingTargetReserve thisFlyingTargetReserve;
		public void SetFlyingTargetReserve(IFlyingTargetReserve reserve){
			thisFlyingTargetReserve = reserve;
		}
		public override void SetUp(){
			base.SetUp();
			thisSmoothLookerAdaptor = CollectSmoothLookerAdaptor();
			thisSmoothLookerAdaptor.SetUp();
		}
		public override void SetUpReference(){
			base.SetUpReference();
			ISmoothLooker looker = thisSmoothLookerAdaptor.GetSmoothLooker();
			thisFlyingTarget.SetSmoothLooker(looker);

			thisFlyingTarget.SetFlyingTargetReserve(thisFlyingTargetReserve);
			thisSmoothLookerAdaptor.SetUpReference();
		}
		
		public void SetDistanceThresholdForGizmo(float thresh){
			distanceThresholdForGizmo = thresh;
		}
		float distanceThresholdForGizmo;
		public void OnDrawGizmos(){
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(
				GetPosition(),
				distanceThresholdForGizmo
			);
		}
	}
}
