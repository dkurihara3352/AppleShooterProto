using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IShootingTargetSpawnPointAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		IShootingTargetSpawnPoint GetSpawnPoint();
		float GetEventPoint();
		float GetRelativeRareChance();
	}
	public abstract class AbsShootingTargetSpawnPointAdaptor: SlickBowShootingMonoBehaviourAdaptor, IShootingTargetSpawnPointAdaptor{
		protected IShootingTargetSpawnPoint thisSpawnPoint;
		public IShootingTargetSpawnPoint GetSpawnPoint(){
			return thisSpawnPoint;
		}
		[Range(0f, 1f)]
		public float eventPoint;
		public float GetEventPoint(){
			return eventPoint;
		}
		public float rareRelativeProbability = 1f;
		public float GetRelativeRareChance(){
			return rareRelativeProbability;
		}
		protected void DisableAllMeshRenderer(){
			Component[] components = this.transform.GetComponentsInChildren<Component>();
			foreach(Component comp in components){
				if(comp is MeshRenderer)
					((MeshRenderer)comp).enabled = false;
			}
		}
	}
}

