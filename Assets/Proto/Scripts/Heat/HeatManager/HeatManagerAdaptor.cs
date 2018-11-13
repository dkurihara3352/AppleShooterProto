using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IHeatManagerAdaptor: IMonoBehaviourAdaptor{
		IHeatManager GetHeatManager();
	}
	public class HeatManagerAdaptor : MonoBehaviourAdaptor, IHeatManagerAdaptor {

		IHeatManager thisHeatManager;
		public IHeatManager GetHeatManager(){
			return thisHeatManager;
		}
		public override void SetUp(){
			thisHeatManager = CreateHeatManager();
		}
		public float initialHeat;
		public float heatDecayRate;
		public float followSmoothTime;

		public float maxComboValue;
		public float minComboTime;
		public float maxComboTime;
		public float comboTimeMultiplier;
		public float initialMaxHeat;
		public float levelUpTime;
		IHeatManager CreateHeatManager(){
			float scaledInitialHeat = initialHeat * initialMaxHeat;
			HeatManager.IConstArg arg = new HeatManager.ConstArg(
				this,
				scaledInitialHeat,
				heatDecayRate,
				followSmoothTime,

				maxComboValue,
				minComboTime,
				maxComboTime,
				comboTimeMultiplier,

				initialMaxHeat,
				levelUpTime
			);
			return new HeatManager(arg);
		}
		public HeatImageAdaptor heatImageAdaptor;
		public override void SetUpReference(){
			IHeatImage heatImage = heatImageAdaptor.GetHeatImage();
			thisHeatManager.SetHeatImage(heatImage);
		}
	}
}
