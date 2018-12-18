using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
using DKUtility;

namespace AppleShooterProto{
	public interface IShootingTarget: IAppleShooterSceneObject, IActivationStateHandler, IActivationStateImplementor, IProcessHandler{
		
		void SetShootingTargetNormalHitDetector(IShootingTargetNormalHitDetector detector);
		void SetShootingTargetCriticalHitDetector(IShootingTargetCriticalHitDetector detector);
		
		void SetShootingManager(IShootingManager shootingManager);

		void Hit(IArrow arrow, bool crit);
		// void AddLandedArrow(ILandedArrow landedArrow);
		// void RemoveLandedArrow(ILandedArrow landedArrow);
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

		TargetType GetTargetType();
		void SetTier(TargetTierData tierData);
		void SetTargetTierDataOnQueue(TargetTierData tierData);
	}
	public abstract class AbsShootingTarget : AppleShooterSceneObject, IShootingTarget {
		public AbsShootingTarget(
			IConstArg arg
		): base(
			arg
		){
			SetIndex(arg.index);
			thisTargetData = arg.targetData;
			thisAdaptor.SetName(thisTargetData.targetType.ToString() + " " + thisIndex.ToString());
			// thisDefaultColor = arg.defaultColor;
			thisActivationStateEngine = new ActivationStateEngine(this);
			thisHealthBellCurve = arg.healthBellCurve;

			thisHitFlashProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisTypedAdaptor.GetFlashProcessTime()
			);
		}
		public void SetShootingManager(IShootingManager manager){
			thisShootingManager = manager;
		}
		IShootingManager thisShootingManager;
		protected ITargetData thisTargetData;
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
				/* thisTypedAdaptor. */ToggleCollider(true);

				thisPopUIReserve.PopText(
					this,
					GetActivationString()
				);
			}
			int ResetHealth(){
				float randomeMult = thisHealthBellCurve.Evaluate();
				return Mathf.FloorToInt(thisTargetData.health * randomeMult);
			}
			string GetActivationString(){
				return GetName();
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
				/* thisTypedAdaptor. */ToggleCollider(false);
				if(thisTargetTierDataOnQueue != null){
					SetTier(thisTargetTierDataOnQueue);
					thisTargetTierDataOnQueue = null;
				}
				StopHitFlashProcess();
			}
			protected abstract void ReserveSelf();
		/* Hit & arrow interaction */
			public void Hit(
				IArrow arrow,
				bool crit
			){
				float attack = arrow.GetAttack();
				float critBonus = 0f;
				if(crit)
					critBonus = attack * (thisShootingManager.GetCriticalMultiplier() - 1f);
				thisHealth -= Mathf.FloorToInt(attack + critBonus);
				if(thisHealth <= 0f){
					DestroyTarget();
				}
				else{
					IndicateHealth(
						thisHealth,
						attack + critBonus
					);
					IndicateHit(
						attack,
						critBonus
					);
				}
				if(crit)
					thisShootingManager.Flash();
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
			Color thisDefaultColor{
				get{
					return thisTypedAdaptor.GetDefaultColor();
				}
			}
			protected virtual void IndicateHealth(
				int health,
				float delta
			){
				float normalizedHealth = (health * 1f)/ thisOriginalHealth;
				Color flashColor = Color.Lerp(
					Color.red,
					thisDefaultColor,
					normalizedHealth
				);
				// thisTypedAdaptor.SetColor(newColor);
				StartHitFlashProcess(flashColor);
			}
			protected virtual void IndicateHit(
				float attack,
				float critBonus
			){
				float totalAttack = attack + critBonus;
				float hitMagnitude = CalculateHitMagnitude(totalAttack);
				thisTypedAdaptor.PlayHitAnimation(hitMagnitude);
				thisPopUIReserve.PopText(
					this,
					GetArrowAttackString(attack, critBonus)
				);
			}
			string GetArrowAttackString(float attack, float critBonus){
				string result = attack.ToString("N0");
				if(critBonus != 0f)
					result += " + " +critBonus.ToString("N0");
				return result;
			}
			
			float CalculateHitMagnitude(float delta){
				return delta/thisOriginalHealth;
			}
			/* Landed Arrows */
			IShootingTargetNormalHitDetector thisNormalHitDetector;
			public void SetShootingTargetNormalHitDetector(IShootingTargetNormalHitDetector detector){
				thisNormalHitDetector = detector;
			}
			IShootingTargetCriticalHitDetector thisCriticalHitDetector;
			public void SetShootingTargetCriticalHitDetector(IShootingTargetCriticalHitDetector detector){
				thisCriticalHitDetector = detector;
			}
			void ToggleCollider(bool toggle){
				thisNormalHitDetector.ToggleCollider(toggle);
				thisCriticalHitDetector.ToggleCollider(toggle);
			}
			public ILandedArrow[] GetLandedArrows(){
				List<ILandedArrow> resultList = new List<ILandedArrow>();
				resultList.AddRange(thisNormalHitDetector.GetLandedArrows());
				resultList.AddRange(thisCriticalHitDetector.GetLandedArrows());
				return resultList.ToArray();
			}
			public void DeactivateAllLandedArrows(){
				thisNormalHitDetector.DeactivateAllLandedArrows();
				thisCriticalHitDetector.DeactivateAllLandedArrows();
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
			public override string GetName(){
				return thisTargetData.targetType.ToString() + " " + GetIndex().ToString();
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
			public TargetType GetTargetType(){
				return thisTargetData.targetType;
			}
			public void SetTier(TargetTierData tierData){
				thisTypedAdaptor.SetMaterial(tierData.material);
				thisTypedAdaptor.UpdateDefaultColor();
				UpdateTargetData(tierData.targetData);
			}
			public void UpdateTargetData(TargetData data){
				thisTargetData = data;
				thisTypedAdaptor.SetTargetData(data);
			}
			TargetTierData thisTargetTierDataOnQueue;
			public void SetTargetTierDataOnQueue(TargetTierData tierData){
				thisTargetTierDataOnQueue = tierData;
			}
		/* hitFlashProcess */
			Color thisFlashTargetColor;
			void StartHitFlashProcess(Color flashColor){
				thisFlashTargetColor = flashColor;
				thisHitFlashProcessSuite.Start();
			}
			void StopHitFlashProcess(){
				thisHitFlashProcessSuite.Stop();
				OnProcessExpire(thisHitFlashProcessSuite);
			}
			
			IProcessSuite thisHitFlashProcessSuite;
			public void OnProcessRun(IProcessSuite suite){
			}
			public void OnProcessUpdate(
				float deltaTime,
				float normalizedTime,
				IProcessSuite suite
			){
				if(suite == thisHitFlashProcessSuite){
					AnimationCurve flashColorValueCurve = thisTypedAdaptor.GetFlashColorValueCurve();
					float colorValue = flashColorValueCurve.Evaluate(normalizedTime);
					//0 => default, 1f => flashColor
					Color newColor = Color.Lerp(
						thisDefaultColor,
						thisFlashTargetColor,
						colorValue
					);
					thisTypedAdaptor.SetColor(newColor);
				}
			}
			public void OnProcessExpire(IProcessSuite suite){
				thisTypedAdaptor.SetColor(thisDefaultColor);	
			}
		/* Const */
			public new interface IConstArg: AppleShooterSceneObject.IConstArg{
				int index{get;}
				// Color defaultColor{get;}
				IBellCurve healthBellCurve{get;}
				ITargetData targetData{get;}
			}
			public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
				public ConstArg(
					int index,
					// Color defaultColor,
					IBellCurve healthBellCurve,
					IShootingTargetAdaptor adaptor,
					ITargetData targetData
				): base(
					adaptor
				){
					thisIndex = index;
					// thisDefaultColor = defaultColor;
					thisHealthBellCurve = healthBellCurve;
					thisTargetData = targetData;
				}
				readonly int thisIndex;
				public int index{get{return thisIndex;}}
				// readonly Color thisDefaultColor;
				// public Color defaultColor{get{return thisDefaultColor;}}
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
