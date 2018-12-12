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
	}

	public interface IBowConfigData{
		bool IsUnlocked();
		void Unlock();
		int GetBowLevel();
		void SetBowLevel(int level);
		int[] GetAttributeLevelArray();
		void SetAttributeLevelArray(int[] array);
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
		int thisBowLevel = 0;
		public int GetBowLevel(){
			return thisBowLevel;
		}
		public void SetBowLevel(int level){
			thisBowLevel = level;
		}
		int[] thisAttributeLevelsArray = new int[3]{0, 0, 0};
		public int[] GetAttributeLevelArray(){
			return thisAttributeLevelsArray;
		}
		public void SetAttributeLevelArray(int[] array){
			thisAttributeLevelsArray = array;
		}
	}
}
