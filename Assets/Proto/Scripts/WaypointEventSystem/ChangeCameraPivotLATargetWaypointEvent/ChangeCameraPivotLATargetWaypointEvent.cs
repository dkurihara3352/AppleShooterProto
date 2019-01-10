﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace SlickBowShooting{
	public interface IChangeCameraPivotLATargetWaypointEvent: IWaypointEvent{
		void SetSmoothLooker(ISmoothLooker looker);
	}
	public class ChangeCameraPivotLATargetWaypointEvent: AbsWaypointEvent, IChangeCameraPivotLATargetWaypointEvent{
		public ChangeCameraPivotLATargetWaypointEvent(
			IConstArg arg
		): base(arg){
			thisTargetMBAdaptor = arg.targetMBAdaptor;
			thisSmoothCoefficient = arg.smoothCoefficient;
		}
		readonly IMonoBehaviourAdaptor thisTargetMBAdaptor;
		readonly float thisSmoothCoefficient;
		ISmoothLooker thisCameraPivotSmoothLooker;
		public void SetSmoothLooker(ISmoothLooker looker){
			thisCameraPivotSmoothLooker = looker;
		}
		protected override void ExecuteImple(IWaypointsFollower follower){
			thisCameraPivotSmoothLooker.SetSmoothCoefficient(thisSmoothCoefficient);
			thisCameraPivotSmoothLooker.SetLookAtTarget(thisTargetMBAdaptor);
		}
		public override string GetName(){
			return "ChangeCameraPivotLATargetWPEvent";
		}
		/* Const */
			public new interface IConstArg: AbsWaypointEvent.IConstArg{
				IMonoBehaviourAdaptor targetMBAdaptor{get;}
				float smoothCoefficient{get;}
			}
			public new struct ConstArg: IConstArg{
				public ConstArg(
					float eventPoint,
					IMonoBehaviourAdaptor targetMBAdaptor,
					float smoothCoefficient
				){
					thisEventPoint = eventPoint;
					thisLookAtTarget = targetMBAdaptor;
					thisSmoothCoefficient = smoothCoefficient;
				}
				readonly float thisEventPoint;
				public float eventPoint{get{return thisEventPoint;}}
				readonly IMonoBehaviourAdaptor thisLookAtTarget;
				public IMonoBehaviourAdaptor targetMBAdaptor{get{return thisLookAtTarget;}}
				readonly float thisSmoothCoefficient;
				public float smoothCoefficient{get{return thisSmoothCoefficient;}}
			}
		/*  */
	}


}
