using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingDataManager: IAppleShooterSceneObject{
		void SetPlayerDataManager(IPlayerDataManager playerDataManager);
		void SetBowDataCalculator(IBowDataCalculator calculator);
		void CalculateShootingData();

		float GetFireRate();
		float GetDrawTime();
		float GetMinDrawStrength();
		float GetMaxDrawStrength();
		float GetCriticalMultiplier();
	}
	public class ShootingDataManager: AppleShooterSceneObject, IShootingDataManager{
		public ShootingDataManager(IConstArg arg): base(arg){
			
		}
		IPlayerDataManager thisPlayerDataManager;
		public void SetPlayerDataManager(IPlayerDataManager manager){
			thisPlayerDataManager = manager;
		}
		IShootingDataManagerAdaptor thisShootingDataManagerAdaptor{
			get{
				return (IShootingDataManagerAdaptor)thisAdaptor;
			}
		}

		IBowDataCalculator thisCalculator;
		public void SetBowDataCalculator(IBowDataCalculator calculator){
			thisCalculator = calculator;
		}
		public void CalculateShootingData(){
			int equippedBowIndex = thisPlayerDataManager.GetEquippedBowIndex();
			IBowConfigData bowConfigData = thisPlayerDataManager.GetBowConfigDataArray()[equippedBowIndex];
			int[] attributeLevels = bowConfigData.GetAttributeLevels();
			float strengthValue = thisCalculator.GetAttributeValue(attributeLevels[0]);
			float quicknessValue = thisCalculator.GetAttributeValue(attributeLevels[1]);
			float criticalValue = thisCalculator.GetAttributeValue(attributeLevels[2]);
			thisMinDrawStrength = CalculateMinDrawStrength(strengthValue);
			thisMaxDrawStrength = CalculateMaxDrawStrength(strengthValue);
			thisFireRate = CalculateFireRate(quicknessValue);
			thisDrawTime = CalculateDrawTime(quicknessValue);
			thisCritMultiplier = CalculateCriticalMultiplier(criticalValue);
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
			float CalculateMinDrawStrength(float attributeValue){
				return Mathf.Lerp(
					thisGlobalMinDrawStrengthLimit[0],
					thisGlobalMinDrawStrengthLimit[1],
					attributeValue
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
			float CalculateMaxDrawStrength(float attributeValue){
				return Mathf.Lerp(
					thisGlobalMaxDrawStrengthLimit[0],
					thisGlobalMaxDrawStrengthLimit[1],
					attributeValue
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
			float CalculateFireRate(float attributeValue){
				return Mathf.Lerp(
					thisGlobalFireRateLimit[0],
					thisGlobalFireRateLimit[1],
					attributeValue
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
			float CalculateDrawTime(float attributeValue){
				return Mathf.Lerp(
					thisGlobalDrawTimeLimit[0],
					thisGlobalDrawTimeLimit[1],
					attributeValue
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
			float CalculateCriticalMultiplier(float attributeValue){
				return Mathf.Lerp(
					thisGlobalCriticalLimit[0],
					thisGlobalCriticalLimit[1],
					attributeValue
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
