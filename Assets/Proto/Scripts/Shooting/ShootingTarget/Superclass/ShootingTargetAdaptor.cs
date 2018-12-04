using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace AppleShooterProto{
	public interface IShootingTargetAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IShootingTarget GetShootingTarget();
		void SetDestroyedTargetReserveAdaptor(IDestroyedTargetReserveAdaptor adaptor);
		void SetPopUIReserveAdaptor(IPopUIReserveAdaptor adaptor);
		void SetGameStatsTrackerAdaptor(IGameStatsTrackerAdaptor adaptor);
		void SetShootingManagerAdaptor(IShootingManagerAdaptor adaptor);
		void SetIndex(int index);
		void ToggleCollider(bool on);
		void SetColor(Color color);
		void PlayHitAnimation(float magnitude);

		void SetMaterial(Material mat);
		void SetTargetData(TargetData data);
	}
	[RequireComponent(typeof(Animator))]
	public abstract class AbsShootingTargetAdaptor: AppleShooterMonoBehaviourAdaptor, IShootingTargetAdaptor{
		public override void SetUp(){
			MeshRenderer meshRenderer = CollectMeshRenderer();
			thisMaterial = meshRenderer.material;
			thisDefaultColor = thisMaterial.color;
			thisHealthBellCurve = CreateHealthBellCurve();
			
			thisShootingTarget = CreateShootingTarget();

			thisCollider = CollectCollider();

			thisAnimator = CollectAnimator();

			thisHitTriggerHash = Animator.StringToHash("Hit");
			thisHitMagnitudeHash = Animator.StringToHash("HitMagnitude");
		}
		public MeshRenderer modelMeshRenderer;
		MeshRenderer CollectMeshRenderer(){
			return modelMeshRenderer;
		}
		BoxCollider CollectCollider(){
			return GetComponentInChildren<BoxCollider>();
		}
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

		/* Collider */
		Collider thisCollider;
		public void ToggleCollider( bool on){
			thisCollider.enabled = on;
		}
		/* Color */
		Material thisMaterial;
		protected Color thisDefaultColor;
		public void SetColor(Color color){
			thisMaterial.color = color;
		}
		Color GetColor(){
			return thisMaterial.color;
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
	}
}
