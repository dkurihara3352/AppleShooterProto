using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IColorSchemeManager: IAppleShooterSceneObject, IProcessHandler{
		void ChangeColorScheme(int index, float time);
	}
	public class ColorSchemeManager: AppleShooterSceneObject, IColorSchemeManager{
		public ColorSchemeManager(IConstArg arg): base(arg){
			thisChangeColorSchemeProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				0f
			);
		}
		IColorSchemeManagerAdaptor thisColorSchemeManagerAdaptor{
			get{
				return (IColorSchemeManagerAdaptor)thisAdaptor;
			}
		}
		public void ChangeColorScheme(
			int index,
			float time
		){
			SetInitColors();
			SetTargetColors(index);
			thisChangeColorSchemeProcessSuite.SetConstraintValue(time);
			thisChangeColorSchemeProcessSuite.Start();
		}
		IProcessSuite thisChangeColorSchemeProcessSuite;
		public void OnProcessRun(IProcessSuite suite){}
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisChangeColorSchemeProcessSuite){
				UpdateColorScheme(normalizedTime);
			}
		}
		public void OnProcessExpire(IProcessSuite suite){
			if(suite == thisChangeColorSchemeProcessSuite)
				UpdateColorScheme(1f);
		}
		void SetInitColors(){
			thisInitSkyColor = thisColorSchemeManagerAdaptor.GetSkyColor();
			thisInitEquatorColor = thisColorSchemeManagerAdaptor.GetEquatorColor();
			thisInitGroundColor = thisColorSchemeManagerAdaptor.GetGroundColor();
			thisInitFogColor = thisColorSchemeManagerAdaptor.GetFogColor();
		}
		Color thisInitSkyColor;
		Color thisInitEquatorColor;
		Color thisInitGroundColor;
		Color thisInitFogColor;

		void SetTargetColors(int index){
			thisTargetSkyColor = thisColorSchemeManagerAdaptor.GetSkyColorForTier(index);
			thisTargetEquatorColor = thisColorSchemeManagerAdaptor.GetEquatorColorForTier(index);
			thisTargetGroundColor = thisColorSchemeManagerAdaptor.GetGroundColorForTier(index);
			thisTargetFogColor = thisColorSchemeManagerAdaptor.GetFogColorForTier(index);
		}

		Color thisTargetSkyColor;
		Color thisTargetEquatorColor;
		Color thisTargetGroundColor;
		Color thisTargetFogColor;
		void UpdateColorScheme(float normalizedTime){
			Color newSkyColor = Color.Lerp(
				thisInitSkyColor,
				thisTargetSkyColor,
				normalizedTime
			);
			thisColorSchemeManagerAdaptor.SetSkyColor(newSkyColor);

			Color newEquatorColor = Color.Lerp(
				thisInitEquatorColor,
				thisTargetEquatorColor,
				normalizedTime
			);
			thisColorSchemeManagerAdaptor.SetEquatorColor(newEquatorColor);
			
			Color newGroundColor = Color.Lerp(
				thisInitGroundColor,
				thisTargetGroundColor,
				normalizedTime
			);
			thisColorSchemeManagerAdaptor.SetGroundColor(newGroundColor);
			
			Color newFogColor = Color.Lerp(
				thisInitFogColor,
				thisTargetFogColor,
				normalizedTime
			);
			thisColorSchemeManagerAdaptor.SetFogColor(newFogColor);
			thisColorSchemeManagerAdaptor.SetCameraBackgroundColor(newFogColor);
		}
	}
}

