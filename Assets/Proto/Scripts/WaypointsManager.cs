using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointsManager{
		void SetFollower(IWaypointsFollower follower);
		IWaypointsFollower GetFollower();
		List<IWaypointGroup> GetWaypointGroups();
		void PlaceWaypointGroups();
		IWaypointConnection GetInitialConnection();
		IWaypointGroup GetNextWaypointGroup(
			IWaypointGroup currentGroup
		);
	}
	public class WaypointsManager : MonoBehaviour, IWaypointsManager {
		IWaypointsFollower thisFollower;
		public IWaypointsFollower GetFollower(){
			return thisFollower;
		}
		public void SetFollower(IWaypointsFollower follower){
			thisFollower = follower;
		}
		List<IWaypointGroup> thisWaypointGroupsInSequence;

		public List<WaypointGroupAdaptor> waypointGroupAdaptors;
		List<IWaypointGroup> thisWaypointGroups;
		public void SetUpAllWaypointGroups(){
			List<IWaypointGroup> result = new List<IWaypointGroup>();
			foreach(WaypointGroupAdaptor waypointGroupAdaptor in waypointGroupAdaptors){
				waypointGroupAdaptor.SetWaypointsManager(this);
				waypointGroupAdaptor.SetUpWaypointGroup();
				IWaypointGroup waypointGroup = waypointGroupAdaptor.GetWaypointGroup();
				float speed = thisFollower.GetSpeed();
				waypointGroup.SetUpWaypoints(speed);

				result.Add(waypointGroup);
			}
			thisWaypointGroups = result;
		}
		public List<IWaypointGroup> GetWaypointGroups(){
			return thisWaypointGroups;
		}

		public void PlaceWaypointGroups(){
			List<IWaypointGroup> sequence = CreateSequenceOfWaypointGroups();
			IWaypointGroup prevWaypointGroup = null;
			foreach(IWaypointGroup group in sequence){
				group.Connect(prevWaypointGroup);
				prevWaypointGroup = group;
			}
			UpdateConnectedWaypointCacheDataOnAllGroup();
		}
		List<IWaypointGroup> CreateSequenceOfWaypointGroups(){
			return thisWaypointGroups;
		}
		void UpdateConnectedWaypointCacheDataOnAllGroup(){
			float speed = thisFollower.GetSpeed();
			foreach(IWaypointGroup group in thisWaypointGroups){
				IWaypointGroup nextWaypointGroup = GetNextWaypointGroup(group);
				if(nextWaypointGroup != null){
					IWaypoint prevGroupLastWP = group.GetLastWaypoint();
					IWaypoint nextGroupFirstWP = nextWaypointGroup.GetFirstWaypoint();

					nextGroupFirstWP.CacheDistanceFromPreviousWaypoint(prevGroupLastWP);
					nextGroupFirstWP.CacheRequiredTime(speed);
				}
			}
		}
		public IWaypointGroup GetNextWaypointGroup(IWaypointGroup group){
			int indexOfGroup = thisWaypointGroups.IndexOf(group);
			if(indexOfGroup != thisWaypointGroups.Count -1){
				return thisWaypointGroups[indexOfGroup + 1];
			}
				return null;
		}
		public IWaypointConnection GetInitialConnection(){
			Vector3 origin = Vector3.zero;
			float originalYRotation = 0f;
			IWaypointConnection result = new WaypointConnection(
				origin,
				originalYRotation
			);
			return result;
		}
	}
}
