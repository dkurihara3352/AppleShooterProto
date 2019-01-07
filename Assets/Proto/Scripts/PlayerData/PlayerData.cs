using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IPlayerData{
		int GetHighScore();
		void SetHighScore(int score);

		int GetEquippedBowIndex();
		void SetEquippedBowIndex(int index);
		IBowConfigData[] GetBowConfigDataArray();
		void SetBowConfigDataArray(IBowConfigData[] array);

		int GetCurrency();
		void SetCurrency(int currency);
		int[] GetBowUnlockCostArray();
		void SetBowUnlockCostArray(int[] array);
		

		bool GetAxisInversion(int axis);
		void SetAxisInversion(int axis, bool inverts);

		float GetBGMVolume();
		void SetBGMVolume(float volume);
		float GetSFXVolume();
		void SetSFXVolume(float volume);
		bool TutorialIsDone();
		void SetTutorialIsDone();
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
		int[] thisBowUnlockCostArray;
		public int[] GetBowUnlockCostArray(){
			return thisBowUnlockCostArray;
		}
		public void SetBowUnlockCostArray(int[] array){
			thisBowUnlockCostArray = array;
		}

		bool thisInvertsHorizontally = false;
		bool thisInvertsVertically = false;
		public bool GetAxisInversion(int axis){
			if(axis == 0)
				return thisInvertsHorizontally;
			else
				return thisInvertsVertically;
		}
		public void SetAxisInversion(int axis, bool value){
			if(axis == 0)
				thisInvertsHorizontally = value;
			else
				thisInvertsVertically = value;
		}

		float thisBGMVolume = 1f;
		public float GetBGMVolume(){
			return thisBGMVolume;
		}
		public void SetBGMVolume(float volume){
			thisBGMVolume = volume;
		}
		float thisSFXVolume = 1f;
		public float GetSFXVolume(){
			return thisSFXVolume;
		}
		public void SetSFXVolume(float volume){
			thisSFXVolume = volume;
		}

		bool thisTutorialIsDone = false;
		public bool TutorialIsDone(){
			return thisTutorialIsDone;
		}
		public void SetTutorialIsDone(){
			thisTutorialIsDone = true;
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
