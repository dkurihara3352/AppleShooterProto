using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DKUtility{
	public static class Calculator{
		public static  void CalcSineAndCosine(Vector2 deltaPos, out float sine, out float cosine){
			Vector3 vecA = Vector3.right;
			Vector3 vecB = new Vector3(deltaPos.x, deltaPos.y, 0f);
			Vector3 crossP = Vector3.Cross(vecA, vecB);
			float deltaMag = deltaPos.magnitude;
			float sin = crossP.magnitude / deltaMag;
			if(deltaPos.y < 0f)
				sin *= -1f;
			float cos = Vector2.Dot(Vector2.right, deltaPos) / deltaMag;

			sine = sin;
			cosine = cos;
		}
		public static int[] GetRandomIntegers(
			int count, 
			int[] pool
		){
			int[] result = new int[count];
			int counter = 0;
			int[] unused = pool;
			while(counter < count){
				int randomInt = GetRandomInt(
					ref unused
				);
				result[counter] = randomInt;
				counter ++;
			}
			return result;
		}
		public static int[] GetRandomIntegers(
			int count,
			int maxNumber
 		){
			 int[] pool = new int[maxNumber + 1];
			 pool[0] = 0;
			 for(int i = 1; i < maxNumber + 1; i ++){
				 pool[i] = i;
			 }
			 return GetRandomIntegers(count, pool);
		 }
		static int GetRandomInt(
			ref int[] unused
		){
			int randomIndex = Random.Range(0, unused.Length);
			int result = unused[randomIndex];
			int[] newUnused = CreateNewUnused(unused, result);
			unused = newUnused;
			return result;
		}
		static int[] CreateNewUnused(
			int[] unused,
			int used
		){
			List<int> result = new List<int>();
			foreach(int i in unused){
				if(i != used){
					result.Add(i);
				}
			}
			return result.ToArray();
		}
	}
}
