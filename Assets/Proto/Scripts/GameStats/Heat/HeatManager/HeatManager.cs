using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface IHeatManager: ISlickBowShootingSceneObject, IHeatManagerStateHandler{
		void SetHeatLevelText(IHeatLevelText text);
		void SetShootingTargetReserves(IShootingTargetReserve[] reserves);
		void SetGameplayWidget(IGameplayWidget widget);
		void SetColorSchemeManager(IColorSchemeManager manager);

		void InitializeHeat();
		void SetHeatImage(IHeatImage heatImage);
		void TickAwayHeat(float delta);
		void AddHeat(float delta);
		float GetFollowSmoothTime();
		float GetComboTime(float normalizedComboValue);
		void ResetCurrentMaxComboMultiplier();

		float GetMaxHeat();
		void SetMaxHeat(float maxHeat);
		void ResetHeat();
		void OnLevelUpExpire();

		bool IsCountingDown();
	}	
	public class HeatManager: SlickBowShootingSceneObject, IHeatManager{
		
		public HeatManager(
			IConstArg arg
		): base(
			arg
		){
			thisHeatDecayRate = arg.heatDecayRate;
			thisStateEngine = CreateStateEngine();
			thisInitialHeat = arg.initialHeat;
			thisHeat = thisInitialHeat;
			thisFollowSmoothTime = arg.followSmoothTime;
			
			thisInitialMaxHeat = arg.initialMaxHeat;
			thisMaxHeat = thisInitialMaxHeat;
			thisLevelUpTime = arg.levelUpTime;
		}
		IHeatManagerAdaptor thisHeatManagerAdaptor{
			get{
				return (IHeatManagerAdaptor)thisAdaptor;
			}
		}
		IHeatManagerStateEngine thisStateEngine;
		public bool IsCountingDown(){
			return thisStateEngine.IsCountingDown();
		}
		float thisHeatDecayRate;
		float thisInitialHeat;
		float thisInitialMaxHeat;
		IHeatManagerStateEngine CreateStateEngine(){
			HeatManagerStateEngine.IConstArg stateEngineArg = new HeatManagerStateEngine.ConstArg(
				this,
				thisHeatDecayRate,
				thisSlickBowShootingProcessFactory
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
		}
		public void InitializeHeat(){
			// thisHeatImage.UpdateHeat(thisHeat);
			ResetHeat();
		}
		public void TickAwayHeat(float delta){
			thisHeat -= delta;
			thisHeatImage.UpdateHeat(thisHeat);
			if(thisHeat <= 0f)
				EndGameplay();
		}
		void EndGameplay(){
			// StopRunningPauseProcess();
			thisGameplayWidget.EndGameplay();
		}
		IGameplayWidget thisGameplayWidget;
		public void SetGameplayWidget(IGameplayWidget widget){
			thisGameplayWidget = widget;
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
			if(!thisHeatImage.IsInCombo())
				thisHeatImage.UpdateHeat(thisHeat);
		}
		public float GetMaxHeat(){
			return thisMaxHeat;
		}
		float thisStandardComboTime{//combo time at minComboValue
			get{
				return thisHeatManagerAdaptor.GetStandardComboTime();
			}
		}
		float thisMaxComboTimeMultiplier{
			get{
				return thisHeatManagerAdaptor.GetMaxComboTimeMultiplier();
			}
		}
		float thisMaxComboValue{//0 to 1, 1 at full circle
			get{
				return thisHeatManagerAdaptor.GetMaxComboValue();
			}
		}
		float thisMinComboValue{
			get{
				return thisHeatManagerAdaptor.GetMinComboValue();
			}
		}
		float thisBowComboTimeMultiplier = 1f;
		
		float thisCurrentMaxComboMultiplier = 0f;
		public void ResetCurrentMaxComboMultiplier(){
			thisCurrentMaxComboMultiplier = 0f;
		}
		public float GetComboTime(float normalizedComboValue){
			float comboTimeMultiplier = CalcComboTimeMultiplier(normalizedComboValue);
			if(comboTimeMultiplier > thisCurrentMaxComboMultiplier)
				thisCurrentMaxComboMultiplier = comboTimeMultiplier;
			float comboValueRelativeToMin = normalizedComboValue/ thisMinComboValue;
				// 1 when min, multiple of min otherwise
			float result = comboValueRelativeToMin * thisStandardComboTime * thisCurrentMaxComboMultiplier;
			result *= thisBowComboTimeMultiplier;
			return result;
		}
		float CalcComboTimeMultiplier(float normalizedComboValue){
			float correctedComboValue = (normalizedComboValue - thisMinComboValue)/ thisMaxComboValue;
			return Mathf.Lerp(
				1f,
				thisMaxComboTimeMultiplier,
				correctedComboValue
			);
		}
		readonly float thisLevelUpTime;
		float thisLevelUpMultiplier{
			get{
				return thisHeatManagerAdaptor.GetLevelUpMultiplier();
			}
		}
		IHeatLevelText thisHeatLevelText;
		public void SetHeatLevelText(IHeatLevelText heatLevelText){
			thisHeatLevelText = heatLevelText;
		}
		void LevelUpHeat(){
			if(thisHeatLevel < thisMaxHeatLevel){
				ResetCurrentMaxComboMultiplier();
				thisHeatLevel ++;
				StartHeatLevelUpProcess();
				thisHeatLevelText.StartLevelUpTo(thisHeatLevel);
				SetAllTargetReserveTier(thisHeatLevel - 1);
				thisColorSchemeManager.ChangeColorScheme(thisHeatLevel - 1, 10f);
			}
		}
		int thisMaxHeatLevel = 3;
		int thisHeatLevel = 1;
		int thisInitHeatLevel = 1;
		void StartHeatLevelUpProcess(){
			float targetMaxHeat = thisMaxHeat * thisLevelUpMultiplier;
			IHeatLevelUpProcess process = thisSlickBowShootingProcessFactory.CreateHeatLevelUpProcess(
				this,
				targetMaxHeat,
				thisLevelUpTime
			);
			process.Run();

		}
		void SetAllTargetReserveTier(int tier){
			foreach(IShootingTargetReserve reserve in thisTargetReserves){
				reserve.SetTier(tier);
			}
		}
		IShootingTargetReserve[] thisTargetReserves;
		public void SetShootingTargetReserves(IShootingTargetReserve[] reserves){
			thisTargetReserves = reserves;
		}
		public void OnLevelUpExpire(){
			AddHeat(0f);
		}
		public void ResetHeat(){
			StopCountingDown();
			thisHeat = thisInitialHeat;
			SetMaxHeat(thisInitialMaxHeat);
			thisHeatLevel = thisInitHeatLevel;
			thisHeatLevelText.SetHeatLevelText(thisHeatLevel);
			SetAllTargetReserveTier(thisHeatLevel - 1);
		}

		IColorSchemeManager thisColorSchemeManager;
		public void SetColorSchemeManager(IColorSchemeManager manager){
			thisColorSchemeManager = manager;
		}
		/*  */
		public new interface IConstArg: SlickBowShootingSceneObject.IConstArg{
			float initialHeat{get;}
			float heatDecayRate{get;}
			float followSmoothTime{get;}

			float initialMaxHeat{get;}
			float levelUpTime{get;}
		}
		public new class ConstArg: SlickBowShootingSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IHeatManagerAdaptor adaptor,
				float initialHeat,
				float heatDecayRate,
				float followSmoothTime,

				float initialMaxHeat,
				float levelUpTime
			): base(
				adaptor
			){
				thisInitialHeat = initialHeat;
				thisHeatDecayRate = heatDecayRate;
				thisFollowSmoothTime = followSmoothTime;

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

			readonly float thisInitialMaxHeat;
			public float initialMaxHeat{get{return thisInitialMaxHeat;}}
			readonly float thisLevelUpTime;
			public float levelUpTime{get{return thisLevelUpTime;}}
		}
	}
}
