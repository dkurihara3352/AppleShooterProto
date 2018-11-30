using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IScoreImage: IAppleShooterSceneObject{
		void UpdateImage(int score);
	}
	public class ScoreImage : AppleShooterSceneObject, IScoreImage {
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
		public new interface IConstArg: AppleShooterSceneObject.IConstArg{}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IScoreImageAdaptor adaptor
			): base(
				adaptor
			){}
		}
	}
}
