using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IScoreManagerAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		IScoreManager GetScoreManager();
	}
	public class ScoreManagerAdaptor : SlickBowShootingMonoBehaviourAdaptor, IScoreManagerAdaptor {

		public override void SetUp(){
			thisScoreManager = CreateScoreManager();
		}
		IScoreManager thisScoreManager;
		public IScoreManager GetScoreManager(){
			return thisScoreManager;
		}
		IScoreManager CreateScoreManager(){
			ScoreManager.IConstArg arg = new ScoreManager.ConstArg(
				this
			);
			return new ScoreManager(arg);
		}

		public override void SetUpReference(){
			base.SetUpReference();
			IScoreImage scoreImage = scoreImageAdaptor.GetScoreImage();
			thisScoreManager.SetScoreImage(scoreImage);
			IScoreImage highScoreImage = highScoreImageAdaptor.GetScoreImage();
			thisScoreManager.SetHighScoreImage(highScoreImage);
		}
		public ScoreImageAdaptor scoreImageAdaptor;
		public ScoreImageAdaptor highScoreImageAdaptor;
	}
}
