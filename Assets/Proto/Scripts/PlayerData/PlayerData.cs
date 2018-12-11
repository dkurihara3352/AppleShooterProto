using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPlayerData{
		int GetHighScore();
		void SetHighScore(int score);
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
	}
}
