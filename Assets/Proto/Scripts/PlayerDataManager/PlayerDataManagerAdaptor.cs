using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IPlayerDataManagerAdaptor: ISlickBowShootingMonoBehaviourAdaptor, BowDataCalculator.IAdaptor{
		IPlayerDataManager GetPlayerDataManager();
		int[] GetBowUnlockCostArray();
	}
	public class PlayerDataManagerAdaptor : SlickBowShootingMonoBehaviourAdaptor, IPlayerDataManagerAdaptor {

		public override void SetUp(){
			thisManager = CreatePlayerDataManager();
		}
		IPlayerDataManager thisManager;
		public IPlayerDataManager GetPlayerDataManager(){
			return thisManager;
		}
		IPlayerDataManager CreatePlayerDataManager(){
			PlayerDataManager.IConstArg arg = new PlayerDataManager.ConstArg(
				this
			);
			return new PlayerDataManager(arg);
		}
		/* BowDataCalculator adaptor */
		public float attributeMultiplier = 1.2f;
		public float GetAttributeMultiplier(){
			return attributeMultiplier;
		}
		public int maxAttributeLevel = 5;
		public int GetMaxAttributeLevel(){
			return maxAttributeLevel;
		}
		public int baseCost = 10;
		public int GetBaseCost(){
			return baseCost;
		}
		public float maxCoinDepreciationValue = 30f;
		public float GetMaxCoinDepreciationValue(){
			return maxCoinDepreciationValue;
		}
		public int[] bowUnlockCostArray = new int[]{
			0,
			100,
			500
		};
		public int[] GetBowUnlockCostArray(){
			return bowUnlockCostArray;
		}
	}
}
