using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointAdaptor: IMonoBehaviourAdaptor{
		IWaypoint GetWaypoint();
		void SetWaypointsManager(IWaypointsManager manager);
		void UpdateGizmosFields(
			Vector3 prevWaypointPosition,
			Vector3 thisPosition,
			Vector3 refPointPosition,
			Vector3 project
		);
		void ToggleGizmosDrawing(bool draws);
	}
	public class WaypointAdaptor: MonoBehaviourAdaptor, IWaypointAdaptor{

		/* disregard SetUp, it is taken care by group
		 */
		 IWaypointsManager thisWaypointsManager;
		 public void SetWaypointsManager(IWaypointsManager manager){
			 thisWaypointsManager = manager;
		 }
		public override void SetUp(){
			thisWaypoint = new Waypoint(
				this,
				thisWaypointsManager
			);
		}
		IWaypoint thisWaypoint;
		public IWaypoint GetWaypoint(){
			return thisWaypoint;
		}
		bool isReadyForGizmos = false;
		public void GetReadyForGizmos(){
			isReadyForGizmos = true;
		}
		public void UpdateGizmosFields(
			Vector3 prevWaypointPosition,
			Vector3 thisPosition,
			Vector3 refPointPosition,
			Vector3 project
		){
			this.prevWaypointPosition = prevWaypointPosition;
			this.thisPosition = thisPosition;
			this.refPointPosition = refPointPosition;
			this.project = project;
		}
		Vector3 prevWaypointPosition;
		Vector3 thisPosition;
		Vector3 refPointPosition;
		Vector3 project;
		public void ToggleGizmosDrawing(bool draws){
			this.isReadyForGizmos = draws;
		}
		public void OnDrawGizmos(){
			if(this.isReadyForGizmos){
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(prevWaypointPosition, 1f);
				Gizmos.color = Color.magenta;
				Gizmos.DrawWireSphere(thisPosition, 1f);
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireSphere(refPointPosition, 1f);
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(project, 1f);
			}
		}
	}
}
