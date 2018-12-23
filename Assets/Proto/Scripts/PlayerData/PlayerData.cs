using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPlayerData{
		int GetHighScore();
		void SetHighScore(int score);

		int GetEquippedBowIndex();
		void SetEquippedBowIndex(int index);
		IBowConfigData[] GetBowConfigDataArray();
		void SetBowConfigDataArray(IBowConfigData[] array);


		int GetCurrency();
		void SetCurrency(int currency);
	}
	[System.Serializable]
	public class PlayerData : IPlayerData {
		int thisHighScore;
		public int GetHighScore(){
			return thisHighScore;
		}
		public void SetHighScore(int score){
			thisHighScore = score;
		}
		
		int thisEquippedBowIndex;
		public int GetEquippedBowIndex(){
			return thisEquippedBowIndex;
		}
		public void SetEquippedBowIndex(int index){
			thisEquippedBowIndex = index;
		}

		IBowConfigData[] thisBowConfigDataArray;
		public IBowConfigData[] GetBowConfigDataArray(){
			return thisBowConfigDataArray;
		}
		public void SetBowConfigDataArray(IBowConfigData[] array){
			thisBowConfigDataArray = array;
		}

		int thisCurrency;
		public int GetCurrency(){
			return thisCurrency;
		}
		public void SetCurrency(int currency){
			thisCurrency = currency;
		}
	}

	public interface IBowConfigData{
		bool IsUnlocked();
		void Unlock();
		void Lock();
		int GetBowLevel();
		int[] GetAttributeLevels();
		void SetAttributeLevels(int[] array);
	}
	[System.Serializable]
	public class BowConfigData: IBowConfigData{
		bool thisIsUnlocked = false;
		public bool IsUnlocked(){
			return thisIsUnlocked;
		}
		public void Unlock(){
			thisIsUnlocked = true;
		}
		public void Lock(){
			thisIsUnlocked = false;
		}
		public int GetBowLevel(){
			int result = 0;
			foreach(int level in thisAttributeLevels)
				result += level;
			return result;
		}
		
		int[] thisAttributeLevels = new int[3]{0, 0, 0};
		public int[] GetAttributeLevels(){
			return thisAttributeLevels;
		}
		public void SetAttributeLevels(int[] array){
			thisAttributeLevels = array;
		}
	}
}
