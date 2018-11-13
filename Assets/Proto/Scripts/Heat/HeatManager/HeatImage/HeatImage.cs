using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IHeatImage: ISceneObject{

		void SetHeatManager(IHeatManager manager);


		void UpdateHeat(float heat);
		void AddHeat(float delta);
		void StartCountingDown();
		void SetInitialHeat(float initHeat);

		void StartSmoothFollowDeltaImageProcess();
		void StartWaitForNextAdditionProcess();
		void StopAllProcess();

		void AddHeatImple(float delta);
		float GetComboWindowTime();
		void SetFollowImageHeat(float imageHeat);
		float GetFollowImageHeat();

		float GetComboValue();
	}
	public class HeatImage : AbsSceneObject, IHeatImage {
		public HeatImage(
			IConstArg arg
		): base(
			arg
		){
			StateEngine.IConstArg engineArg = new StateEngine.ConstArg(this);
			thisStateEngine = new StateEngine(engineArg);
		}
		IStateEngine thisStateEngine;
		
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
		public void SetInitialHeat(float heat){
			thisHeat = heat;
			thisFollowHeat = thisHeat;
		}
		public void AddHeat(float delta){
			thisStateEngine.AddHeat(delta);
		}
		public void AddHeatImple(float delta){
			thisHeat += delta;
			float deltaFill = GetFill(thisHeat);
			thisTypedAdaptor.UpdateDeltaHeatImageFill(deltaFill);
		}
		IHeatManager thisHeatManager;
		public void SetHeatManager(IHeatManager manager){
			thisHeatManager = manager;;
		}
		public void StartCountingDown(){
			// thisStateEngine.StartCountingDown();
			thisHeatManager.StartCountingDown();
		}
		IHeatImageSmoothFollowDeltaImageProcess thisSmoothFollowDeltaImageProcess;
		public void StartSmoothFollowDeltaImageProcess(){
			StopAllProcess();
			float followSmoothTime = thisHeatManager.GetFollowSmoothTime();
			thisSmoothFollowDeltaImageProcess = thisProcessFactory.CreateHeatImageSmoothFollowDeltaImageProcess(
				this,
				followSmoothTime,
				thisHeat
			);
			thisSmoothFollowDeltaImageProcess.Run();
		}
		public void StopAllProcess(){
			StopSmoothFollowDeltaImageProcess();
			StopWaitForNextAdditionProcess();
		}
		void StopSmoothFollowDeltaImageProcess(){
			if(thisSmoothFollowDeltaImageProcess != null)
				thisSmoothFollowDeltaImageProcess.Stop();
			thisSmoothFollowDeltaImageProcess = null;
		}
		IHeatImageWaitForNextAdditionProcess thisWaitProcess;
		public void StartWaitForNextAdditionProcess(){
			StopAllProcess();
			float comboTime = GetComboWindowTime();
			thisWaitProcess = thisProcessFactory.CreateHeatImageWaitForNextAdditionProcess(
				this,
				comboTime
			);
			thisWaitProcess.Run();
		}
		void StopWaitForNextAdditionProcess(){
			if(thisWaitProcess != null)
				thisWaitProcess.Stop();
			thisWaitProcess = null;
		}
		public float GetComboWindowTime(){
			return thisHeatManager.GetComboWindowTime();
		}
		public float GetComboValue(){
			return thisHeat - thisFollowHeat;
		}
		/* StateEngine */
			interface IStateEngine: ISwitchableStateEngine<StateEngine.IState>{
				void AddHeat(float delta);
				void StartCountingDown();

				void SwitchToWaitingForAdditionState();
				void SwitchToWaitingForComboExpireState();
			}
			class StateEngine: AbsSwitchableStateEngine<StateEngine.IState>, IStateEngine{
				public StateEngine(IConstArg arg){
					thisHeatImage = arg.heatImage;
					AbsState.IConstArg stateArg = new AbsState.ConstArg(
						thisHeatImage,
						this
					);
					thisWaitingForAdditionState = new WaitingForAdditionState(
						stateArg
					);
					thisWaitingForComboExpireState = new WaitingForComboExpireState(
						stateArg
					);
					thisCurState = thisWaitingForAdditionState;
				}
				IHeatImage thisHeatImage;
				readonly IWaitingForAdditionState thisWaitingForAdditionState;
				readonly IWaitingForComboExpireState thisWaitingForComboExpireState;
				public void SwitchToWaitingForAdditionState(){
					TrySwitchState(thisWaitingForAdditionState);
				}
				public void SwitchToWaitingForComboExpireState(){
					TrySwitchState(thisWaitingForComboExpireState);
				}
				public void AddHeat(float delta){
					thisCurState.AddHeat(delta);
				}
				public void StartCountingDown(){
					SwitchToWaitingForAdditionState();
				}
				
				/* States */
					public interface IState: ISwitchableState{
						void AddHeat(float delta);
					}
					public abstract class AbsState: IState{
						public AbsState(
							IConstArg arg
						){
							thisHeatImage = arg.heatImage;
							thisEngine = arg.engine;
						}
						protected readonly IHeatImage thisHeatImage;
						protected readonly IStateEngine thisEngine;
						public abstract void OnEnter();
						public virtual void OnExit(){}
						public abstract void AddHeat(float delta);
						public interface IConstArg{
							IHeatImage heatImage{get;}
							IStateEngine engine{get;}
						}
						public struct ConstArg: IConstArg{
							public ConstArg(
								IHeatImage heatImage,
								IStateEngine engine
							){
								thisHeatImage = heatImage;
								thisEngine = engine;
							}
							readonly IHeatImage thisHeatImage;
							public IHeatImage heatImage{get{return thisHeatImage;}}
							readonly IStateEngine thisEngine;
							public IStateEngine engine{get{return thisEngine;}}
						}
					}
					public interface IWaitingForAdditionState: IState{
					}
					public class WaitingForAdditionState: AbsState, IWaitingForAdditionState{
						public WaitingForAdditionState(
							AbsState.IConstArg arg
						): base(arg){}
						public override void OnEnter(){
							thisHeatImage.StartSmoothFollowDeltaImageProcess();
						}
						public override void AddHeat(float delta){
							thisHeatImage.AddHeatImple(delta);
							thisEngine.SwitchToWaitingForComboExpireState();
						}
					}
					public interface IWaitingForComboExpireState: IState{}
					public class WaitingForComboExpireState: AbsState, IWaitingForComboExpireState{
						public WaitingForComboExpireState(
							IConstArg arg
						): base(arg){}
						public override void OnEnter(){
							thisHeatImage.StartWaitForNextAdditionProcess();
						}
						public override void AddHeat(float delta){
							thisHeatImage.AddHeatImple(delta);
							thisHeatImage.StartWaitForNextAdditionProcess();
						}
					}
				/* ConstArg */
					public interface IConstArg{
						IHeatImage heatImage{get;}
					}
					public class ConstArg: IConstArg{
						public ConstArg(
							IHeatImage image
						){
							thisHeatImage = image;
						}
						readonly IHeatImage thisHeatImage;
						public IHeatImage heatImage{get{return thisHeatImage;}}
					}
			}
		/* Const */
			public new interface IConstArg: AbsSceneObject.IConstArg{
			}
			public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
				public ConstArg(
					IHeatImageAdaptor adaptor
				): base(adaptor){
				}
			}
	}
}
