using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IBowDataCalculator{
		void CalcAttributeData();
		float GetAttributeValue(int level);
		float GetPrevAttributeValue(int level);
		float GetDeltaAttributeValue(int level);
		int CalcCost(int level, float coinDepreciationValue);
		float CalcCoinDepreciationValue(float totalAttributeValue);

		int GetAttributeLevel(int index);
		int GetBowLevel();
		float GetCurrentAttributeValueByIndex(int index);
		float GetTotalAttributeValue();
		int GetTotalSpentCoin();
		void IncrementAttributeLevel(
			int index,
			int spentCoin
		);
		void ClearAttribute();
	}
	public class BowDataCalculator: IBowDataCalculator{
		public BowDataCalculator(IAdaptor adaptor){
			thisAdaptor = adaptor;
			InitLevels();
			InitAttributeValues();
		}
		IAdaptor thisAdaptor;
		/*  */

		public void CalcAttributeData(){
			thisDeltaMultiplierArray = CalculateDeltaMultiplierArray();
			thisSumOfMultArray = CalcSumOfMultArray(thisDeltaMultiplierArray);
			thisBaseAttValue = 1f/ thisSumOfMultArray[thisSumOfMultArray.Length -1];
		}
		float[] thisDeltaMultiplierArray;
		float[] thisSumOfMultArray;
		float[] CalculateDeltaMultiplierArray(){
			List<float> resultList = new List<float>();
			for(int i = 0; i < thisMaxAttributeLevel + 1; i ++){
				if(i == 0)
					resultList.Add(0f);
				else{
					resultList.Add(Mathf.Pow(thisAttributeMultiplier, i - 1));
				}
			}
			return resultList.ToArray();
		}
		int thisMaxAttributeLevel{
			get{
				return thisAdaptor.GetMaxAttributeLevel();
			}
		}
		float thisAttributeMultiplier{
			get{
				return thisAdaptor.GetAttributeMultiplier();
			}
		}
		float thisBaseAttValue;
		float[] CalcSumOfMultArray(float[] deltaArray){
			List<float> resultList = new List<float>();
			float sum = 0f;
			foreach(float delta in deltaArray){
				sum += delta;
				resultList.Add(sum);
			}
			return resultList.ToArray();
		}


		public float GetAttributeValue(int level){
			return (thisBaseAttValue * thisSumOfMultArray[level]);
		}
		public float GetPrevAttributeValue(int level){
			if(level == 0)
				return 0f;
			else
				return GetAttributeValue(level - 1);
		}
		public float GetDeltaAttributeValue(int level){
			return GetAttributeValue(level) - GetPrevAttributeValue(level);
		}
		public int CalcCost(int level, float coinDepreciationValue){
			return Mathf.RoundToInt(GetDeltaAttributeValue(level)/ thisInitialValuePerCoin * coinDepreciationValue);
		}
		float thisInitialValuePerCoin{
			get{
				return thisBaseAttValue/ thisBaseCost;
			}
		}
		int thisBaseCost{
			get{
				return thisAdaptor.GetBaseCost();
			}
		}


		public float CalcCoinDepreciationValue(
			float attriValue
		){
			float normalizedAttriValue = attriValue/ thisMaxTotalAttriValue;
			return Mathf.Lerp(
				thisMinCoinDepreValue,
				thisMaxCoinDepreValue,
				normalizedAttriValue
			);
		}
		float thisMaxTotalAttriValue = 3f;
		float thisMinCoinDepreValue = 1f;
		float thisMaxCoinDepreValue{
			get{
				return thisAdaptor.GetMaxCoinDepreciationValue();
			}
		}


		public int GetAttributeLevel(int index){
			return thisLevels[index];
		}
		int[] thisLevels;
		public int GetBowLevel(){
			return thisLevels[0] + thisLevels[1] + thisLevels[2];
		}
		public float GetCurrentAttributeValueByIndex(int index){
			return thisAttributeValues[index];
		}
		float[] thisAttributeValues;
		public float GetTotalAttributeValue(){
			return 
				thisAttributeValues[0] +
				thisAttributeValues[1] +
				thisAttributeValues[2];
		}

		public void IncrementAttributeLevel(
			int index,
			int spentCoin
		){
			if(thisLevels[index] < thisMaxAttributeLevel){
				int newLevel = thisLevels[index] + 1;
				thisLevels[index] = newLevel;
				thisAttributeValues[index] = GetAttributeValue(newLevel);
				thisTotalSpentCoin += spentCoin;
			}
		}

		public int GetTotalSpentCoin(){
			return thisTotalSpentCoin;
		}
		int thisTotalSpentCoin;





		public void ClearAttribute(){
			thisBaseAttValue = 0f;
			thisTotalSpentCoin = 0;
			InitLevels();
			InitAttributeValues();
			CalcAttributeData();
		}
		void InitLevels(){
			thisLevels = new int[3]{0, 0, 0};
		}
		void InitAttributeValues(){
			thisAttributeValues = new float[3]{0f, 0f, 0f};
		}
		/*  */
		public interface IAdaptor{
			float GetAttributeMultiplier();//1.2
			int GetMaxAttributeLevel();//5
			int GetBaseCost();//10
			float GetMaxCoinDepreciationValue();//30
		}
	}
}

