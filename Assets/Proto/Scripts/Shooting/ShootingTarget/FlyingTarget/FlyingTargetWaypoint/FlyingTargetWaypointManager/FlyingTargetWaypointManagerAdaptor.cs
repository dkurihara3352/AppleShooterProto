using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IFlyingTargetWaypointManagerAdaptor: IMonoBehaviourAdaptor{
		IFlyingTargetWaypointManager GetFlyingTargetWaypointManager();
	}
	public class FlyingTargetWaypointManagerAdaptor: MonoBehaviourAdaptor, IFlyingTargetWaypointManagerAdaptor{
		public override void SetUp(){
			thisManager = CreateFlyingTargetWaypointManager();
			thisWaypointAdaptors = CollectWaypointAdaptors();
		}
		IFlyingTargetWaypointManager thisManager;
		public IFlyingTargetWaypointManager GetFlyingTargetWaypointManager(){
			return thisManager;
		}
		IFlyingTargetWaypointManager CreateFlyingTargetWaypointManager(){
			FlyingTargetWaypointManager.IConstArg arg = new FlyingTargetWaypointManager.ConstArg(
				this
			);
			return new FlyingTargetWaypointManager(arg);
		}
		IFlyingTargetWaypointAdaptor[] thisWaypointAdaptors;
		IFlyingTargetWaypointAdaptor[] CollectWaypointAdaptors(){
			List<IFlyingTargetWaypointAdaptor> resultList = new List<IFlyingTargetWaypointAdaptor>();
			Component[] childComponents = this.transform.GetComponentsInChildren<Component>();
			foreach(Component comp in childComponents)
				if(comp is IFlyingTargetWaypointAdaptor)
					resultList.Add((IFlyingTargetWaypointAdaptor)comp);
			return resultList.ToArray();
		}
		public override void SetUpReference(){
			IFlyingTargetWaypoint[] waypoints = CollectWaypoints();
			thisManager.SetWaypoints(waypoints);
		}
		IFlyingTargetWaypoint[] CollectWaypoints(){
			List<IFlyingTargetWaypoint> resultList = new List<IFlyingTargetWaypoint>();
			foreach(IFlyingTargetWaypointAdaptor adaptor in thisWaypointAdaptors)
				resultList.Add(adaptor.GetWaypoint());
			return resultList.ToArray();
		}
	}
}

