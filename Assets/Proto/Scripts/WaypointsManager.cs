using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointsManager{
		void SetFollower(IWaypointsFollower follower);
		IWaypointsFollower GetFollower();
		List<IWaypointGroup> GetWaypointGroupsInSequence();
		void PlaceWaypointGroups();
		IWaypointConnection GetInitialConnection();
		IWaypointGroup GetNextWaypointGroup(
			IWaypointGroup currentGroup
		);
		void CycleGroup();
		int GetWaypointGroupIndex(IWaypointGroup group);
	}
	public class WaypointsManager : MonoBehaviour, IWaypointsManager {
		void Awake(){
			thisGroupSequence = new List<IWaypointGroup>();
		}
		IWaypointsFollower thisFollower;
		public IWaypointsFollower GetFollower(){
			return thisFollower;
		}
		public void SetFollower(IWaypointsFollower follower){
			thisFollower = follower;
		}

		public List<WaypointGroupAdaptor> waypointGroupAdaptors;
		List<IWaypointGroup> thisWaypointGroups;
		public int GetWaypointGroupIndex(IWaypointGroup group){
			return thisWaypointGroups.IndexOf(group);
		}
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

		public void PlaceWaypointGroups(){
			PlaceAllWaypointGroupAtReserve();
			List<IWaypointGroup> sequence = CreateSequenceOfWaypointGroups();
			thisGroupSequence = sequence;
			IWaypointGroup prevWaypointGroup = null;
			foreach(IWaypointGroup group in sequence){
				group.Connect(prevWaypointGroup);
				prevWaypointGroup = group;
			}
			UpdateConnectedWaypointCacheDataOnAllGroupInSequence();
		}
		/* Creating sequence */
			public int groupCountInSequence = 3;
			List<IWaypointGroup> thisGroupSequence;
			public List<IWaypointGroup> GetWaypointGroupsInSequence(){
				return thisGroupSequence;
			}
			List<IWaypointGroup> CreateSequenceOfWaypointGroups(){
				int[] indexes = new int[groupCountInSequence];
				List<int> used = new List<int>();
				for(int i = 0; i < groupCountInSequence; i ++){
					int index = GetRandomInt(
						groupCountInSequence,
						used
					);
					used.Add(index);
					indexes[i] = index;
				}
				return GetWaypointsListAtIndexes(indexes);
			}
			int GetRandomInt(
				int max,
				List<int> usedInt
			){
				int nonUsedIndexCount  = max - usedInt.Count + 1;
				if(nonUsedIndexCount == 0)
					throw new System.InvalidOperationException(
						"there's no more unused index in source"
					);
				List<int> nonUsedIndexes = new List<int>();
				for(int i = 0; i < max; i ++){
					if(!usedInt.Contains(i))
						nonUsedIndexes.Add(i);
				}
				int randomIndex = Random.Range(0, nonUsedIndexes.Count);
				return nonUsedIndexes[randomIndex];
			}
			List<IWaypointGroup> GetWaypointsListAtIndexes(int[] indexes){
				List<IWaypointGroup> result = new List<IWaypointGroup>();
				foreach(int index in indexes)
					result.Add(
						thisWaypointGroups[index]
					);
				return result;
			}
			public IWaypointGroup GetFirstGroupInSequence(){
				return thisGroupSequence[0];
			}
			public IWaypointGroup GetLastGroupInSequence(){
				return thisGroupSequence[groupCountInSequence - 1];
			}
		/*  */
		void UpdateConnectedWaypointCacheDataOnAllGroupInSequence(){
			float speed = thisFollower.GetSpeed();
			IWaypointGroup prevWaypointGroup = null;
			foreach(IWaypointGroup group in thisGroupSequence){
				group.UpdateConnectedWaypointCacheData(
					prevWaypointGroup,
					speed
				);
				prevWaypointGroup = group;
			}
		}
		public Transform groupReservePointTransform;
		Vector3 thisReservePosition{
			get{return groupReservePointTransform.position;}
		}
		void PlaceAllWaypointGroupAtReserve(){
			foreach(IWaypointGroup group in thisWaypointGroups){
				group.SetPosition(thisReservePosition);
			}
		}
		public IWaypointGroup GetNextWaypointGroup(IWaypointGroup group){
			int indexOfGroup = thisGroupSequence.IndexOf(group);
			if(indexOfGroup != thisGroupSequence.Count -1){
				return thisGroupSequence[indexOfGroup + 1];
			}
				return null;
		}
		public IWaypointConnection GetInitialConnection(){
			Vector3 origin = Vector3.zero;
			IWaypointConnection result = new WaypointConnection(
				origin,
				Quaternion.identity
			);
			return result;
		}
		public void CycleGroup(){
			/*  move first group to reserve 
				get random one from reserve and place it
			*/
			if(ShouldCycle()){
				IWaypointGroup lastWaypointGroup = thisGroupSequence[thisGroupSequence.Count - 1];
				RemoveFirstWaypointGroupToReserve();
				IWaypointGroup newGroupToAdd = GetNewWaypointGroupToAddToSequence();
				ReformList(
					newGroupToAdd
				);
				newGroupToAdd.Connect(lastWaypointGroup);
				float speed = thisFollower.GetSpeed();
				newGroupToAdd.UpdateConnectedWaypointCacheData(
					lastWaypointGroup,
					speed
				);
			}
		}
		bool thisCycleHasStarted = false;
		int thisIndexToStartCycle = 1;
		bool ShouldCycle(){
			if(thisCycleHasStarted){
				return true;
			}else{
				IWaypointGroup currentGroup = thisFollower.GetCurrentWaypointGroup();
				int indexOfCurGroup = thisGroupSequence.IndexOf(currentGroup);
				if(indexOfCurGroup == thisIndexToStartCycle){
					thisCycleHasStarted = true;
					return true;
				}
			}
				return false;
		}
		void RemoveFirstWaypointGroupToReserve(){
			IWaypointGroup firstGroupInSequence = thisGroupSequence[0];
			firstGroupInSequence.SetPosition(thisReservePosition);
			List<IWaypointGroup> reducedSequence = new List<IWaypointGroup>(thisGroupSequence);
			reducedSequence.Remove(firstGroupInSequence);
			thisGroupSequence = reducedSequence;
		}
		IWaypointGroup GetNewWaypointGroupToAddToSequence(){
			List<IWaypointGroup> groupsInReserve = new List<IWaypointGroup>();
			foreach(IWaypointGroup group in thisWaypointGroups)
				if(!thisGroupSequence.Contains(group))
					groupsInReserve.Add(group);
			int randomIndex = Random.Range(0, groupsInReserve.Count);
			return groupsInReserve[randomIndex];
		}
		void ReformList(
			IWaypointGroup newGroupToAdd
		){
			// List<IWaypointGroup> result = new List<IWaypointGroup>();
			// for(int i = 1; i < thisGroupSequence.Count; i ++){
			// 	result.Add(thisGroupSequence[i]);
			// }
			// result.Add(newGroupToAdd);

			// thisGroupSequence = result;
			thisGroupSequence.Add(newGroupToAdd);
		}
	}
}
