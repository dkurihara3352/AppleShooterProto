using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBase{
	public interface IPool{
		int Draw();
		void Log();
		void ClearLog();
		int[] GetDrawResult();
		int[] GetDrawCountsByIndex();
	}
	public class Pool: IPool{
		public Pool(
			float[] relativeProbabilities
		){
			thisProgressiveProbabilities = CreateProgressiveProbabiilties(
				relativeProbabilities
			);
		}
		float[] thisProgressiveProbabilities;
		float[] CreateProgressiveProbabiilties(
			float[] relativeProbabilities
		){
			List<float> resultList = new List<float>();
			float sumOfRelativeProb = 0f;
			foreach(float relativeProb in relativeProbabilities){
				sumOfRelativeProb += relativeProb;
			}
			float sumOfProb = 0f;;
			foreach(float relativeProb in relativeProbabilities){
				float prob = relativeProb/ sumOfRelativeProb;
				sumOfProb += prob;
				resultList.Add(sumOfProb);
			}
			return resultList.ToArray();
		}
		public int Draw(){
			float randomValue = Random.Range(0f, 1f);
			int result = -1;
			int index = 0;
			foreach(float prob in thisProgressiveProbabilities){
				if(prob > randomValue){
					result = index;
					thisDrawResult.Add(result);
					return result;
				}
				index ++;
			}
			return thisProgressiveProbabilities.Length -1;
		}
		List<int> thisDrawResult = new List<int>();
		public int[] GetDrawResult(){
			return thisDrawResult.ToArray();
		}
		public int[] GetDrawCountsByIndex(){
			int[] drawCountsByIndex = new int[thisProgressiveProbabilities.Length];
			foreach(int draw in thisDrawResult){
				drawCountsByIndex[draw] += 1;
			}
			return drawCountsByIndex;
		}
		public void Log(){
			int[] drawCountsByIndex = GetDrawCountsByIndex();
			string overview = 
				"pool draw result: " +
				", total draw count: " + 
				thisDrawResult.Count.ToString();
			Debug.Log(overview);
			int index = 0;
			foreach(int drawCountByIndex in drawCountsByIndex){
				string result = 
					"entry: " + index.ToString() + ", " +
					"draws: " + drawCountByIndex.ToString() + ", " +
					"prob: " + ((drawCountByIndex * 1f) / thisDrawResult.Count).ToString();
				
				index++;
				Debug.Log(result);
			}
		}
		public void ClearLog(){
			thisDrawResult.Clear();
		}
	}

}

