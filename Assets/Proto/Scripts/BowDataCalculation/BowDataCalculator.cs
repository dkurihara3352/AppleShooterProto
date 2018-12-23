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
	}
	public class BowDataCalculator: IBowDataCalculator{
		public BowDataCalculator(IAdaptor adaptor){
			thisAdaptor = adaptor;
			CalcAttributeData();
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
		/*  */
		public interface IAdaptor{
			float GetAttributeMultiplier();//1.2
			int GetMaxAttributeLevel();//5
			int GetBaseCost();//10
			float GetMaxCoinDepreciationValue();//30
		}
	}
}

