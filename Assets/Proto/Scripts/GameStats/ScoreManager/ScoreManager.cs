using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface IScoreManager: IAppleShooterSceneObject{
		void SetScoreImage(IScoreImage image);
		void AddScore(int score);
		void ClearScore();
		int GetScore();

		/* HighScore */
		void SetHighScoreImage(IScoreImage image);
		void SetHighScore(int score);
		int GetHighScore();
	}
	public class ScoreManager : AppleShooterSceneObject, IScoreManager {

		public ScoreManager(
			IConstArg arg
		): base(
			arg
		){}
		IScoreImage thisScoreImage;
		public void SetScoreImage(IScoreImage image){
			thisScoreImage = image;
		}
		
		int thisScore = 0;
		public int GetScore(){
			return thisScore;
		}
		public void AddScore(int score){
			thisScore += score;
			thisScoreImage.UpdateImage(thisScore);
		}
		public void ClearScore(){
			thisScore = 0;
			thisScoreImage.UpdateImage(thisScore);
			thisHighScore = 0;
			thisHighScoreImage.UpdateImage(thisHighScore);
		}
		/* highScore */
		IScoreImage thisHighScoreImage;
		int thisHighScore;
		public void SetHighScoreImage(IScoreImage image){
			thisHighScoreImage = image;
		}
		public void SetHighScore(int score){
			thisHighScore = score;
			thisHighScoreImage.UpdateImage(score);
		}
		public int GetHighScore(){
			return thisHighScore;
		}

		public new interface IConstArg: AppleShooterSceneObject.IConstArg{}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IScoreManagerAdaptor adaptor
			): base(
				adaptor
			){

			}
		}
	}
}

