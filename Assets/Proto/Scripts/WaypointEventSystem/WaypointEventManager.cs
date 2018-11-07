using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointEventManager{
		void CheckForWaypointEvent(float normalizedPositionOnCurve);
		void SetNewCurve(IWaypointCurve curve);
	}
	public class WaypointEventManager : IWaypointEventManager {
		IWaypointCurve thisCurrentCurve;
		Queue<IWaypointEvent> thisCurrentWaypontCurveEvents;
		public void SetNewCurve(IWaypointCurve curve){
			thisCurrentCurve = curve;
			IWaypointEvent[] list = thisCurrentCurve.GetWaypointEvents();
			Queue<IWaypointEvent> result = new Queue<IWaypointEvent>();
			foreach(IWaypointEvent waypointEvent in list){
				result.Enqueue(waypointEvent);
			}
			thisCurrentWaypontCurveEvents = result;
		}
		public void CheckForWaypointEvent(float normalizedPositionOnCurve){
			while(true){
				if(thisCurrentWaypontCurveEvents.Count != 0){
					IWaypointEvent nextWaypontEvent = thisCurrentWaypontCurveEvents.Peek();
					float eventPoint = nextWaypontEvent.GetEventPoint();
					if(eventPoint <= normalizedPositionOnCurve){
						nextWaypontEvent = thisCurrentWaypontCurveEvents.Dequeue();
						nextWaypontEvent.Execute();
					}else{
						break;
					}
				}else{
					break;
				}
			}
		}
	}
}
