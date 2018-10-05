using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IDrawProcess: IProcess{
		void Hold();
	}
	public class DrawProcess : AbsProcess, IDrawProcess {

		public DrawProcess(
			IDrawProcessConstArg arg
		):base(
			arg
		){
			thisShootingManager = arg.shootingManager;
			thisProcessOrder = arg.processOrder;
		}
		readonly IShootingManager thisShootingManager;
		protected override void UpdateProcessImple(float deltaT){
			float drawDeltaTime = deltaT;
			if(isHeld)
				drawDeltaTime = 0f;
			thisShootingManager.Draw(drawDeltaTime);
		}
		readonly int thisProcessOrder;
		public override int GetProcessOrder(){
			return thisProcessOrder;
		}
		bool isHeld = false;
		public void Hold(){
			isHeld = true;
		}
	}

	public interface IDrawProcessConstArg: IProcessConstArg{
		IShootingManager shootingManager{get;}
		int processOrder{get;}
	}
	public class DrawProcessConstArg: ProcessConstArg, IDrawProcessConstArg{
		public DrawProcessConstArg(
			IProcessManager processManager,
			IShootingManager shootingManager,
			int processOrder
		): base(
			processManager
		){
			thisShootingManager = shootingManager;
			thisProcessOrder = processOrder;
		}
		readonly IShootingManager thisShootingManager;
		public IShootingManager shootingManager{get{return thisShootingManager;}}
		readonly int thisProcessOrder;
		public int processOrder{get{return thisProcessOrder;}}
	}
}
