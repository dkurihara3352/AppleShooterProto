using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	[ExecuteInEditMode]
	public class TargetSpawnWaypointEventDrawer : MonoBehaviour {
		public AbsShootingTargetSpawnPointGroupAdaptor[] targetSpawnPointGroupAdaptors;
		public PCWaypointCurveAdaptor pcWaypointCurveAdaptor;
		void Update(){
			if(targetSpawnPointGroupAdaptors != null)
				thisSpawnPointAdaptors = CollectSpawnPointAdaptors();
			UpdateReadiness();
		}
		void UpdateReadiness(){
			if(thisIsReady){
				if(
					targetSpawnPointGroupAdaptors == null ||
					pcWaypointCurveAdaptor == null
				)
					thisIsReady = false;
			}else{
				if(
					targetSpawnPointGroupAdaptors != null &&
					pcWaypointCurveAdaptor != null &&
					thisSpawnPointAdaptors != null
				)
					thisIsReady = true;
			}
		}
		bool thisIsReady = false;
		IShootingTargetSpawnPointAdaptor[] thisSpawnPointAdaptors;
		IShootingTargetSpawnPointAdaptor[] CollectSpawnPointAdaptors(){
			List<IShootingTargetSpawnPointAdaptor> resultList = new List<IShootingTargetSpawnPointAdaptor>();
			foreach(IShootingTargetSpawnPointGroupAdaptor groupAdaptor in targetSpawnPointGroupAdaptors)
				if(groupAdaptor != null)
					resultList.AddRange(groupAdaptor.GetAdaptors());
			return resultList.ToArray();
		}
		public Color lineColor;
		// void OnDrawGizmos(){
		// 	if(thisIsReady){
		// 		Gizmos.color = lineColor;
		// 		foreach(IShootingTargetSpawnPointAdaptor pointAdaptor in thisSpawnPointAdaptors){
		// 			Vector3 position = pointAdaptor.GetPosition();
		// 			float eventPoint = pointAdaptor.GetEventPoint();
		// 			Vector3 eventPointPosition = pcWaypointCurveAdaptor.GetPositionOnCurve(eventPoint);
					
		// 			Gizmos.DrawLine(position, eventPointPosition);
		// 		}
		// 	}
		// }
	}
}
