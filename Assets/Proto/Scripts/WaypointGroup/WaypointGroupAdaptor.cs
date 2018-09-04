using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointGroupAdaptor{
		IWaypointGroup GetWaypointGroup();
		void SetPosition(Vector3 position);
		void SetYRotation(float yRotation);
		Vector3 GetConnectionPosition();
		float GetConnectionEulerY();
	}
	public class WaypointGroupAdaptor : MonoBehaviour, IWaypointGroupAdaptor {
		IWaypointsManager thisManager;
		public void SetWaypointsManager(IWaypointsManager manager){
			thisManager = manager;
		}
		IWaypointGroup thisWaypointGroup;
		public IWaypointGroup GetWaypointGroup(){
			return thisWaypointGroup;
		}
		public Transform connectionPoint;
		public Vector3 GetConnectionPosition(){
			return connectionPoint.position;
		}
		public float GetConnectionEulerY(){
			return connectionPoint.eulerAngles.y;
		}
		public void SetUpWaypointGroup(){
			IWaypointConnection waypointConnection = new WaypointConnection(
				GetConnectionPosition(),
				GetConnectionEulerY()
			);
			List<IWaypointAdaptor> childWaypointAdaptors = GetChildWaypointAdaptors();
			thisWaypointGroup = new WaypointGroup(
				this,
				thisManager,
				waypointConnection,
				childWaypointAdaptors
			);
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
		public void SetPosition(Vector3 position){
			this.transform.position = position;
		}
		public void SetYRotation(float yRotation){
			this.transform.Rotate(
				new Vector3(0f, yRotation, 0f),
				Space.World
			);
		}
	}
}
