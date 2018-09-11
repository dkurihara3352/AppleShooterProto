using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointGroupAdaptor: IMonoBehaviourAdaptor{
		IWaypointGroup GetWaypointGroup();
		Vector3 GetConnectionPosition();
		Quaternion GetConnectionRotation();
		void SetWaypointsManager(IWaypointsManager manager);
		void SetManagerOnAllWaypointAdaptors();
	}
	public class WaypointGroupAdaptor : MonoBehaviourAdaptor, IWaypointGroupAdaptor {
		IWaypointsManager thisManager;
		public void SetWaypointsManager(IWaypointsManager manager){
			thisManager = manager;
		}
		public void SetManagerOnAllWaypointAdaptors(){
			foreach(IWaypointAdaptor wpAdaptor in GetChildWaypointAdaptors()){
				wpAdaptor.SetWaypointsManager(thisManager);
			}
		}
		IWaypointGroup thisWaypointGroup;
		public IWaypointGroup GetWaypointGroup(){
			return thisWaypointGroup;
		}
		public Transform connectionPoint;
		public Vector3 GetConnectionPosition(){
			return connectionPoint.position;
		}
		public Quaternion GetConnectionRotation(){
			return connectionPoint.rotation;
		}
		public override void SetUp(){
			IWaypointConnection waypointConnection = new WaypointConnection(
				GetConnectionPosition(),
				GetConnectionRotation()
			);
			thisWaypointGroup = new WaypointGroup(
				this,
				thisManager,
				waypointConnection
			);
			thisChildWaypointAdaptors = GetChildWaypointAdaptors();
			thisManager.AddWaypointGroup(thisWaypointGroup);
		}
		List<IWaypointAdaptor> thisChildWaypointAdaptors;
		public override void SetUpReference(){
			List<IWaypoint> waypoints = GetWaypointsFromAdaptors(thisChildWaypointAdaptors);
			thisWaypointGroup.SetWaypoints(waypoints);
			thisWaypointGroup.CacheTravelData(thisManager.GetSpeed());
		}
		List<IWaypoint> GetWaypointsFromAdaptors(List<IWaypointAdaptor> adaptors){
			List<IWaypoint> result = new List<IWaypoint>();
			foreach(IWaypointAdaptor adaptor in adaptors)
				result.Add(adaptor.GetWaypoint());
			return result;
		}

		List<IWaypointAdaptor> GetChildWaypointAdaptors(){
			List<IWaypointAdaptor> result = new List<IWaypointAdaptor>();
			int childCount = this.transform.childCount;
			for(int i = 0; i < childCount; i ++){
				Transform child = this.transform.GetChild(i);
				IWaypointAdaptor waypointAdaptor = child.GetComponent(typeof(IWaypointAdaptor)) as IWaypointAdaptor;
				if(waypointAdaptor != null){
					result.Add(waypointAdaptor);
				}
			}
			return result;
		}
	}
}
