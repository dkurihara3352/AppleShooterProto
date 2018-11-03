using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IStartFlyingTargetFlightWaypointEventAdaptor: IWaypointEventAdaptor{}
	public class StartFlyingTargetFlightWaypointEventAdaptor: MonoBehaviourAdaptor, IStartFlyingTargetFlightWaypointEventAdaptor{
		IStartFlyingTargetFlightWaypointEvent thisEvent;
		public IWaypointEvent GetWaypointEvent(){
			return thisEvent;
		}
		public float eventPoint;
		public override void SetUp(){
			thisEvent = CreateWaypointEvent();
			thisWaypointAdaptors = CollectWaypointAdaptors();
		}
		IStartFlyingTargetFlightWaypointEvent CreateWaypointEvent(){
			AbsWaypointEvent.IConstArg arg = new AbsWaypointEvent.ConstArg(
				eventPoint
			);
			return new StartFlyingTargetFlightWaypointEvent(arg);
		}
		public FlyingTargetReserveAdaptor flyingTargetReserveAdaptor;
		public Transform waypointsParent;
		IFlyingTargetWaypointAdaptor[] thisWaypointAdaptors;
		IFlyingTargetWaypointAdaptor[] CollectWaypointAdaptors(){
			Component[] childComps = waypointsParent.GetComponentsInChildren<Component>();
			List<IFlyingTargetWaypointAdaptor> resultList = new List<IFlyingTargetWaypointAdaptor>();
			foreach(Component comp in childComps){
				if(comp is IFlyingTargetWaypointAdaptor)
					resultList.Add((IFlyingTargetWaypointAdaptor)comp);
			}
			return resultList.ToArray();
		}
		public FlyingTargetWaypointManagerAdaptor flyingTargetWaypointManagerAdaptor;
		public override void SetUpReference(){
			IFlyingTargetWaypointManager manager = flyingTargetWaypointManagerAdaptor.GetFlyingTargetWaypointManager();
			thisEvent.SetFlyingTargetWaypointManager(manager);
			IFlyingTargetReserve reserve  = flyingTargetReserveAdaptor.GetFlyingTargetReserve();
			thisEvent.SetFlyingTargetReserve(reserve);
		}

		IFlyingTargetWaypoint[] GetWaypoints(){
			List<IFlyingTargetWaypoint> resultList = new List<IFlyingTargetWaypoint>();
			foreach(IFlyingTargetWaypointAdaptor adaptor in thisWaypointAdaptors){
				resultList.Add(adaptor.GetWaypoint());
			}
			return resultList.ToArray();
		}
	}
}

