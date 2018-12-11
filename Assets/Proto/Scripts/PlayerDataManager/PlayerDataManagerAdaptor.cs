using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPlayerDataManagerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IPlayerDataManager GetPlayerDataManager();
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
	}
}
