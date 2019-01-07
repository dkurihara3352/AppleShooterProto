using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IScoreImage: ISlickBowShootingSceneObject{
		void UpdateImage(int score);
	}
	public class ScoreImage : SlickBowShootingSceneObject, IScoreImage {
		public ScoreImage(
			IConstArg arg
		): base(
			arg
		){
		}
		public void UpdateImage(int score){
			thisTypedAdaptor.SetText(score.ToString());
		}
		IScoreImageAdaptor thisTypedAdaptor{
			get{
				return (IScoreImageAdaptor)thisAdaptor;
			}
		}
		public new interface IConstArg: SlickBowShootingSceneObject.IConstArg{}
		public new class ConstArg: SlickBowShootingSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IScoreImageAdaptor adaptor
			): base(
				adaptor
			){}
		}
	}
}
