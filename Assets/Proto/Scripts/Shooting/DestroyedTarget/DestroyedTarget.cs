﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
namespace SlickBowShooting{
	public interface IDestroyedTarget: ISlickBowShootingSceneObject, IActivationStateHandler, IActivationStateImplementor{
		void ActivateAt(IShootingTarget shootingTarget);
		void SetPopUIReserve(IPopUIReserve reserve);
		void SetDestroyedTargetReserve(IDestroyedTargetReserve reserve);
		int GetIndex();
		IShootingTarget GetShootingTarget();
		void StopParticleSystem();
		void SetTier(TargetTierData data);
		void SetTargetTierDataOnQueue(TargetTierData data);
		void PopText(string text, Color color);
	}
	public class DestroyedTarget : SlickBowShootingSceneObject, IDestroyedTarget {
		public DestroyedTarget(
			IConstArg arg
		): base(
			arg
		){
			thisIndex = arg.index;
			thisActivationStateEngine = new ActivationStateEngine(this);
		}
		IActivationStateEngine thisActivationStateEngine;
		int thisIndex;
		public int GetIndex(){
			return thisIndex;
		}
		IDestroyedTargetAdaptor thisTypedAdaptor{
			get{
				return (IDestroyedTargetAdaptor)thisAdaptor;
			}
		}
		IPopUIReserve thisPopUIReserve;
		public void SetPopUIReserve(IPopUIReserve reserve){
			thisPopUIReserve = reserve;
		}

		public void ActivateAt(IShootingTarget target){
			Deactivate();
			thisTarget = target;
			thisTarget.SetDestroyedTarget(this);
					
			Vector3 position = target.GetPosition();
			Quaternion rotation = target.GetRotation();
			SetPosition(position);
			SetRotation(rotation);
			Activate();
		}
		public void Activate(){
			thisActivationStateEngine.Activate();
		}
		public void ActivateImple(){
			thisTypedAdaptor.StartDestruction();
			PopText(
				thisTarget.GetDestructionScore().ToString(),
				Color.green
			);
		}
		public void PopText(
			string text,
			Color color
		){
			thisPopUIReserve.PopText(
				this,
				text,
				color
			);
		}
		IShootingTarget thisTarget;
		public IShootingTarget GetShootingTarget(){
			return thisTarget;
		}
		public bool IsActivated(){
			return thisActivationStateEngine.IsActivated();
		}
		public void Deactivate(){
			thisActivationStateEngine.Deactivate();
		}
		IDestroyedTargetReserve thisDestroyedTargetReserve;
		public void SetDestroyedTargetReserve(IDestroyedTargetReserve reserve){
			thisDestroyedTargetReserve = reserve;
		}
		public void DeactivateImple(){
			if(thisTarget != null)
				thisTarget.CheckAndClearDestroyedTarget(this);
			thisTarget = null;
			thisTypedAdaptor.StopDestruction();
			thisDestroyedTargetReserve.Reserve(this);
			if(thisTargetTierDataOnQueue != null){
				SetTier(thisTargetTierDataOnQueue);
				thisTargetTierDataOnQueue = null;
			}
		}
		public void StopParticleSystem(){
			thisTypedAdaptor.StopParticleSystem();
		}
		public void SetTier(TargetTierData data){
			thisTypedAdaptor.SetMaterial(data.material);
		}
		TargetTierData thisTargetTierDataOnQueue;
		public void SetTargetTierDataOnQueue(TargetTierData data){
			thisTargetTierDataOnQueue = data;
		}
		/* Const */
			public new interface IConstArg: SlickBowShootingSceneObject.IConstArg{
				int index{get;}
			}
			public new class ConstArg: SlickBowShootingSceneObject.ConstArg, IConstArg{
				public ConstArg(
					int index,
					IDestroyedTargetAdaptor adaptor
				): base(
					adaptor
				){
					thisIndex = index;
				}
				readonly int thisIndex;
				public int index{get{return thisIndex;}}
			}
		/*  */
	}
}
