using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IShootingProcess: IProcess{

	}
	public class ShootingProcess : AbsConstrainedProcess, IShootingProcess {

		public ShootingProcess(
			IConstArg arg
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



		public new interface IConstArg: AbsConstrainedProcess.IConstArg{
			IShootingManager shootingManager{get;}
			float fireRate{get;}
		}
		public new class ConstArg: AbsConstrainedProcess.ConstArg, IConstArg{
			public ConstArg(
				IShootingManager shootingManager,
				float fireRate,

				IProcessManager processManager
			): base(
				processManager,
				ProcessConstraint.None,
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


}
