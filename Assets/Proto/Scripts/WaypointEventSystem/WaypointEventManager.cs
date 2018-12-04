﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointEventManager{
		void CheckForWaypointEvent(float normalizedPositionOnCurve);
		void SetNewCurve(IWaypointCurve curve);
		void SetInitialEventPoint(float eventPoint);
		void ExecuteWaypointEventsUpTo(float eventPoint);
	}
	public class WaypointEventManager : IWaypointEventManager {
		IWaypointCurve thisCurrentCurve;
		Queue<IWaypointEvent> thisCurrentWaypontCurveEvents;
		public void SetNewCurve(IWaypointCurve curve){
			thisInitialEventPoint = 0f;
			thisCurrentCurve = curve;
			IWaypointEvent[] list = thisCurrentCurve.GetWaypointEvents();
			Queue<IWaypointEvent> result = new Queue<IWaypointEvent>();
			foreach(IWaypointEvent waypointEvent in list){
				result.Enqueue(waypointEvent);
				waypointEvent.Reset();
			}
			thisCurrentWaypontCurveEvents = result;
		}
		float thisInitialEventPoint = 0f;
		public void SetInitialEventPoint(float eventPoint){
			thisInitialEventPoint = eventPoint;
			thisCurrentWaypontCurveEvents = GetCorrectedWaypointEvents();
		}
		Queue<IWaypointEvent> GetCorrectedWaypointEvents(){
			Queue<IWaypointEvent> result = new Queue<IWaypointEvent>();
			foreach(IWaypointEvent wpEvent in thisCurrentWaypontCurveEvents){
				if(wpEvent.GetEventPoint() >= thisInitialEventPoint)
					result.Enqueue(wpEvent);
			}
			return result;
		}
		public void CheckForWaypointEvent(float normalizedPositionOnCurve){
			while(true){
				if(thisCurrentWaypontCurveEvents.Count != 0){
					IWaypointEvent nextWaypontEvent = thisCurrentWaypontCurveEvents.Peek();
					float eventPoint = nextWaypontEvent.GetEventPoint();
					if(eventPoint <= normalizedPositionOnCurve){
						if(eventPoint >= thisInitialEventPoint){
							nextWaypontEvent = thisCurrentWaypontCurveEvents.Dequeue();
							nextWaypontEvent.Execute();
						}
					}else{
						break;
					}
				}else{
					break;
				}
			}
		}
		public void ExecuteWaypointEventsUpTo(float eventPoint){

			foreach(IWaypointEvent wpEvent in thisCurrentCurve.GetWaypointEvents())
				if(wpEvent.GetEventPoint() < eventPoint)
					wpEvent.Execute();
		}
	}
}
