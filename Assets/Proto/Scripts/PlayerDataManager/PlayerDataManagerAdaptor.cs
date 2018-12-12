using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPlayerDataManagerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IPlayerDataManager GetPlayerDataManager();

		int GetTierSteps();
		int GetTierCount();
		int[] GetTierLevelMultipliers();
	}
	public class PlayerDataManagerAdaptor : AppleShooterMonoBehaviourAdaptor, IPlayerDataManagerAdaptor {

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
	}
}
