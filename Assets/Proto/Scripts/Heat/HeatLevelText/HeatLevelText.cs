using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IHeatLevelText: IAppleShooterSceneObject, IProcessHandler{
		void SetHeatLevelText(int level);
		void StartLevelUpTo(int newLevel);
	}
	public class HeatLevelText : AppleShooterSceneObject, IHeatLevelText {
		public HeatLevelText(IConstArg arg): base(arg){
			thisLevelUpProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisHeatLevelTextAdaptor.GetLevelUpProcessTime()
			);
		}
		IHeatLevelTextAdaptor thisHeatLevelTextAdaptor{
			get{
				return (IHeatLevelTextAdaptor)thisAdaptor;
			}
		}
		public void SetHeatLevelText(int level){
			thisHeatLevelTextAdaptor.SetText(level.ToString());
		}
		IProcessSuite thisLevelUpProcessSuite;
		public void StartLevelUpTo(int newLevel){
			CacheNewLevel(newLevel);
			thisLevelUpProcessSuite.Start();
		}
		int thisCachedNewLevel;
		void CacheNewLevel(int newLevel){
			thisCachedNewLevel = newLevel;
		}
		public void OnProcessRun(IProcessSuite suite){}
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisLevelUpProcessSuite){
				AnimationCurve scaleValueCurve = thisHeatLevelTextAdaptor.GetScaleValueCurve();
				float scaleValue = scaleValueCurve.Evaluate(normalizedTime);
				thisHeatLevelTextAdaptor.SetScale(scaleValue);
				if(thisHeatLevelTextAdaptor.GetTextSwapPoint() < normalizedTime)
					SetHeatLevelText(thisCachedNewLevel);
			}
		}
		public void OnProcessExpire(IProcessSuite suite){
			thisHeatLevelTextAdaptor.SetScale(1f);
		}
		public new interface IConstArg: AppleShooterSceneObject.IConstArg{}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IHeatLevelTextAdaptor adaptor
			): base(adaptor){
			}
		}
	}
}
