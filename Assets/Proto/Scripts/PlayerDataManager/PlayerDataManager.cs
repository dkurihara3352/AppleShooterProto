using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace AppleShooterProto{
	public interface IPlayerDataManager: IAppleShooterSceneObject{
		void SetFileIndex(int fileIndex);
		void InitializePlayerData();
		void Load();
		void Save();
		int GetHighScore();
		void SetHighScore(int highScore);
		void SwitchEquippedBow(int index);
		int GetEquippedBowIndex();
		void SetEquippedBow(int index);
		bool PlayerDataIsLoaded();
		IBowConfigData[] GetBowConfigDataArray();


		int GetTierSteps();
		int GetTierCount();
		int[] GetTierLevelMultipliers();

		void IncrementBowLevel(int bowIndex, int attributeIndex);
		void ClearBowConfigData(int bowIndex);
		
		string GetDebugString();
		int GetMaxScaledLevel();
	}
	public class PlayerDataManager : AppleShooterSceneObject, IPlayerDataManager {
		public PlayerDataManager(
			IConstArg arg
		): base(arg){

		}
		public void SetFileIndex(int index){
			thisFileIndex = index;
		}
		int thisFileIndex = 0;
		string thisFilePath{
			get{
				return UnityEngine.Application.persistentDataPath + "/playerData" + thisFileIndex.ToString() + ".dat";
			}
		}
		public void Save(){
			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new BinaryFormatter();
			System.IO.FileStream file = System.IO.File.Create(
				thisFilePath
			);
			if(thisPlayerData == null){
				InitializePlayerData();
			}
			bf.Serialize(file, thisPlayerData);
			thisPlayerData = null;
			file.Close();
		}
		IPlayerDataManagerAdaptor thisPlayerDataManagerAdaptor{
			get{
				return (IPlayerDataManagerAdaptor)thisAdaptor;
			}
		}
		IPlayerData thisPlayerData;
		public bool PlayerDataIsLoaded(){
			return thisPlayerData != null;
		}
		public void Load(){
			if(System.IO.File.Exists(
				thisFilePath
			)){
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new BinaryFormatter();
				System.IO.FileStream file = System.IO.File.Open(
					thisFilePath,
					FileMode.Open
				);
				thisPlayerData = (IPlayerData)bf.Deserialize(file);
				file.Close();
			}else
				throw new System.InvalidOperationException(
					"no file by the path exists, need to save first"
				);
		}
		public int GetHighScore(){
			if(thisPlayerData != null)
				return thisPlayerData.GetHighScore();
			else
				throw new System.InvalidOperationException(
					"load first"
				);
		}
		public void SetHighScore(int score){
			if(thisPlayerData != null)
				thisPlayerData.SetHighScore(score);
			else
				new System.InvalidOperationException(
					"load first"
				);
		}
		public void InitializePlayerData(){
			IPlayerData data = new PlayerData();
			data.SetHighScore(0);
			data.SetEquippedBowIndex(0);
			IBowConfigData[] bowConfigDataArray = CreateInitializedBowCofigDataArray();
			data.SetBowConfigDataArray(bowConfigDataArray);
			thisPlayerData = data;
		}
		int thisBowCount = 3;
		IBowConfigData[] CreateInitializedBowCofigDataArray(){
			IBowConfigData[] result = new IBowConfigData[thisBowCount];
			for(int i = 0; i < thisBowCount; i ++){
				IBowConfigData configData = new BowConfigData();
				int[] attributeLevelArray = new int[]{0, 0, 0};
				configData.SetAttributeLevels(attributeLevelArray);

				result[i] = configData;
			}
			result[0].Unlock();
			return result;

		}
		public string GetDebugString(){
			string result = "";
			if(thisPlayerData != null){
				result += "HighScore: " + thisPlayerData.GetHighScore().ToString()+ "\n";
				result += "EqpBow: "+ thisPlayerData.GetEquippedBowIndex().ToString() + "\n";
				result += GetBowConfigDataArrayString(thisPlayerData.GetBowConfigDataArray());
			}else{
				result += "data is null";
			}
			return result;
		}
		string GetBowConfigDataArrayString(IBowConfigData[] array){
			string result = "";
			if(array != null){
				result += "bowConfigData: " + "\n";
				foreach(IBowConfigData data in array){
					result += "\t";
					result += "unlocked: " + data.IsUnlocked().ToString() + ", ";
					result += "bowLevel: " + data.GetBowLevel().ToString() + ", ";
					result += "attLevel: " + DKUtility.DebugHelper.GetIndicesString(data.GetAttributeLevels());
					result += "\n";
				}

			}else{
				result += "array is null";
			}
			return result;
		}
		public void SwitchEquippedBow(int index){
			if(thisPlayerData != null){
				thisPlayerData.SetEquippedBowIndex(index);
			}
		}
		public int GetEquippedBowIndex(){
			if(thisPlayerData != null){
				return thisPlayerData.GetEquippedBowIndex();
			}else
				throw new System.InvalidOperationException(
					"there's no player data"
				);
		}
		public void SetEquippedBow(int index){
			if(thisPlayerData != null)
				thisPlayerData.SetEquippedBowIndex(index);
			else
				throw new System.InvalidOperationException(
					"no player data"
				);
		}
		public IBowConfigData[] GetBowConfigDataArray(){
			if(thisPlayerData != null)
				return thisPlayerData.GetBowConfigDataArray();
			throw new System.InvalidOperationException(
				"no player data"
			);
		}
		/*  */
		public int GetTierSteps(){
			return thisPlayerDataManagerAdaptor.GetTierSteps();
		}
		public int GetTierCount(){
			return thisPlayerDataManagerAdaptor.GetTierCount();
		}
		public int[] GetTierLevelMultipliers(){
			return thisPlayerDataManagerAdaptor.GetTierLevelMultipliers();
		}
		/*  */
		public void IncrementBowLevel(int bowIndex, int attributeIndex){
			
			IBowConfigData data = GetBowConfigDataArray()[bowIndex];
			int currentLevel = data.GetBowLevel();
			if(currentLevel < thisBowMaxLevel){
				int deltaAttributeLevel = GetDeltaScaledLevel(currentLevel + 1);

				int[] newAttributeLevelArray = data.GetAttributeLevels();
				newAttributeLevelArray[attributeIndex] += deltaAttributeLevel;

				data.SetAttributeLevels(newAttributeLevelArray);
				data.SetBowLevel(currentLevel + 1);
			}
		}
		public void ClearBowConfigData(int bowIndex){
			IBowConfigData newData = new BowConfigData();
			
			GetBowConfigDataArray()[bowIndex] = newData;
		}
		int thisBowMaxLevel{
			get{
				return GetTierSteps() * GetTierCount();
			}
		}
		protected int GetLevelTier(int sourceLevel){
			return (sourceLevel - 1) / GetTierSteps();
		}
		protected int GetScaledLevel(int sourceLevel){
			int result = 0;
			for(int i = 1; i <= sourceLevel; i ++){
				int tier = GetLevelTier(i);
				result += GetTierLevelMultipliers()[tier];
			}
			return result;
		}
		public int GetMaxScaledLevel(){
			return GetScaledLevel(thisBowMaxLevel);
		}
		public int GetDeltaScaledLevel(int level){
			int tier = GetLevelTier(level);
			return GetTierLevelMultipliers()[tier];
		}
		/*  */
		public new interface IConstArg: AppleShooterSceneObject.IConstArg{

		}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IPlayerDataManagerAdaptor adaptor
			): base(
				adaptor
			){}
		}
	}
}
