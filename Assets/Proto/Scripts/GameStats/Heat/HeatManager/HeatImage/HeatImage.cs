using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface IHeatImage: ISlickBowShootingSceneObject, IProcessHandler{

		void SetHeatManager(IHeatManager manager);


		void UpdateHeat(float heat);
		void AddHeat(float delta);
		void StartCountingDown();
		float GetComboWindowTime();
		void SetFollowImageHeat(float imageHeat);
		float GetFollowImageHeat();

		float GetComboValue();
		bool IsInCombo();
	}
	public class HeatImage : SlickBowShootingSceneObject, IHeatImage {
		public HeatImage(
			IConstArg arg
		): base(
			arg
		){
			thisComboProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				0f
			);
		}
		
		IHeatImageAdaptor thisTypedAdaptor{
			get{
				return (IHeatImageAdaptor)thisAdaptor;
			}
		}
		public void UpdateHeat(float heat){
			thisHeat = heat;
			SetFollowImageHeat(thisHeat);
			SetDeltaImageHeat(thisHeat);
		}
		float thisHeat;
		float thisFollowHeat;
		public void SetFollowImageHeat(float heat){
			thisFollowHeat = heat;
			float followFill = GetFill(heat);
			thisTypedAdaptor.UpdateMainHeatImageFill(followFill);
		}
		void SetDeltaImageHeat(float heat){
			float deltaFill = GetFill(heat);
			thisTypedAdaptor.UpdateDeltaHeatImageFill(deltaFill);
		}
		float GetFill(float heat){
			float maxHeat = thisHeatManager.GetMaxHeat();
			return heat/ maxHeat;
		}
		public float GetFollowImageHeat(){
			return thisFollowHeat;
		}
		public void AddHeat(float delta){
			thisInitFollowHeat = thisFollowHeat;
			thisHeat += delta;
			SetDeltaImageHeat(thisHeat);
			thisTargetFollowHeat = thisHeat;
			StartComboProcess();
		}
		/* Combo Process */
			IProcessSuite thisComboProcessSuite;
			float thisInitFollowHeat;
			float thisTargetFollowHeat;
			void StartComboProcess(){
				StopComboProcess();
				thisIsInCombo = true;
				thisComboProcessSuite.SetConstraintValue(GetComboWindowTime());
				thisComboProcessSuite.Start();
			}
			void StopComboProcess(){
				thisComboProcessSuite.Stop();
			}
			public void OnProcessRun(IProcessSuite suite){
				return;
			}
			public void OnProcessUpdate(
				float deltaTime,
				float normalizedTime,
				IProcessSuite suite
			){
				if(suite == thisComboProcessSuite){
					float newFollowHeat = Mathf.Lerp(
						thisInitFollowHeat,
						thisTargetFollowHeat,
						normalizedTime
					);
					SetFollowImageHeat(newFollowHeat);
				}
			}
			public void OnProcessExpire(IProcessSuite suite){
				thisIsInCombo = false;
				StartCountingDown();
				thisHeatManager.ResetCurrentMaxComboMultiplier();
			}
		/*  */
		IHeatManager thisHeatManager;
		public void SetHeatManager(IHeatManager manager){
			thisHeatManager = manager;;
		}
		public void StartCountingDown(){
			thisHeatManager.StartCountingDown();
		}

		public float GetComboWindowTime(){
			return thisHeatManager.GetComboTime(GetNormalizedComboValue());
		}
		public float GetComboValue(){
			return thisHeat - thisFollowHeat;
		}
		float GetNormalizedComboValue(){
			return GetComboValue()/ thisHeatManager.GetMaxHeat();
		}
		bool thisIsInCombo = false;
		public bool IsInCombo(){
			return thisIsInCombo;
		}
		/* Const */
			public new interface IConstArg: SlickBowShootingSceneObject.IConstArg{
			}
			public new class ConstArg: SlickBowShootingSceneObject.ConstArg, IConstArg{
				public ConstArg(
					IHeatImageAdaptor adaptor
				): base(adaptor){
				}
			}
	}
}
