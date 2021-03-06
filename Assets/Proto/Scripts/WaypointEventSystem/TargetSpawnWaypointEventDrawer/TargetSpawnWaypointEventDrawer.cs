﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	[ExecuteInEditMode]
	public class TargetSpawnWaypointEventDrawer : MonoBehaviour {
		void Update(){}
		public AbsShootingTargetSpawnPointGroupAdaptor[] targetSpawnPointGroupAdaptors;
		public PCWaypointCurveAdaptor pcWaypointCurveAdaptor;

		bool thisIsReady = false;
		IShootingTargetSpawnPointAdaptor[] thisSpawnPointAdaptors{
			get{
				return CollectSpawnPointAdaptors();
			}
		}
		IShootingTargetSpawnPointAdaptor[] CollectSpawnPointAdaptors(){
			List<IShootingTargetSpawnPointAdaptor> resultList = new List<IShootingTargetSpawnPointAdaptor>();
			foreach(IShootingTargetSpawnPointGroupAdaptor groupAdaptor in targetSpawnPointGroupAdaptors)
				if(groupAdaptor != null)
					resultList.AddRange(groupAdaptor.GetAdaptors());
			return resultList.ToArray();
		}
		public TriggerMotorMechanismWaypointEventAdaptor[] triggerMotorMechanismWaypointEventAdaptors;
		public bool drawTriggers = true;

		public Color lineColor;
		public bool drawsStatics = true;
		public bool drawsGliders = true;
		void OnDrawGizmos(){
			Gizmos.color = lineColor;
			foreach(IShootingTargetSpawnPointAdaptor pointAdaptor in thisSpawnPointAdaptors){
				if(pointAdaptor != null){
					if(
						(pointAdaptor is IStaticTargetSpawnPointAdaptor && drawsStatics) ||
						(pointAdaptor is IGlidingTargetSpawnPointAdaptor && drawsGliders)
					){
						Vector3 position = pointAdaptor.GetPosition();
						float eventPoint = pointAdaptor.GetEventPoint();
						if(pcWaypointCurveAdaptor != null){
							Vector3 eventPointPosition = pcWaypointCurveAdaptor.GetPositionOnCurve(eventPoint);
							
							Gizmos.DrawLine(position, eventPointPosition);
						}
					}
				}
			}
			if(drawTriggers){
				foreach(ITriggerMotorMechanismWaypointEventAdaptor adaptor in triggerMotorMechanismWaypointEventAdaptors){
					// ITriggerMotorMechanismWaypointEvent wpEvent = adaptor.GetTriggerMotorMechanismWaypointEvent();
					Vector3 position = adaptor.GetMechanismPosition();
					float eventPoint = adaptor.GetEventPoint();
					if(pcWaypointCurveAdaptor != null){
						Vector3 eventPointPosition = pcWaypointCurveAdaptor.GetPositionOnCurve(eventPoint);
						
						Gizmos.DrawLine(position, eventPointPosition);
					}
				}
			}
		}
	}
}
