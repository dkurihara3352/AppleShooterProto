using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IHeatManagerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IHeatManager GetHeatManager();
		float GetStandardComboTime();
		float GetMaxComboTimeMultiplier();
		float GetMinComboValue();
		float GetMaxComboValue();
		float GetLevelUpMultiplier();
	}
	public class HeatManagerAdaptor : AppleShooterMonoBehaviourAdaptor, IHeatManagerAdaptor {

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

		public float standardComboTime = .5f;
		public float GetStandardComboTime(){
			return standardComboTime;
		}
		public float maxComboTimeMultiplier = 3f;
		public float GetMaxComboTimeMultiplier(){
			return maxComboTimeMultiplier;
		}
		public float minComboValue = .05f;
		public float GetMinComboValue(){
			return minComboValue;
		}
		public float maxComboValue = .5f;
		public float GetMaxComboValue(){
			return maxComboValue;
		}
		public float levelUpMultiplier = 4f;
		public float GetLevelUpMultiplier(){
			return levelUpMultiplier;
		}

		public float initialMaxHeat;
		public float levelUpTime;
		IHeatManager CreateHeatManager(){
			HeatManager.IConstArg arg = new HeatManager.ConstArg(
				this,
				initialHeat,
				heatDecayRate,
				followSmoothTime,

				initialMaxHeat,
				levelUpTime
			);
			return new HeatManager(arg);
		}
		public HeatImageAdaptor heatImageAdaptor;
		public HeatLevelTextAdaptor heatLevelTextAdaptor;
		public override void SetUpReference(){
			IHeatImage heatImage = heatImageAdaptor.GetHeatImage();
			thisHeatManager.SetHeatImage(heatImage);

			IHeatLevelText heatLevelText = heatLevelTextAdaptor.GetHeatLevelText();
			thisHeatManager.SetHeatLevelText(heatLevelText);

			IShootingTargetReserve[] shootingTargetReserves = CollectShootingTargetReserves();
			thisHeatManager.SetShootingTargetReserves(shootingTargetReserves);

			IGameplayWidget widget = gameplayWidgetAdaptor.GetGameplayWidget();
			thisHeatManager.SetGameplayWidget(widget);

			IColorSchemeManager colorSchemeManager = colorSchemeManagerAdaptor.GetColorSchemeManager();
			thisHeatManager.SetColorSchemeManager(colorSchemeManager);
		}
		public AbsShootingTargetReserveAdaptor[] shootingTargetReserveAdaptors;
		public GameplayWidgetAdaptor gameplayWidgetAdaptor;
		public ColorSchemeManagerAdaptor colorSchemeManagerAdaptor;
		IShootingTargetReserve[] CollectShootingTargetReserves(){
			List<IShootingTargetReserve> resultList = new List<IShootingTargetReserve>();
			foreach(IShootingTargetReserveAdaptor adaptor in shootingTargetReserveAdaptors)
				resultList.Add(adaptor.GetReserve());
			return resultList.ToArray();
		}
		public override void FinalizeSetUp(){
			thisHeatManager.InitializeHeat();
		}
	}
}
