using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace AppleShooterProto{
	public interface IShootingTarget: IAppleShooterSceneObject, IActivationStateHandler, IActivationStateImplementor{
		void Hit(IArrow arrow);
		void AddLandedArrow(ILandedArrow landedArrow);
		void RemoveLandedArrow(ILandedArrow landedArrow);
		void DeactivateAllLandedArrows();
		ILandedArrow[] GetLandedArrows();

		void SetDestroyedTargetReserve(IDestroyedTargetReserve reserve);
		void SetPopUIReserve(IPopUIReserve reserve);
		void SetGameStatsTracker(IGameStatsTracker tracker);

		void DeactivateAll();// called when curve is cycled

		void SetIndex(int index);
		int GetIndex();

		void CheckAndClearDestroyedTarget(IDestroyedTarget target);
		void SetDestroyedTarget(IDestroyedTarget target);
		IDestroyedTarget GetDestroyedTarget();

		float GetHeatBonus();
		int GetDestructionScore();
	}
	public abstract class AbsShootingTarget : AppleShooterSceneObject, IShootingTarget {
		public AbsShootingTarget(
			IConstArg arg
		): base(
			arg
		){
			SetIndex(arg.index);
			thisTargetData = arg.targetData;
			thisDefaultColor = arg.defaultColor;
			thisActivationStateEngine = new ActivationStateEngine(this);
			thisHealthBellCurve = arg.healthBellCurve;
		}
		ITargetData thisTargetData;
		protected int thisOriginalHealth;
		protected int thisHealth;

		IShootingTargetAdaptor thisTypedAdaptor{
			get{
				return (IShootingTargetAdaptor)thisAdaptor;
			}
		}
		public IMarkerUIReserve thisMarkerUIReserve;
		/* Activation */
			IActivationStateEngine thisActivationStateEngine;
			public bool IsActivated(){
				return thisActivationStateEngine.IsActivated();
			}
			public void Activate(){
				thisActivationStateEngine.Activate();
			}
			IBellCurve thisHealthBellCurve;
			public virtual void ActivateImple(){
				thisOriginalHealth = ResetHealth();
				thisHealth = thisOriginalHealth;
				thisTypedAdaptor.SetColor(thisDefaultColor);
				thisTypedAdaptor.ToggleCollider(true);
				thisPopUIReserve.PopText(
					this,
					GetHealthString()
				);
			}
			int ResetHealth(){
				float randomeMult = thisHealthBellCurve.Evaluate();
				return Mathf.FloorToInt(thisTargetData.health * randomeMult);
			}
			string GetHealthString(){
				string result = "health \n" + thisHealth.ToString();
				return result;
			}
			public void Deactivate(){
				thisActivationStateEngine.Deactivate();
			}
			public virtual void DeactivateImple(){
				DeactivateAllLandedArrows();
				ReserveSelf();
				thisTypedAdaptor.ToggleCollider(false);
			}
			protected abstract void ReserveSelf();
		/* Hit & arrow interaction */
			public void Hit(
				IArrow arrow
			){
				float attack = arrow.GetAttack();
				thisHealth -= Mathf.FloorToInt(attack);
				if(thisHealth <= 0f){
					DestroyTarget();
				}
				else{
					IndicateHealth(
						thisHealth,
						attack
					);
					IndicateHit(attack);
				}
			}
			/* DestroyedTarget */
				IDestroyedTargetReserve thisDestroyedTargetReserve;
				public void SetDestroyedTargetReserve(IDestroyedTargetReserve reserve){
					thisDestroyedTargetReserve = reserve;
				}
				IGameStatsTracker thisGameStatsTracker;
				public void SetGameStatsTracker(IGameStatsTracker tracker){
					thisGameStatsTracker = tracker;
				}
				protected virtual void DestroyTarget(){
					thisDestroyedTargetReserve.ActivateDestoryedTargetAt(this);
					Deactivate();
					thisGameStatsTracker.RegisterTargetDestroyed(this);
				}
				IDestroyedTarget thisDestroyedTarget;
				public void SetDestroyedTarget(IDestroyedTarget target){
					thisDestroyedTarget = target;
				}
				public void CheckAndClearDestroyedTarget(IDestroyedTarget target){
					if(thisDestroyedTarget == target)
						thisDestroyedTarget = null;
				}
				public IDestroyedTarget GetDestroyedTarget(){
					return thisDestroyedTarget;
				}
				void DeactivateDestroyedTarget(){
					if(thisDestroyedTarget != null)
						thisDestroyedTarget.Deactivate();
				}
			/*  */
			readonly Color thisDefaultColor;
			protected virtual void IndicateHealth(
				int health,
				float delta
			){
				float normalizedHealth = (health * 1f)/ thisOriginalHealth;
				Color newColor = Color.Lerp(
					Color.red,
					thisDefaultColor,
					normalizedHealth
				);
				thisTypedAdaptor.SetColor(newColor);
			}
			protected virtual void IndicateHit(
				float delta
			){
				float hitMagnitude = CalculateHitMagnitude(delta);
				thisTypedAdaptor.PlayHitAnimation(hitMagnitude);
				int deltaInt = Mathf.RoundToInt(delta);
				thisPopUIReserve.PopText(
					this,
					deltaInt.ToString()
				);
			}
			float CalculateHitMagnitude(float delta){
				return delta/thisOriginalHealth;
			}
			/* Landed Arrows */
			List<ILandedArrow> thisLandedArrows = new List<ILandedArrow>();
			public ILandedArrow[] GetLandedArrows(){
				return thisLandedArrows.ToArray();
			}
			public void AddLandedArrow(ILandedArrow arrow){
				thisLandedArrows.Add(arrow);
			}
			public void RemoveLandedArrow(ILandedArrow arrow){
				if(thisLandedArrows.Contains(arrow))
					thisLandedArrows.Remove(arrow);
			}
			public void DeactivateAllLandedArrows(){
				List<ILandedArrow> temp = new List<ILandedArrow>(thisLandedArrows);
				foreach(ILandedArrow arrow in temp)
					if(arrow != null)
						arrow.Deactivate();
			}
			protected IPopUIReserve thisPopUIReserve;
			public void SetPopUIReserve(IPopUIReserve reserve){
				thisPopUIReserve = reserve;
			}
			public float GetHeatBonus(){
				return thisTargetData.heatBonus;
			}
			public int GetDestructionScore(){
				return thisTargetData.destructionScore;
			}
		/* Misc */
			int thisIndex;
			public virtual void SetIndex(int index){
				thisIndex = index;
			}
			public int GetIndex(){
				return thisIndex;
			}
			public void DeactivateAll(){
				Deactivate();
				DeactivateAllLandedArrows();
				DeactivateDestroyedTarget();
			}
		/* Const */
			public new interface IConstArg: AppleShooterSceneObject.IConstArg{
				int index{get;}
				Color defaultColor{get;}
				IBellCurve healthBellCurve{get;}
				ITargetData targetData{get;}
			}
			public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
				public ConstArg(
					int index,
					Color defaultColor,
					IBellCurve healthBellCurve,
					IShootingTargetAdaptor adaptor,
					ITargetData targetData
				): base(
					adaptor
				){
					thisIndex = index;
					thisDefaultColor = defaultColor;
					thisHealthBellCurve = healthBellCurve;
					thisTargetData = targetData;
				}
				readonly int thisIndex;
				public int index{get{return thisIndex;}}
				readonly Color thisDefaultColor;
				public Color defaultColor{get{return thisDefaultColor;}}
				readonly IBellCurve thisHealthBellCurve;
				public IBellCurve healthBellCurve{
					get{return thisHealthBellCurve;}
				}
				readonly ITargetData thisTargetData;
				public ITargetData targetData{get{return thisTargetData;}}
			}
		/*  */
	}

}
