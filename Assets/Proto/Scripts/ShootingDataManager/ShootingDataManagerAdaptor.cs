using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingDataManagerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IShootingDataManager GetShootingDataManager();

		int GetTierSteps();
		int GetTierCount();
		int[] GetTierLevelMultipliers();

		float[] GetGlobalMinDrawStrengthLimit();
		float[] GetGlobalMaxDrawStrengthLimit();
		float[] GetGlobalFireRateLimit();
		float[] GetGlobalDrawTimeLimit();
		float[] GetGlobalCriticalLimit();
	}
	public class ShootingDataManagerAdaptor : AppleShooterMonoBehaviourAdaptor, IShootingDataManagerAdaptor {

		public override void SetUp(){
			base.SetUp();
			thisShootingDataManager = CreateShootingDataManager();
		}
		IShootingDataManager thisShootingDataManager;
		public IShootingDataManager GetShootingDataManager(){
			return thisShootingDataManager;
		}
		IShootingDataManager CreateShootingDataManager(){
			ShootingDataManager.IConstArg arg = new ShootingDataManager.ConstArg(
				this
			);
			return new ShootingDataManager(arg);
		}

		public override void SetUpReference(){
			base.SetUpReference();
			IPlayerDataManager playerDataManager = CollectPlayerDataManager();
			thisShootingDataManager.SetPlayerDataManager(playerDataManager);
		}
		public PlayerDataManagerAdaptor playerDataManagerAdaptor;
		IPlayerDataManager CollectPlayerDataManager(){
			return playerDataManagerAdaptor.GetPlayerDataManager();
		}


		/*  */
		public int tierSteps = 4;
		public int GetTierSteps(){
			return tierSteps;
		}
		public int tierCount = 3;
		public int GetTierCount(){
			return tierCount;
		}
		public int[] tierLevelMultipliers = new int[3]{1, 2, 4};
		public int[] GetTierLevelMultipliers(){
			return tierLevelMultipliers;
		}

		public float[] globalMinDrawStrengthLimit = new float[2]{0f, .2f};
		public float[] GetGlobalMinDrawStrengthLimit(){
			return globalMinDrawStrengthLimit;
		}
		public float[] globalMaxDrawStrengthLimit = new float[2]{.1f, 1f};
		public float[] GetGlobalMaxDrawStrengthLimit(){
			return globalMaxDrawStrengthLimit;
		}
		public float[] globalFireRateLimit = new float[2]{1f, .2f};
		public float[] GetGlobalFireRateLimit(){
			return globalFireRateLimit;
		}
		public float[] globalDrawTimeLimit = new float[2]{1f, .2f};
		public float[] GetGlobalDrawTimeLimit(){
			return globalDrawTimeLimit;
		}
		public float[] globalCriticalMulitiplierLimit = new float[2]{1.1f, 4f};
		public float[] GetGlobalCriticalLimit(){
			return globalCriticalMulitiplierLimit;
		}
		
	}
}
