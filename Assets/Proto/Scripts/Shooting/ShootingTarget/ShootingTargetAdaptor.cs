using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetAdaptor: IMonoBehaviourAdaptor{
		IShootingTarget GetShootingTarget();
		void SetDestroyedTargetReserveAdaptor(IDestroyedTargetReserveAdaptor adaptor);
		void SetPopUIReserveAdaptor(IPopUIReserveAdaptor adaptor);
		void SetIndex(int index);
		// void SetHealth(float health);
		void ToggleCollider(bool on);
		void SetColor(Color color);
		void PlayHitAnimation(float magnitude);
	}
	[RequireComponent(typeof(Collider), typeof(MeshRenderer), typeof(Animator))]
	public abstract class AbsShootingTargetAdaptor: MonoBehaviourAdaptor, IShootingTargetAdaptor{
		public override void SetUp(){
			MeshRenderer meshRenderer = this.transform.GetComponent<MeshRenderer>();
			thisMaterial = meshRenderer.material;
			thisDefaultColor = thisMaterial.color;
			
			thisShootingTarget = CreateShootingTarget();

			thisCollider = transform.GetComponent<Collider>();

			thisAnimator = transform.GetComponent<Animator>();

			thisHitTriggerHash = Animator.StringToHash("Hit");
			thisHitMagnitudeHash = Animator.StringToHash("HitMagnitude");
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
		public override void SetUpReference(){

			if(destroyedTargetReserveAdaptor != null)
				thisDestroyedTargetReserveAdaptor = destroyedTargetReserveAdaptor;
			if(popUIReserveAdaptor != null)
				thisPopUIReserveAdaptor = popUIReserveAdaptor;
				
			IDestroyedTargetReserve destroyedTargetReserve = thisDestroyedTargetReserveAdaptor.GetDestroyedTargetReserve();
			thisShootingTarget.SetDestroyedTargetReserve(destroyedTargetReserve);
			
			IPopUIReserve popUIReserve = thisPopUIReserveAdaptor.GetPopUIReserve();
			thisShootingTarget.SetPopUIReserve(popUIReserve);

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
		public float health;
		// public void SetHealth(float health){
		// 	thisHealth = health;
		// }
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
	}
}
