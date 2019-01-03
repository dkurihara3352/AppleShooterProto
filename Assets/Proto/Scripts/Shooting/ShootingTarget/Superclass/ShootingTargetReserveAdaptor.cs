using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetReserveAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IShootingTargetReserve GetReserve();
		int GetSpawnValue();
		TargetType GetTargetType();
		Material GetMaterialForTier(int tier);
		TargetData GetTargetDataForTier(int tier);
		float GetTargetTypeRareProbability();
	}
	public abstract class AbsShootingTargetReserveAdaptor : AppleShooterMonoBehaviourAdaptor, IShootingTargetReserveAdaptor {
		public override void SetUpReference(){
			IDestroyedTargetReserve destroyedTargetReserve = destroyedTargetReserveAdaptor.GetDestroyedTargetReserve();
			thisReserve.SetDestroyedTargetReserve(destroyedTargetReserve);
		}
		public override void FinalizeSetUp(){
			base.FinalizeSetUp();
			thisReserve.SetTier(0);
		}
		protected IShootingTargetReserve thisReserve;
		public IShootingTargetReserve GetReserve(){
			return thisReserve;
		}
		public float reservedSpace;
		public int targetCount;
		public GameStatsTrackerAdaptor gameStatsTrackerAdaptor;
		public ShootingManagerAdaptor shootingManagerAdaptor;
		public DestroyedTargetReserveAdaptor destroyedTargetReserveAdaptor;
		public UnityBase.PopUIReserveAdaptor popUIReserveAdaptor;
		public AudioManagerAdaptor audioManagerAdaptor;
		public GameObject shootingTargetPrefab;
		public int GetSpawnValue(){
			return thisSpawnValue;
		}
		public int thisSpawnValue;
		public TargetType targetType;
		public TargetType GetTargetType(){
			return targetType;
		}
		public Material[] materials;
		public Material GetMaterialForTier(int tier){
			return materials[tier];
		}
		public TargetData[] targetDataArray;
		public TargetData GetTargetDataForTier(int tier){
			return targetDataArray[tier];
		}
		public float targetTypeRareProbability = 0f;
		public float GetTargetTypeRareProbability(){
			return targetTypeRareProbability;
		}
	}
}
