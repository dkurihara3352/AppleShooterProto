using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTarget: ISceneObject, IActivationStateHandler, IActivationStateImplementor{
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
	}
	public abstract class AbsShootingTarget : AbsSceneObject, IShootingTarget {
		public AbsShootingTarget(
			IConstArg arg
		): base(
			arg
		){
			SetIndex(arg.index);
			thisOriginalHealth = arg.health;
			thisHealth = thisOriginalHealth;
			thisDefaultColor = arg.defaultColor;
			thisActivationStateEngine = new ActivationStateEngine(this);
			thisHealthBellCurve = arg.healthBellCurve;

			thisHeatBonus = arg.heatBonus;
		}
		protected float thisOriginalHealth;
		protected float thisHealth;

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
				float randomeMult = thisHealthBellCurve.Evaluate();
				
				thisHealth = thisOriginalHealth *= randomeMult;
				thisTypedAdaptor.SetColor(thisDefaultColor);
				thisTypedAdaptor.ToggleCollider(true);
				thisPopUIReserve.PopText(
					this,
					GetHealthString()
				);
			}
			string GetHealthString(){
				string result = "health \n" + Mathf.RoundToInt(thisHealth).ToString();
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
				thisHealth -= attack;
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
				float health,
				float delta
			){
				float normalizedHealth = health/ thisOriginalHealth;
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

			readonly float thisHeatBonus;
			public float GetHeatBonus(){
				return thisHeatBonus;
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
			public new interface IConstArg: AbsSceneObject.IConstArg{
				int index{get;}
				float health{get;}
				Color defaultColor{get;}
				IBellCurve healthBellCurve{get;}
				float heatBonus{get;}
			}
			public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
				public ConstArg(
					int index,
					float health,
					Color defaultColor,
					IBellCurve healthBellCurve,
					IShootingTargetAdaptor adaptor,
					float heatBonus
				): base(
					adaptor
				){
					thisIndex = index;
					thisHealth = health;
					thisDefaultColor = defaultColor;
					thisHealthBellCurve = healthBellCurve;
					thisHeatBonus = heatBonus;
				}
				readonly int thisIndex;
				public int index{get{return thisIndex;}}
				readonly float thisHealth;
				public float health{get{return thisHealth;}}
				readonly Color thisDefaultColor;
				public Color defaultColor{get{return thisDefaultColor;}}
				readonly IBellCurve thisHealthBellCurve;
				public IBellCurve healthBellCurve{
					get{return thisHealthBellCurve;}
				}
				readonly float thisHeatBonus;
				public float heatBonus{get{return thisHeatBonus;}}
			}
		/*  */
	}

}
