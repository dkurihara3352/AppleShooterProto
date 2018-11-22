using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IHeatManager: IAppleShooterSceneObject, IHeatManagerStateHandler{
		void SetHeatImage(IHeatImage heatImage);
		void TickAwayHeat(float delta);
		void AddHeat(float delta);
		float GetFollowSmoothTime();
		float GetComboWindowTime();

		float GetMaxHeat();
		void SetMaxHeat(float maxHeat);
	}	
	public class HeatManager: AppleShooterSceneObject, IHeatManager{
		
		public HeatManager(
			IConstArg arg
		): base(
			arg
		){
			thisHeatDecayRate = arg.heatDecayRate;
			thisStateEngine = CreateStateEngine();
			thisHeat = arg.initialHeat;
			thisFollowSmoothTime = arg.followSmoothTime;

			thisMaxComboValue = arg.maxComboValue;
			thisMinComboTime = arg.minComboTime;
			thisMaxComboTime = arg.maxComboTime;
			thisComboTimeMultiplier = arg.comboTimeMultiplier;

			thisMaxHeat = arg.initialMaxHeat;
			thisLevelUpTime = arg.levelUpTime;
		}
		IHeatManagerStateEngine thisStateEngine;
		float thisHeatDecayRate;
		IHeatManagerStateEngine CreateStateEngine(){
			HeatManagerStateEngine.IConstArg stateEngineArg = new HeatManagerStateEngine.ConstArg(
				this,
				thisHeatDecayRate,
				thisAppleShooterProcessFactory
			);
			return new HeatManagerStateEngine(stateEngineArg);
		}
		float thisFollowSmoothTime;
		public float GetFollowSmoothTime(){
			return thisFollowSmoothTime;
		}
		float thisHeat;
		IHeatImage thisHeatImage;
		public void SetHeatImage(IHeatImage image){
			thisHeatImage = image;
			image.SetHeatManager(this);
			thisHeatImage.UpdateHeat(thisHeat);
		}
		public void TickAwayHeat(float delta){
			thisHeat -= delta;
			thisHeatImage.UpdateHeat(thisHeat);
		}
		public void AddHeat(float delta){
			thisHeat += delta;
			if(thisHeat > thisMaxHeat)
				LevelUpHeat();
			StopCountingDown();
			thisHeatImage.AddHeat(delta);
		}
		public void StartCountingDown(){
			thisStateEngine.StartCountingDown();
		}
		public void StopCountingDown(){
			thisStateEngine.StopCountingDown();
		}

		float thisMaxHeat;
		public void SetMaxHeat(float maxHeat){
			thisMaxHeat = maxHeat;
			thisHeatImage.UpdateHeat(thisHeat);
		}
		public float GetMaxHeat(){
			return thisMaxHeat;
		}

		float thisMaxComboValue;
		float thisMinComboTime;
		float thisMaxComboTime;
		float thisComboTimeMultiplier;
		public float GetComboWindowTime(){
			float comboValue = thisHeatImage.GetComboValue();
			float normalizedComboValue = comboValue / thisMaxComboValue;
			float result = Mathf.Lerp(
				thisMinComboTime,
				thisMaxComboTime,
				normalizedComboValue
			);
			result *= thisComboTimeMultiplier;
			return result;
		}
		readonly float thisLevelUpTime;
		void LevelUpHeat(){
			float targetMaxHeat = thisMaxHeat * 2f;
			IHeatLevelUpProcess process = thisAppleShooterProcessFactory.CreateHeatLevelUpProcess(
				this,
				targetMaxHeat,
				thisLevelUpTime
			);
			process.Run();
		}
		/*  */
		public new interface IConstArg: AppleShooterSceneObject.IConstArg{
			float initialHeat{get;}
			float heatDecayRate{get;}
			float followSmoothTime{get;}

			float maxComboValue{get;}
			float minComboTime{get;}
			float maxComboTime{get;}
			float comboTimeMultiplier{get;}

			float initialMaxHeat{get;}
			float levelUpTime{get;}
		}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IHeatManagerAdaptor adaptor,
				float initialHeat,
				float heatDecayRate,
				float followSmoothTime,

				float maxComboValue,
				float minComboTime,
				float maxComboTime,
				float comboTimeMultiplier,

				float initialMaxHeat,
				float levelUpTime
			): base(
				adaptor
			){
				thisInitialHeat = initialHeat;
				thisHeatDecayRate = heatDecayRate;
				thisFollowSmoothTime = followSmoothTime;
				thisMaxComboValue = maxComboValue;
				thisMinComboTime = minComboTime;
				thisMaxComboTime = maxComboTime;
				thisComboTimeMultiplier = comboTimeMultiplier;

				thisInitialMaxHeat = initialMaxHeat;
				thisLevelUpTime = levelUpTime;
			}
			readonly float thisHeatDecayRate;
			public float heatDecayRate{
				get{
					return thisHeatDecayRate;
				}
			}
			readonly float thisInitialHeat;
			public float initialHeat{get{return thisInitialHeat;}}
			readonly float thisFollowSmoothTime;
			public float followSmoothTime{get{return thisFollowSmoothTime;}}
			readonly float thisMaxComboValue;
			public float maxComboValue{get{return thisMaxComboValue;}}
			readonly float thisMinComboTime;
			public float minComboTime{get{return thisMinComboTime;}}
			readonly float thisMaxComboTime;
			public float maxComboTime{get{return thisMaxComboTime;}}
			readonly float thisComboTimeMultiplier;
			public float comboTimeMultiplier{get{return thisComboTimeMultiplier;}}
			readonly float thisInitialMaxHeat;
			public float initialMaxHeat{get{return thisInitialMaxHeat;}}
			readonly float thisLevelUpTime;
			public float levelUpTime{get{return thisLevelUpTime;}}
		}
	}
}
