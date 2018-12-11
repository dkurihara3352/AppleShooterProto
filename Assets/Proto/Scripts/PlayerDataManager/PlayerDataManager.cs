using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace AppleShooterProto{
	public interface IPlayerDataManager: IAppleShooterSceneObject{
		void Load();
		void Save();
		int GetHighScore();
		void SetHighScore(int highScore);
	}
	public class PlayerDataManager : AppleShooterSceneObject, IPlayerDataManager {
		public PlayerDataManager(
			IConstArg arg
		): base(arg){

		}
		string thisFilePath = UnityEngine.Application.persistentDataPath + "/playerData.dat";
		public void Save(){
			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new BinaryFormatter();
			System.IO.FileStream file = System.IO.File.Create(
				thisFilePath
			);
			if(thisPlayerData == null)
				thisPlayerData = new PlayerData();
			bf.Serialize(file, thisPlayerData);
			thisPlayerData = null;
			file.Close();
			Debug.Log(
				"file saved: " + 
				UnityEngine.Application.persistentDataPath
			);
		}
		IPlayerData thisPlayerData;
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
			Debug.Log(
				"file loaded: " + 
				UnityEngine.Application.persistentDataPath
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
