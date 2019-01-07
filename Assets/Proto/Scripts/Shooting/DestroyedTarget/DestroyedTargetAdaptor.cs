using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace SlickBowShooting{
	public interface IDestroyedTargetAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		IDestroyedTarget GetDestroyedTarget();

		void SetDestroyedTargetReserveAdaptor(IDestroyedTargetReserveAdaptor adaptor);
		void SetPopUIReserveAdaptor(IPopUIReserveAdaptor adaptor);
		
		/* used by client */
			void StartDestruction();
			void StopDestruction();
		/* used by process */
			void PlayParticleSystem();
			void StopParticleSystem();
		void SetIndex(int index);
		int GetIndex();
		void SetMaterial(Material mat);
	}
	public class DestroyedTargetAdaptor: SlickBowShootingMonoBehaviourAdaptor, IDestroyedTargetAdaptor{
		public override void SetUp(){
			base.SetUp();
			thisDestroyedTarget = CreateDestroyedTarget();

			thisParticleSystem = GetComponent<ParticleSystem>();
			thisParticleSystemRenderer = GetComponent<ParticleSystemRenderer>();
		}
		public override void SetUpReference(){
			IDestroyedTargetReserve destroyedTargetReserve = thisDestroyedTargetReserveAdaptor.GetDestroyedTargetReserve();
			thisDestroyedTarget.SetDestroyedTargetReserve(destroyedTargetReserve);
			
			IPopUIReserve popUIReserve  = thisPopUIReserveAdaptor.GetPopUIReserve();
			thisDestroyedTarget.SetPopUIReserve(popUIReserve);
		}
		public override void FinalizeSetUp(){
			thisDestroyedTarget.Deactivate();
		}
		IDestroyedTarget thisDestroyedTarget;
		IDestroyedTarget CreateDestroyedTarget(){
			DestroyedTarget.IConstArg arg = new DestroyedTarget.ConstArg(
				thisIndex,
				this
			);
			return new DestroyedTarget(arg);
		}
		public IDestroyedTarget GetDestroyedTarget(){
			return thisDestroyedTarget;
		}
		IDestroyedTargetReserveAdaptor thisDestroyedTargetReserveAdaptor;
		public void SetDestroyedTargetReserveAdaptor(IDestroyedTargetReserveAdaptor adaptor){
			thisDestroyedTargetReserveAdaptor = adaptor;
		}
		IPopUIReserveAdaptor thisPopUIReserveAdaptor;
		public void SetPopUIReserveAdaptor(IPopUIReserveAdaptor adaptor){
			thisPopUIReserveAdaptor = adaptor;
		}

		/* process */
			public float particleSystemLifeTime;
			public void StartDestruction(){
				StartParticleProcess();
			}
			IDestroyedTargetParticleProcess thisProcess;
			void StartParticleProcess(){
				StopParticleProcess();
				thisProcess = thisSlickBowShootingProcessFactory.CreateDestroyedTargetParticleProcess(
					this,
					particleSystemLifeTime
				);
				thisProcess.Run();
			}
			void StopParticleProcess(){
				if(thisProcess != null && thisProcess.IsRunning())
					thisProcess.Stop();
				thisProcess = null;
			}
			public void StopDestruction(){
				StopParticleProcess();
				thisParticleSystem.Stop();
			}
		/* particle */
			ParticleSystem thisParticleSystem;
			public void PlayParticleSystem(){
				thisParticleSystem.Play();
			}
			public void StopParticleSystem(){
				thisParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
			ParticleSystemRenderer thisParticleSystemRenderer;
			public void SetMaterial(Material mat){
				thisParticleSystemRenderer.material = mat;
			}
		/* misc */

		int thisIndex;
		public void SetIndex(int index){
			thisIndex = index;
		}
		public int GetIndex(){
			return thisIndex;
		}
	}
}

