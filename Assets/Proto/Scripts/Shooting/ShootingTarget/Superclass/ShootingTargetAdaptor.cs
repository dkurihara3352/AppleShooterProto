using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace SlickBowShooting{
	public interface IShootingTargetAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		void SetUpDetectorAdaptors();

		IShootingTarget GetShootingTarget();
		void SetDestroyedTargetReserveAdaptor(IDestroyedTargetReserveAdaptor adaptor);
		void SetPopUIReserveAdaptor(IPopUIReserveAdaptor adaptor);
		void SetGameStatsTrackerAdaptor(IGameStatsTrackerAdaptor adaptor);
		void SetShootingManagerAdaptor(IShootingManagerAdaptor adaptor);
		void SetAudioManagerAdaptor(IAudioManagerAdaptor adaptor);

		void SetIndex(int index);
		void SetColor(Color color);

		void SetMaterial(Material mat);
		void SetTargetData(TargetData data);
		void UpdateDefaultColor();

		float GetFlashProcessTime();
		AnimationCurve GetFlashColorValueCurve();
		Color GetDefaultColor();
		Color GetOriginalEmissionColor();
		void SetEmissionColor(Color color);

		bool IsRare();
		void PlayDestructionSound();
		void ToggleRenderer(bool toggle);

		/* hit animation */
			float GetHitAnimationProcessTime();
			float GetMinHitScaleValue();
			float GetMaxHitScaleValue();
			AnimationCurve GetHitAnimationScaleCurve();
			void SetHitAnimationScale(float scaleValue);
		/* spawn animation */
			float GetSpawnAnimationProcessTime();
			AnimationCurve GetSpawnAnimationScaleCurve();
			void SetSpawnAnimationScale(float scaleValue);

	}
	public abstract class AbsShootingTargetAdaptor: SlickBowShootingMonoBehaviourAdaptor, IShootingTargetAdaptor{
		public override void SetUp(){

			thisColorHash = Shader.PropertyToID("_Color");
			thisEmissionHash = Shader.PropertyToID("_Emission");
			thisDefaultColor = GetColor();
			thisOriginalEmissionColor = GetEmissionColor();
			thisHealthBellCurve = CreateHealthBellCurve();
			
			thisShootingTarget = CreateShootingTarget();
		}
		int thisColorHash;
		public MeshRenderer modelMeshRenderer;
		public void ToggleRenderer(bool toggle){
			modelMeshRenderer.enabled = toggle;
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
			if(thisCriticalHitDetectorAdaptor != null)
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

			if(thisCriticalHitDetectorAdaptor != null){
				IShootingTargetCriticalHitDetector criticalHitDetector = thisCriticalHitDetectorAdaptor.GetShootingTargetCriticalHitDetector();
				thisShootingTarget.SetShootingTargetCriticalHitDetector(criticalHitDetector);
				criticalHitDetector.SetShootingTarget(thisShootingTarget);
			}

			thisAudioManager = thisAudioManagerAdaptor.GetAudioManager();
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
			protected Color thisOriginalEmissionColor;
			public Color GetOriginalEmissionColor(){
				return thisOriginalEmissionColor;
			}
			int thisEmissionHash;
			public Color GetEmissionColor(){
				Material mat = modelMeshRenderer.material;
				return mat.GetColor(thisEmissionHash);
			}
			public void SetEmissionColor(Color color){
				Material mat = modelMeshRenderer.material;
				mat.SetColor(thisEmissionHash, color);
			}

		/* Material */
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

		public void PlayDestructionSound(){
			destructionSoundSource.volume = thisAudioManager.GetSFXVolume();
			destructionSoundSource.Play();
		}
		public AudioSource destructionSoundSource;
		IAudioManager thisAudioManager;
		IAudioManagerAdaptor thisAudioManagerAdaptor;
		public void SetAudioManagerAdaptor(IAudioManagerAdaptor adaptor){
			thisAudioManagerAdaptor = adaptor;
		}
		/* Hit Animation */
			public float GetHitAnimationProcessTime(){
				return hitAnimationProcessTime;
			}
			public float hitAnimationProcessTime = .5f;
			public float GetMinHitScaleValue(){
				return minHitScaleValue;
			}
			public float minHitScaleValue = 1.1f;
			public float GetMaxHitScaleValue(){
				return maxHitScaleValue;
			}
			public float maxHitScaleValue = 1.5f;
			public AnimationCurve GetHitAnimationScaleCurve(){
				return hitAnimationScaleCurve;
			}
			public AnimationCurve hitAnimationScaleCurve;
			public void SetHitAnimationScale(float scaleValue){
				hitAnimationScaleTransform.localScale = Vector3.one * scaleValue;
			}
			public Transform hitAnimationScaleTransform;
		/* Spawn Animation */
			public float GetSpawnAnimationProcessTime(){
				return spawnAnimationProcessTime;
			}
			public float spawnAnimationProcessTime = 1f;
			public AnimationCurve GetSpawnAnimationScaleCurve(){
				return spawnAnimationScaleCurve;
			}
			public AnimationCurve spawnAnimationScaleCurve;
			public void SetSpawnAnimationScale(float scaleValue){
				spawnAnimationScaleTransform.localScale = Vector3.one * scaleValue;
			}
			public Transform spawnAnimationScaleTransform;
	}
}
