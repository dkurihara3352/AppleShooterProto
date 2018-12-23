using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace AppleShooterProto{
	public interface IPlayerDataManager: IAppleShooterSceneObject{
		/* File Management */
			string GetDirectory();
			string GetBaseFileName();
			void CreateNewPlayerDataFile(string fileName);
			string[] GetFilePaths();
			int GetFileIndex(string fileName);
			void SetFileIndex(int fileIndex);

			bool PlayerDataIsLoaded();
			void InitializePlayerData();
			void Load();
			void Save();
		/* Fields */
			int GetBowCount();
			int GetMaxAttributeLevel();
		/* Data Manipulation */
			int GetHighScore();
			void SetHighScore(int highScore);

			int GetCurrency();
			void SetCurrency(int currency);

			int GetEquippedBowIndex();
			void SetEquippedBow(int index);

			IBowConfigData[] GetBowConfigDataArray();
			void IncreaseAttributeLevel(int attributeIndex);
			void DecreaseAttributeLevel(int attributeIndex);

			void UnlockBow(int bowIndex);
			void LockBow(int bowIndex);
		/* Debug */
			string GetDebugString();
		/* Other */
			void ClearBowConfigData(int bowIndex);
	}
	public class PlayerDataManager : AppleShooterSceneObject, IPlayerDataManager {
		public PlayerDataManager(
			IConstArg arg
		): base(arg){
		}
		/* File Management */
			public void SetFileIndex(int index){
				thisFileIndex = index;
				Debug.Log("fileID is now " + thisFileIndex.ToString());
			}
			int thisFileIndex = 0;

			string thisDirectory{
				get{
					return UnityEngine.Application.persistentDataPath + "/PlayerData";
				}
			}
			public string GetDirectory(){
				return thisDirectory;
			}
			public string GetBaseFileName(){
				return "PlayerData_";
			}
			public string[] GetFilePaths(){
				string playerDataPath = GetDirectory();
				if(!System.IO.Directory.Exists(playerDataPath)){
					System.IO.Directory.CreateDirectory(playerDataPath);
				}
				string[] filePaths = System.IO.Directory.GetFiles(playerDataPath);	
				return filePaths;
			}
			string GetFilePathFromName(string fileName){
				return thisDirectory + "/" + fileName + ".dat";
			}
			public int GetFileIndex(string fileName){
				string targetFilePath = GetFilePathFromName(fileName);
				int index = 0;
				foreach(string filePath in GetFilePaths()){
					if(filePath == targetFilePath)
						return index;
					index ++;
				}
				return -1;
			}
			public void CreateNewPlayerDataFile(string fileName){
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new BinaryFormatter();
				string filePath = GetFilePathFromName(fileName);
				System.IO.FileStream file = System.IO.File.Create(
					filePath
				);
				InitializePlayerData();
				bf.Serialize(file, thisPlayerData);
				file.Close();
				Debug.Log("created at: " + filePath);
			}
			public void Save(){
				string path = GetFilePaths()[thisFileIndex];
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new BinaryFormatter();
				System.IO.FileStream file = System.IO.File.Create(
					path
				);
				if(thisPlayerData == null){
					InitializePlayerData();
				}
				bf.Serialize(file, thisPlayerData);
				thisPlayerData = null;
				file.Close();
				Debug.Log("fileID: " + thisFileIndex.ToString() + " is saved");
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
				string path = GetFilePaths()[thisFileIndex];
				if(System.IO.File.Exists(
					path
				)){
					System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new BinaryFormatter();
					System.IO.FileStream file = System.IO.File.Open(
						path,
						FileMode.Open
					);
					thisPlayerData = (IPlayerData)bf.Deserialize(file);
					file.Close();
					Debug.Log("fileID: " + thisFileIndex.ToString() + " is loaded");
				}else
					throw new System.InvalidOperationException(
						"no file by the path exists, need to save first"
					);
			}
		/* Fields */
			int thisBowCount = 3;
			public int GetBowCount(){
				return thisBowCount;
			}
			int thisMaxAttributeLevel = 5;
			public int GetMaxAttributeLevel(){
				return thisMaxAttributeLevel;
			}
		/* Data Manipulation */
			public void InitializePlayerData(){
				IPlayerData data = new PlayerData();
				data.SetHighScore(0);
				data.SetEquippedBowIndex(0);
				IBowConfigData[] bowConfigDataArray = CreateInitializedBowCofigDataArray();
				data.SetBowConfigDataArray(bowConfigDataArray);
				thisPlayerData = data;

				Debug.Log("fileID: " + thisFileIndex.ToString() + "'s playerData is init'ed");
			}
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
			public int GetHighScore(){
				if(PlayerDataIsLoaded())
					return thisPlayerData.GetHighScore();
				else
					throw new System.InvalidOperationException(
						"load first"
					);
			}
			public void SetHighScore(int score){
				if(PlayerDataIsLoaded())
					thisPlayerData.SetHighScore(score);
				else
					throw new System.InvalidOperationException(
						"load first"
					);
			}
			public int GetCurrency(){
				if(PlayerDataIsLoaded())
					return thisPlayerData.GetCurrency();
				else
					throw new System.InvalidOperationException(
						"load first"
					);

			}
			public void SetCurrency(int currency){
				if(PlayerDataIsLoaded())
					thisPlayerData.SetCurrency(currency);
				else
					throw new System.InvalidOperationException(
						"load first"
					);
			}
			public int GetEquippedBowIndex(){
				if(PlayerDataIsLoaded()){
					return thisPlayerData.GetEquippedBowIndex();
				}else
					throw new System.InvalidOperationException(
						"there's no player data"
					);
			}
			public void SetEquippedBow(int index){
				if(PlayerDataIsLoaded()){
					thisPlayerData.SetEquippedBowIndex(index);
					Debug.Log("bow " + index.ToString() + " is equipped");
				}
				else
					throw new System.InvalidOperationException(
						"no player data"
					);
			}
			public IBowConfigData[] GetBowConfigDataArray(){
				if(PlayerDataIsLoaded())
					return thisPlayerData.GetBowConfigDataArray();
				throw new System.InvalidOperationException(
					"no player data"
				);
			}
			public void IncreaseAttributeLevel(
				int attributeIndex
			){
				ChangeAttributeLevelByOne(attributeIndex, true);
			}
			public void DecreaseAttributeLevel(
				int attributeIndex
			){
				ChangeAttributeLevelByOne(attributeIndex, false);
			}
			void ChangeAttributeLevelByOne(
				int attributeIndex,
				bool increment
			){
				if(PlayerDataIsLoaded()){
					int equippedBowIndex = GetEquippedBowIndex();
					IBowConfigData configData = GetBowConfigDataArray()[equippedBowIndex];

					int[] attributeLevels = configData.GetAttributeLevels();
					int currentAttributeLevel = attributeLevels[attributeIndex];
					
					int newAttributeLevel = increment? currentAttributeLevel +1: currentAttributeLevel -1;
					if(newAttributeLevel > thisMaxAttributeLevel)
						newAttributeLevel = thisMaxAttributeLevel;
					else if(newAttributeLevel < 0)
						newAttributeLevel = 0;

					int[] newAttributeLevels = attributeLevels;
					newAttributeLevels[attributeIndex] = newAttributeLevel;

					configData.SetAttributeLevels(newAttributeLevels);

				}else
					throw new System.InvalidOperationException(
						"no player data"
					);
			}
			public void UnlockBow(int bowIndex){
				if(PlayerDataIsLoaded()){
					IBowConfigData configData = GetBowConfigDataArray()[bowIndex];
					configData.Unlock();
				}else
					throw new System.InvalidOperationException(
						"no player data"
					);
			}
			public void LockBow(int bowIndex){
				if(PlayerDataIsLoaded()){
					IBowConfigData configData = GetBowConfigDataArray()[bowIndex];
					configData.Lock();
				}else
					throw new System.InvalidOperationException(
						"no player data"
					);
			}
			
		/* Others */

			public void ClearBowConfigData(int bowIndex){
				IBowConfigData newData = new BowConfigData();
				
				GetBowConfigDataArray()[bowIndex] = newData;
			}
		/* Debug */
			public string GetDebugString(){
				string result = "";
				if(thisPlayerData != null){
					result += "HighScore: " + thisPlayerData.GetHighScore().ToString()+ "\n";
					result += "Currency: " + thisPlayerData.GetCurrency().ToString() + "\n";
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
