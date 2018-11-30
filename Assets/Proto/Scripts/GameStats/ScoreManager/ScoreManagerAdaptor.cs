using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IScoreManagerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IScoreManager GetScoreManager();
	}
	public class ScoreManagerAdaptor : AppleShooterMonoBehaviourAdaptor, IScoreManagerAdaptor {

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
		}
		public ScoreImageAdaptor scoreImageAdaptor;
	}
}
