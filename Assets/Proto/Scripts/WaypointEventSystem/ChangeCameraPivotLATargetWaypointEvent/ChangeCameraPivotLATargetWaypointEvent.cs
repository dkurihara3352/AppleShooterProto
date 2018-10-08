﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IChangeCameraPivotLATargetWaypointEvent: IWaypointEvent{
		void SetSmoothLooker(ISmoothLooker looker);
	}
	public class ChangeCameraPivotLATargetWaypointEvent: AbsWaypointEvent, IChangeCameraPivotLATargetWaypointEvent{
		public ChangeCameraPivotLATargetWaypointEvent(
			IChangeCameraPivotLATargetWaypointEventConstArg arg
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
		public override void Execute(){
			thisCameraPivotSmoothLooker.SetSmoothCoefficient(thisSmoothCoefficient);
			thisCameraPivotSmoothLooker.SetLookAtTarget(thisTargetMBAdaptor);
		}
	}


	public interface IChangeCameraPivotLATargetWaypointEventConstArg: IWaypointEventConstArg{
		IMonoBehaviourAdaptor targetMBAdaptor{get;}
		float smoothCoefficient{get;}
	}
	public struct ChangeCameraPivotLATargetWaypointEventConstArg: IChangeCameraPivotLATargetWaypointEventConstArg{
		public ChangeCameraPivotLATargetWaypointEventConstArg(
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
}