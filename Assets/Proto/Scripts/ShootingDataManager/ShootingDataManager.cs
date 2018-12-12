﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingDataManager: IAppleShooterSceneObject{
		void SetPlayerDataManager(IPlayerDataManager playerDataManager);
		void CalculateShootingData();

		float GetFireRate();
		float GetDrawTime();
		float GetMinDrawStrength();
		float GetMaxDrawStrength();
		float GetCriticalMultiplier();
	}
	public class ShootingDataManager: AppleShooterSceneObject, IShootingDataManager{
		public ShootingDataManager(IConstArg arg): base(arg){}
		IPlayerDataManager thisPlayerDataManager;
		public void SetPlayerDataManager(IPlayerDataManager manager){
			thisPlayerDataManager = manager;
		}
		IShootingDataManagerAdaptor thisShootingDataManagerAdaptor{
			get{
				return (IShootingDataManagerAdaptor)thisAdaptor;
			}
		}
		public void CalculateShootingData(){
			thisMaxScaledLevel = CalculateMaxScaledLevel();

			int equippedBowIndex = thisPlayerDataManager.GetEquippedBowIndex();
			IBowConfigData bowConfigData = thisPlayerDataManager.GetBowConfigDataArray()[equippedBowIndex];
			int[] attributeLevelArray = bowConfigData.GetAttributeLevelArray();
			thisMinDrawStrength = CalculateMinDrawStrength(attributeLevelArray[0]);
			thisMaxDrawStrength = CalculateMaxDrawStrength(attributeLevelArray[0]);
			thisFireRate = CalculateFireRate(attributeLevelArray[1]);
			thisDrawTime = CalculateDrawTime(attributeLevelArray[1]);
			thisCritMultiplier = CalculateCriticalMultiplier(attributeLevelArray[2]);
		}
		/*  */
			int thisTierSteps{
				get{
					return thisShootingDataManagerAdaptor.GetTierSteps();
				}
			}
			int thisTierCount{
				get{
					return thisShootingDataManagerAdaptor.GetTierCount();
				}
			}
			int thisMaxLevel{
				get{
					return thisTierSteps * thisTierCount;
				}
			}
			int[] thisTierMultipliers{
				get{
					return thisShootingDataManagerAdaptor.GetTierLevelMultipliers();
				}
			}
			protected int GetLevelTier(int sourceLevel){
				return (sourceLevel - 1) / thisTierSteps;
			}
			protected int GetScaledLevel(int sourceLevel){
				int result = 0;
				for(int i = 1; i <= sourceLevel; i ++){
					int tier = GetLevelTier(i);
					result += thisTierMultipliers[tier];
				}
				return result;
			}
			int thisMaxScaledLevel;
			protected int CalculateMaxScaledLevel(){
				return GetScaledLevel(thisMaxLevel);
			}

			protected float GetNormalizedScaledLevel(int scaledLevel, int maxScaledLevel){
				if(scaledLevel == 0)
					return 0f;
				return (scaledLevel * 1f) / maxScaledLevel;
			}
		/* DrawStrength */
			float thisMinDrawStrength;
			public float GetMinDrawStrength(){
				return thisMinDrawStrength;
			}
			float[] thisGlobalMinDrawStrengthLimit{
				get{
					return thisShootingDataManagerAdaptor.GetGlobalMinDrawStrengthLimit();
				}
			}
			float CalculateMinDrawStrength(int strengthLevel/* or quickness?? */){
				int scaledLevel = GetScaledLevel(strengthLevel);
				float normalizedScaledLevel = GetNormalizedScaledLevel(scaledLevel, thisMaxScaledLevel);
				return Mathf.Lerp(
					thisGlobalMinDrawStrengthLimit[0],
					thisGlobalMinDrawStrengthLimit[1],
					normalizedScaledLevel
				);
			}
			float thisMaxDrawStrength;
			public float GetMaxDrawStrength(){
				return thisMaxDrawStrength;
			}
			float[] thisGlobalMaxDrawStrengthLimit{
				get{
					return thisShootingDataManagerAdaptor.GetGlobalMaxDrawStrengthLimit();
				}
			}
			float CalculateMaxDrawStrength(int strengthLevel){
				int scaledLevel = GetScaledLevel(strengthLevel);
				float normalizedScaledLevel = GetNormalizedScaledLevel(scaledLevel, thisMaxScaledLevel);
				return Mathf.Lerp(
					thisGlobalMaxDrawStrengthLimit[0],
					thisGlobalMaxDrawStrengthLimit[1],
					normalizedScaledLevel
				);
			}
		/* FireRate */
			float thisFireRate;
			public float GetFireRate(){
				return thisFireRate;
			}
			float[] thisGlobalFireRateLimit{
				get{
					return thisShootingDataManagerAdaptor.GetGlobalFireRateLimit();
				}
			}
			float CalculateFireRate(int quicknessLevel){
				int scaledLevel = GetScaledLevel(quicknessLevel);
				float normalizedScaledLevel = GetNormalizedScaledLevel(scaledLevel, thisMaxScaledLevel);
				return Mathf.Lerp(
					thisGlobalFireRateLimit[0],
					thisGlobalFireRateLimit[1],
					normalizedScaledLevel
				);
			}
		/* DrawTime */
			float thisDrawTime;
			public float GetDrawTime(){
				return thisDrawTime;
			}
			float[] thisGlobalDrawTimeLimit{
				get{
					return thisShootingDataManagerAdaptor.GetGlobalDrawTimeLimit();
				}
			}
			float CalculateDrawTime(int quicknessLevel){
				int scaledLevel = GetScaledLevel(quicknessLevel);
				float normalizedScaledLevel = GetNormalizedScaledLevel(scaledLevel, thisMaxScaledLevel);
				return Mathf.Lerp(
					thisGlobalDrawTimeLimit[0],
					thisGlobalDrawTimeLimit[1],
					normalizedScaledLevel
				);

			}
		/* Crit */
			float thisCritMultiplier;
			public float GetCriticalMultiplier(){
				return thisCritMultiplier;
			}
			float[] thisGlobalCriticalLimit{
				get{
					return thisShootingDataManagerAdaptor.GetGlobalCriticalLimit();
				}
			}
			float CalculateCriticalMultiplier(int criticalLevel){
				int scaledLevel = GetScaledLevel(criticalLevel);
				float normalizedScaledLevel = GetNormalizedScaledLevel(scaledLevel, thisMaxScaledLevel);
				return Mathf.Lerp(
					thisGlobalCriticalLimit[0],
					thisGlobalCriticalLimit[1],
					normalizedScaledLevel
				);
			}
		

		/*  */

		public new interface IConstArg: AppleShooterSceneObject.IConstArg{}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IShootingDataManagerAdaptor adaptor
			): base(
				adaptor
			){}
		}
	}
}
