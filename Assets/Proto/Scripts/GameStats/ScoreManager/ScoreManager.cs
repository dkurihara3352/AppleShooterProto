using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface IScoreManager: IAppleShooterSceneObject{
		void SetScoreImage(IScoreImage image);
		void AddScore(int score);
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
		public void AddScore(int score){
			thisScore += score;
			thisScoreImage.UpdateImage(thisScore);
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

