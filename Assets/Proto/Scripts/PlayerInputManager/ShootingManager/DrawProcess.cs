using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IDrawProcess: IProcess{}
	public class DrawProcess : AbsProcess, IDrawProcess {

		public DrawProcess(
			IDrawProcessConstArg arg
		):base(
			arg
		){
			thisShootingManager = arg.shootingManager;
		}
		readonly IShootingManager thisShootingManager;
		protected override void UpdateProcessImple(float deltaT){
			thisShootingManager.Draw(deltaT);
		}
	}

	public interface IDrawProcessConstArg: IProcessConstArg{
		IShootingManager shootingManager{get;}
	}
	public class DrawProcessConstArg: ProcessConstArg, IDrawProcessConstArg{
		public DrawProcessConstArg(
			IProcessManager processManager,
			IShootingManager shootingManager
		): base(
			processManager
		){
			thisShootingManager = shootingManager;
		}
		readonly IShootingManager thisShootingManager;
		public IShootingManager shootingManager{get{return thisShootingManager;}}
	}
}
