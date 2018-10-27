using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface ITestShootingTargetAdaptor: IShootingTargetAdaptor{
		void SetReserveTransform(Transform reserveTransform);
		void ToggleCollider(bool on);
		void SetColor(Color color);
		void StartHitAnimation(float magnitude);
		void StopHitAnimation();
		float GetAlpha();
		void SetAlpha(float a);
		void CheckAndStartNewHitAnimation(float magnitude);
	}
	[RequireComponent(typeof(Collider))]
	public class TestShootingTargetAdaptor : ShootingTargetAdaptor, ITestShootingTargetAdaptor {
		public override void SetUp(){
			SetUpTarget();
			thisAnimator = transform.GetComponent<Animator>();

			MeshRenderer meshRenderer = this.transform.GetComponent<MeshRenderer>();
			thisMaterial = meshRenderer.material;
			SetColor(defaultColor);
			thisHitTriggerHash = Animator.StringToHash("Hit");
			thisHitMagnitudeHash = Animator.StringToHash("HitMagnitude");
		}
		protected virtual void SetUpTarget(){
			TestShootingTarget.IConstArg arg = new TestShootingTarget.ConstArg(
				health,
				this,
				defaultColor,
				processFactory,
				fadeTime
			);
			thisShootingTarget = new TestShootingTarget(arg);
		}

		public void SetReserveTransform(Transform reserveTransform){
			this.reserveTransform = reserveTransform;
		}
		public float health;
		public Color defaultColor;
		public float fadeTime;
		public void ToggleCollider( bool on){
			Collider collider = this.transform.GetComponent<Collider>();
			collider.enabled = on;
		}
		Material thisMaterial;
		public void SetColor(Color color){
			thisMaterial.color = color;
		}
		Color GetColor(){
			return thisMaterial.color;
		}
		Animator thisAnimator;
		int thisHitTriggerHash;
		int thisHitMagnitudeHash;
		public void StartHitAnimation(float magnitude){
			thisAnimator.SetTrigger(thisHitTriggerHash);
		}
		public void StopHitAnimation(){
			return;
		}
		public float GetAlpha(){
			return thisMaterial.color.a;
		}
		public void SetAlpha(float alpha){
			Color curColor = GetColor();
			Color newColor = new Color(
				curColor.r,
				curColor.g,
				curColor.b,
				alpha
			);
			SetColor(newColor);
		}
		public void CheckAndStartNewHitAnimation(float magnitude){
			thisAnimator.SetFloat(thisHitMagnitudeHash, magnitude);
			thisAnimator.SetTrigger(thisHitTriggerHash);
		}
	}
}
