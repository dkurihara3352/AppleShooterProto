using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace AppleShooterProto{
	public interface IShootingTargetAdaptor: IAppleShooterMonoBehaviourAdaptor{
		void SetUpDetectorAdaptors();

		IShootingTarget GetShootingTarget();
		void SetDestroyedTargetReserveAdaptor(IDestroyedTargetReserveAdaptor adaptor);
		void SetPopUIReserveAdaptor(IPopUIReserveAdaptor adaptor);
		void SetGameStatsTrackerAdaptor(IGameStatsTrackerAdaptor adaptor);
		void SetShootingManagerAdaptor(IShootingManagerAdaptor adaptor);
		void SetIndex(int index);
		void SetColor(Color color);
		void PlayHitAnimation(float magnitude);

		void SetMaterial(Material mat);
		void SetTargetData(TargetData data);
		void UpdateDefaultColor();

		float GetFlashProcessTime();
		AnimationCurve GetFlashColorValueCurve();
		Color GetDefaultColor();

		bool IsRare();
	}
	[RequireComponent(typeof(Animator))]
	public abstract class AbsShootingTargetAdaptor: AppleShooterMonoBehaviourAdaptor, IShootingTargetAdaptor{
		public override void SetUp(){

			thisColorHash = Shader.PropertyToID("_Color");
			thisDefaultColor = GetColor();

			thisHealthBellCurve = CreateHealthBellCurve();
			
			thisShootingTarget = CreateShootingTarget();

			thisAnimator = CollectAnimator();

			thisHitTriggerHash = Animator.StringToHash("Hit");
			thisHitMagnitudeHash = Animator.StringToHash("HitMagnitude");
		}
		int thisColorHash;
		public MeshRenderer modelMeshRenderer;
		Animator CollectAnimator(){
			return GetComponent<Animator>();
		}
		protected IShootingTarget thisShootingTarget;
		protected abstract IShootingTarget CreateShootingTarget();
		public IShootingTarget GetShootingTarget(){
			return thisShootingTarget;
		}
		IDestroyedTargetReserveAdaptor thisDestroyedTargetReserveAdaptor;
		public DestroyedTargetReserveAdaptor destroyedTargetReserveAdaptor;
		public void SetDestroyedTargetReserveAdaptor(IDestroyedTargetReserveAdaptor adaptor){
			thisDestroyedTargetReserveAdaptor = adaptor;
		}
		IPopUIReserveAdaptor thisPopUIReserveAdaptor;
		public PopUIReserveAdaptor popUIReserveAdaptor;
		public void SetPopUIReserveAdaptor(IPopUIReserveAdaptor adaptor){
			thisPopUIReserveAdaptor = adaptor;
		}
		protected IBellCurve thisHealthBellCurve;

		public TargetData targetData;
		public void SetTargetData(TargetData data){
			targetData = data;
		}

		IGameStatsTrackerAdaptor thisGameStatsTrackerAdaptor;
		public void SetGameStatsTrackerAdaptor(IGameStatsTrackerAdaptor adaptor){
			thisGameStatsTrackerAdaptor = adaptor;
		}
		IShootingManagerAdaptor thisShootingManagerAdaptor;
		public void SetShootingManagerAdaptor(IShootingManagerAdaptor adaptor){
			thisShootingManagerAdaptor = adaptor;
		}

		IBellCurve CreateHealthBellCurve(){
			return new BellCurve(
				1f,
				.3f,
				.8f,
				1.2f,
				30
			);
		}
		public ShootingTargetNormalHitDetectorAdaptor thisNormalHitDetectorAdaptor;
		public ShootingTargetCriticalHitDetectorAdaptor thisCriticalHitDetectorAdaptor;
		public void SetUpDetectorAdaptors(){
			thisNormalHitDetectorAdaptor.SetUp();
			thisCriticalHitDetectorAdaptor.SetUp();
		}
		public override void SetUpReference(){

			if(destroyedTargetReserveAdaptor != null)
				thisDestroyedTargetReserveAdaptor = destroyedTargetReserveAdaptor;
			if(popUIReserveAdaptor != null)
				thisPopUIReserveAdaptor = popUIReserveAdaptor;
				
			IDestroyedTargetReserve destroyedTargetReserve = thisDestroyedTargetReserveAdaptor.GetDestroyedTargetReserve();
			thisShootingTarget.SetDestroyedTargetReserve(destroyedTargetReserve);
			
			IPopUIReserve popUIReserve = thisPopUIReserveAdaptor.GetPopUIReserve();
			thisShootingTarget.SetPopUIReserve(popUIReserve);

			IGameStatsTracker tracker = thisGameStatsTrackerAdaptor.GetTracker();
			thisShootingTarget.SetGameStatsTracker(tracker);

			IShootingManager shootingManager = thisShootingManagerAdaptor.GetShootingManager();
			thisShootingTarget.SetShootingManager(shootingManager);

			IShootingTargetNormalHitDetector normalHitDetector = thisNormalHitDetectorAdaptor.GetShootingTargetNormalHitDetector();
			thisShootingTarget.SetShootingTargetNormalHitDetector(normalHitDetector);
			normalHitDetector.SetShootingTarget(thisShootingTarget);

			IShootingTargetCriticalHitDetector criticalHitDetector = thisCriticalHitDetectorAdaptor.GetShootingTargetCriticalHitDetector();
			thisShootingTarget.SetShootingTargetCriticalHitDetector(criticalHitDetector);
			criticalHitDetector.SetShootingTarget(thisShootingTarget);
		}
		public override void FinalizeSetUp(){
			thisShootingTarget.Deactivate();
		}


		IPopUIReserve thisPopUIReserve;
		public void SetPopUIReserve(IPopUIReserve reserve){
			thisPopUIReserve = reserve;
		}
		IDestroyedTargetReserve thisDestroyedTargetReserve;
		public void SetDestroyedTargetReserve(IDestroyedTargetReserve reserve){
			thisDestroyedTargetReserve = reserve;
		}
		protected int thisIndex;
		public void SetIndex(int index){
			thisIndex = index;
		}


		/* Color */
			protected Color thisDefaultColor;
			public Color GetDefaultColor(){
				return thisDefaultColor;
			}
			public void SetColor(Color color){
				Material mat = modelMeshRenderer.material;
				mat.SetColor(thisColorHash, color);

			}
			Color GetColor(){
				Material mat = modelMeshRenderer.material;
				return mat.GetColor(thisColorHash);
			}

		/* Animator */
			Animator thisAnimator;
			int thisHitTriggerHash;
			int thisHitMagnitudeHash;
			public void PlayHitAnimation(float magnitude){
				thisAnimator.SetFloat(thisHitMagnitudeHash, magnitude);
				thisAnimator.SetTrigger(thisHitTriggerHash);
			}
			public void SetMaterial(Material material){
				modelMeshRenderer.material = material;
			}
			public void UpdateDefaultColor(){
				thisDefaultColor = GetColor();
			}
		/*  */
		public float flashProcessTime = .5f;
		public float GetFlashProcessTime(){
			return flashProcessTime;
		}
		public AnimationCurve flashColorValueCurve;//0 => default, 1 => flashColor
		public AnimationCurve GetFlashColorValueCurve(){
			return flashColorValueCurve;
		}
		public bool isRare = false;
		public bool IsRare(){
			return isRare;
		}
	}
}
