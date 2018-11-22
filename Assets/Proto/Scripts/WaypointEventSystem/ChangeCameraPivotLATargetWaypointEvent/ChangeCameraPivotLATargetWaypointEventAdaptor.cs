using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace AppleShooterProto{
	public interface IChangeCameraPivotLATargetWaypointEventAdaptor: IWaypointEventAdaptor{}
	public class ChangeCameraPivotLATargetWaypointEventAdaptor: AppleShooterMonoBehaviourAdaptor, IChangeCameraPivotLATargetWaypointEventAdaptor{
		public override void SetUp(){
			ChangeCameraPivotLATargetWaypointEvent.IConstArg arg = new ChangeCameraPivotLATargetWaypointEvent.ConstArg(
				eventPoint,
				targetMBAdaptor,
				smoothCoefficient
			);
			thisWaypointEvent = new ChangeCameraPivotLATargetWaypointEvent(arg);
		}
		public float eventPoint;
		public MonoBehaviourAdaptor targetMBAdaptor;
		public float smoothCoefficient;
		public SmoothLookerAdaptor cameraPivotSmoothLookerAdaptor;
		public override void SetUpReference(){
			ISmoothLooker looker = cameraPivotSmoothLookerAdaptor.GetSmoothLooker();
			thisWaypointEvent.SetSmoothLooker(looker);
		}
		public IWaypointEvent GetWaypointEvent(){
			return thisWaypointEvent;
		}
		IChangeCameraPivotLATargetWaypointEvent thisWaypointEvent;
	}
}

