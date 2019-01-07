using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlickBowShooting{
	public interface IScoreImageAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		IScoreImage GetScoreImage();
		void SetText(string scoreText);
	}
	public class ScoreImageAdaptor : SlickBowShootingMonoBehaviourAdaptor, IScoreImageAdaptor {

		public override void SetUp(){
			thisScoreImage = CreateScoreImage();
		}
		IScoreImage thisScoreImage;
		public IScoreImage GetScoreImage(){
			return thisScoreImage;
		}
		public IScoreImage CreateScoreImage(){
			ScoreImage.IConstArg arg = new ScoreImage.ConstArg(
				this
			);
			return new ScoreImage(arg);
		}
		public Text scoreText;
		public void SetText(string text){
			scoreText.text = text;
		}
	}
}
