using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
using DKUtility;

namespace SlickBowShooting{
	public interface IShootingTarget: ISlickBowShootingSceneObject, IActivationStateHandler, IActivationStateImplementor, IProcessHandler{
		
		void SetShootingTargetNormalHitDetector(IShootingTargetNormalHitDetector detector);
		void SetShootingTargetCriticalHitDetector(IShootingTargetCriticalHitDetector detector);
		
		void SetShootingManager(IShootingManager shootingManager);
		void Hit(IArrow arrow, bool crit);

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
		int GetTier();
		void SetTargetTierDataOnQueue(TargetTierData tierData);
	}
	public abstract class AbsShootingTarget : SlickBowShootingSceneObject, IShootingTarget {
		public AbsShootingTarget(
			IConstArg arg
		): base(
			arg
		){
			SetIndex(arg.index);
			thisTargetData = arg.targetData;
			thisAdaptor.SetName(thisTargetData.targetType.ToString() + " " + thisIndex.ToString());

			thisActivationStateEngine = new ActivationStateEngine(this);
			thisHealthBellCurve = arg.healthBellCurve;

			thisHitFlashProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisShootingTargetAdaptor.GetFlashProcessTime()
			);
			thisHitAnimationProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisShootingTargetAdaptor.GetHitAnimationProcessTime()
			);
			thisSpawnAnimationProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisShootingTargetAdaptor.GetSpawnAnimationProcessTime()
			);
		}
		public void SetShootingManager(IShootingManager manager){
			thisShootingManager = manager;
		}
		IShootingManager thisShootingManager;
		protected ITargetData thisTargetData;
		protected int thisOriginalHealth;
		protected int thisHealth;

		IShootingTargetAdaptor thisShootingTargetAdaptor{
			get{
				return (IShootingTargetAdaptor)thisAdaptor;
			}
		}
		protected bool thisIsRare{
			get{
				return thisShootingTargetAdaptor.IsRare();
			}
		}
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
				thisShootingTargetAdaptor.SetColor(thisDefaultColor);
				ToggleCollider(true);
				ToggleRenderer(true);

				StartSpawnAnimationProcess();
			}
			int ResetHealth(){
				float randomeMult = thisHealthBellCurve.Evaluate();
				return Mathf.FloorToInt(thisTargetData.health * randomeMult);
			}
			string GetActivationString(){
				return GetName();
			}

			public void Deactivate(){
				thisActivationStateEngine.Deactivate();
			}
			public virtual void DeactivateImple(){
				DeactivateAllLandedArrows();
				ReserveSelf();
				ToggleCollider(false);
				ToggleRenderer(false);
				if(thisTargetTierDataOnQueue != null){
					SetTier(thisTargetTierDataOnQueue);
					thisTargetTierDataOnQueue = null;
				}
				StopHitFlashProcess();
				ExpireHitAnimationProcess();
				ExpireSpawnAnimationProcess();
				// ToggleGameObject(false);
			}
			protected abstract void ReserveSelf();

			void ToggleGameObject(bool toggle){
				thisShootingTargetAdaptor.ToggleGameObject(toggle);
			}
		/* SpawnAnimationProcess */
			void StartSpawnAnimationProcess(){
				thisSpawnAnimationProcessSuite.Start();
			}
			void ExpireSpawnAnimationProcess(){
				if(thisSpawnAnimationProcessSuite.IsRunning())
					thisSpawnAnimationProcessSuite.Expire();
			}
			IProcessSuite thisSpawnAnimationProcessSuite;
			void UpdateSpawnScale(float normalizedTime){
				AnimationCurve spawnScaleCurve = thisShootingTargetAdaptor.GetSpawnAnimationScaleCurve();
				float newScaleValue = spawnScaleCurve.Evaluate(normalizedTime);
				thisShootingTargetAdaptor.SetSpawnAnimationScale(newScaleValue);
			}
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
					thisGameStatsTracker.RegisterTargetDestroyed(this, thisIsRare);
					thisShootingTargetAdaptor.PlayDestructionSound();
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
					return thisShootingTargetAdaptor.GetDefaultColor();
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
				StartHitFlashProcess(flashColor);
			}
			protected virtual void IndicateHit(
				float attack,
				float critBonus
			){
				float totalAttack = attack + critBonus;
				float hitMagnitude = CalculateHitMagnitude(totalAttack);
				StartHitAnimationProcess(hitMagnitude);
				if(critBonus > 0f){
					Color popTextColor = new Color(1f, .6f, .2f);
					thisPopUIReserve.PopText(
						this,
						"Great",
						popTextColor
					);
				}
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
			/* Hit Animation */
				void StartHitAnimationProcess(float hitMagnitude){
					thisInitialScaleValue = Mathf.Lerp(
						thisMinHitScale,
						thisMaxHitScale,
						hitMagnitude
					);
					thisHitAnimationProcessSuite.Start();
				}
				float thisInitialScaleValue;
				float thisMinHitScale{
					get{
						return thisShootingTargetAdaptor.GetMinHitScaleValue();
					}
				}
				float thisMaxHitScale{
					get{
						return thisShootingTargetAdaptor.GetMaxHitScaleValue();
					}
				}
				IProcessSuite thisHitAnimationProcessSuite;
				void UpdateHitScale(float normalizedTime){
					AnimationCurve scaleCurve = thisShootingTargetAdaptor.GetHitAnimationScaleCurve();
					float processValue = scaleCurve.Evaluate(normalizedTime);
					float newScaleValue = Mathf.Lerp(
						thisInitialScaleValue,
						1f,
						processValue
					);
					thisShootingTargetAdaptor.SetHitAnimationScale(newScaleValue);
				}
				void ExpireHitAnimationProcess(){
					if(thisHitAnimationProcessSuite.IsRunning())
						thisHitAnimationProcessSuite.Expire();
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
					if(!thisIsRare)
						thisCriticalHitDetector.ToggleCollider(toggle);
				}
				void ToggleRenderer(bool toggle){
					thisShootingTargetAdaptor.ToggleRenderer(toggle);
				}
				public ILandedArrow[] GetLandedArrows(){
					List<ILandedArrow> resultList = new List<ILandedArrow>();
					resultList.AddRange(thisNormalHitDetector.GetLandedArrows());
					if(!thisIsRare)
						resultList.AddRange(thisCriticalHitDetector.GetLandedArrows());
					return resultList.ToArray();
				}
				public void DeactivateAllLandedArrows(){
					thisNormalHitDetector.DeactivateAllLandedArrows();
					if(!thisIsRare)
						thisCriticalHitDetector.DeactivateAllLandedArrows();
				}
			/*  */
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
				thisShootingTargetAdaptor.SetMaterial(tierData.material);
				thisShootingTargetAdaptor.UpdateDefaultColor();
				UpdateTargetData(tierData.targetData);
			}
			public int GetTier(){
				return thisTargetData.tier;
			}
			public void UpdateTargetData(TargetData data){
				thisTargetData = data;
				thisShootingTargetAdaptor.SetTargetData(data);
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
					AnimationCurve flashColorValueCurve = thisShootingTargetAdaptor.GetFlashColorValueCurve();
					float colorValue = flashColorValueCurve.Evaluate(normalizedTime);
					//0 => default, 1f => flashColor
					Color newColor = Color.Lerp(
						thisDefaultColor,
						thisFlashTargetColor,
						colorValue
					);
					thisShootingTargetAdaptor.SetColor(newColor);

					Color newEmissionColor = Color.Lerp(
						thisOriginalEmissionColor,
						thisFlashTargetColor,
						colorValue
					);
					thisShootingTargetAdaptor.SetEmissionColor(newEmissionColor);

				}else if(suite == thisHitAnimationProcessSuite){
					UpdateHitScale(normalizedTime);
				}else if(suite == thisSpawnAnimationProcessSuite){
					UpdateSpawnScale(normalizedTime);
				}
			}
			Color thisOriginalEmissionColor{
				get{
					return thisShootingTargetAdaptor.GetOriginalEmissionColor();
				}
			}
			public void OnProcessExpire(IProcessSuite suite){
				if(suite == thisHitFlashProcessSuite){
					thisShootingTargetAdaptor.SetColor(thisDefaultColor);	
					thisShootingTargetAdaptor.SetEmissionColor(thisOriginalEmissionColor);
				}
				else if(suite == thisHitAnimationProcessSuite)
					UpdateHitScale(1f);
				else if(suite == thisSpawnAnimationProcessSuite)
					UpdateSpawnScale(1f);
			}
		/* Const */
			public new interface IConstArg: SlickBowShootingSceneObject.IConstArg{
				int index{get;}
				IBellCurve healthBellCurve{get;}
				ITargetData targetData{get;}
			}
			public new class ConstArg: SlickBowShootingSceneObject.ConstArg, IConstArg{
				public ConstArg(
					int index,
					IBellCurve healthBellCurve,
					IShootingTargetAdaptor adaptor,
					ITargetData targetData
				): base(
					adaptor
				){
					thisIndex = index;
					thisHealthBellCurve = healthBellCurve;
					thisTargetData = targetData;
				}
				readonly int thisIndex;
				public int index{get{return thisIndex;}}
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
