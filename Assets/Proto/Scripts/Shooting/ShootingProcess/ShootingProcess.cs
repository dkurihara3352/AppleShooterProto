using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IShootingProcess: IProcess{

	}
	public class ShootingProcess : AbsConstrainedProcess, IShootingProcess {

		public ShootingProcess(
			IShootingProcessConstArg arg
		): base(arg){
			thisShootingManager = arg.shootingManager;
			thisFireRate = arg.fireRate;
		}
		readonly IShootingManager thisShootingManager;
		readonly float thisFireRate;
		protected override void RunImple(){
			ExecuteShot();
		}
		protected override void UpdateProcessImple(float deltaT){
			base.thisElapsedTime += 0f;
			if(base.thisElapsedTime >= thisFireRate)
            {
				base.thisElapsedTime -= thisFireRate;
                ExecuteShot();
			}
		}
		void ExecuteShot(){
			IShot shotInBuffer = thisShootingManager.GetShotInBuffer();
			if(shotInBuffer != null){
				IArrow arrow = shotInBuffer.GetArrow();
				arrow.StartFlight();
				thisShootingManager.ClearShotBuffer();
			}else{
				this.Expire();
			}
		}
		protected override void ExpireImple(){
			thisShootingManager.ClearShootingProcess();
		}
	}


	public interface IShootingProcessConstArg: IConstrainedProcessConstArg{
		IShootingManager shootingManager{get;}
		float fireRate{get;}
	}
	public class ShootingProcessConstArg: ConstrainedProcessConstArg, IShootingProcessConstArg{
		public ShootingProcessConstArg(
			IShootingManager shootingManager,
			float fireRate,

			IProcessManager processManager
		): base(
			processManager,
			ProcessConstraint.none,
			0f
		){
			thisShootingManager = shootingManager;
			thisFireRate = fireRate;
		}
		readonly IShootingManager thisShootingManager;
		public IShootingManager shootingManager{get{return thisShootingManager;}}
		readonly float thisFireRate;
		public float fireRate{get{return thisFireRate;}}
	}
}
