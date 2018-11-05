using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IBellCurve{
		float Evaluate();
		void Log();
	}
	public class BellCurve: IBellCurve{
		public BellCurve(
			float mean,
			float standardDeviation,
			float min,
			float max,
			int resolution
		){
			thisMean = mean;
			thisSV = standardDeviation;
			thisMin = min;
			thisMax = max;
			thisResolution = resolution;
			thisEntries = CalculateEntries();
			float[] probabilityTable = CalculateProbabilities();
			thisPool = new Pool(probabilityTable);
		}
		float thisMean;
		float thisSV;
		float thisMin;
		float thisMax;
		int thisResolution;
		float[] thisEntries;
		float[] CalculateEntries(){
			List<float> resultList = new List<float>();
			for(int i = 0; i < thisResolution; i ++){
				float normalizedIndex;
				if(i == 0)
					normalizedIndex = 0f;
				else
					normalizedIndex = (i * 1f)/ (thisResolution - 1);
				float entry = Mathf.Lerp(
					thisMin,
					thisMax,
					normalizedIndex
				);
				resultList.Add(entry);
			}
			return resultList.ToArray();
		}
		float[] CalculateProbabilities(){
			List<float> resultList = new List<float>();
			term_0 = CalculateTerm0();
			for(int i = 0; i < thisResolution; i++){
				float prob = CalculateProbability(thisEntries[i]);
				resultList.Add(prob);
			}
			float sumOfProb = 0f;
			foreach(float prob in resultList){
				sumOfProb += prob;
			}

			float remain = 1f - sumOfProb;
			float adjustDelta = remain/ resultList.Count;
			List<float> adjusted = new List<float>();

			foreach(float prob in resultList){
				float adjustedProb = prob + adjustDelta;
				adjusted.Add(adjustedProb);
			}
			return adjusted.ToArray();
		}
		float term_0;
		float CalculateProbability(float entry){
			float term_1 = CalculateTerm1(entry);
			return term_0 - term_1;
		}
		float CalculateTerm0(){
			float e = Mathf.Exp(1);
			float pi = Mathf.PI;
			float rootTarget = 2f * thisSV * thisSV * pi;
			float rooted = Mathf.Pow(rootTarget, .5f);
			return e / rooted;
		}
		float CalculateTerm1(float entry){
			float numerator = (entry - thisMean) * (entry - thisMean);
			float denominator = (2f * thisSV) * (2f * thisSV);
			return numerator/ denominator;
		}
		readonly IPool thisPool;
		public float Evaluate(){
			int index = thisPool.Draw();
			return thisEntries[index];
		}
		public void Log(){
			int[] drawCountsByIndex = thisPool.GetDrawCountsByIndex();
			int drawCount = thisPool.GetDrawResult().Length;
			int index = 0;
			foreach(int draws in drawCountsByIndex){
				float entry = thisEntries[index];
				float prob;
					if(draws == 0)
						prob = 0f;
					else
						prob = draws / (drawCount * 1f);
					prob *= 100f;
				Debug.Log(
					"entry: " + entry.ToString() + ", " +
					"draws: " + draws.ToString() + ", " +
					"prob: " + prob.ToString() + " %"
				);
				index ++; 
			}
		}
		
	}
}
