using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface ITestShootingTargetAdaptor: IShootingTargetAdaptor{
		void SetProcessManager(IProcessManager processManager);
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
		protected override void Awake(){
			base.Awake();
			MeshRenderer meshRenderer = this.transform.GetComponent<MeshRenderer>();
			thisMaterial = meshRenderer.material;
			thisHitTriggerHash = Animator.StringToHash("Hit");
			if(processManager != null)
				SetProcessManager(processManager);
		}
		public override void SetUp(){
			SetColor(defaultColor);
			SetUpTarget();
			thisAnimator = transform.GetComponent<Animator>();
		}
		protected virtual void SetUpTarget(){
			IAppleShooterProcessFactory processFactory = new AppleShooterProcessFactory(
				thisProcessManager
			);
			TestShootingTarget.IConstArg arg = new TestShootingTarget.ConstArg(
				health,
				this,
				defaultColor,
				processFactory,
				fadeTime
			);
			thisShootingTarget = new TestShootingTarget(arg);
		}
		public ProcessManager processManager;
		protected IProcessManager thisProcessManager;
		public void SetProcessManager(IProcessManager processManager){
			thisProcessManager = processManager;
		}
		public float health;
		public Color defaultColor;
		public float fadeTime;
		public override void SetUpReference(){

		}
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
			// Color newColor = GetColor();
			// newColor.a = alpha;
			SetColor(newColor);
		}
		public void CheckAndStartNewHitAnimation(float magnitude){
			thisAnimator.SetTrigger(thisHitTriggerHash);
		}
	}
}
