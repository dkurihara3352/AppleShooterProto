using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface IDrawProcess: IProcess{
		void Hold();
		bool IsHeld();
		void ReleaseHold();
	}
	public class DrawProcess : AbsProcess, IDrawProcess {

		public DrawProcess(
			IConstArg arg
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
			thisShootingManager.DrawImple(drawDeltaTime);
		}
		readonly int thisProcessOrder;
		public override int GetProcessOrder(){
			return thisProcessOrder;
		}
		bool isHeld = false;
		public void Hold(){
			isHeld = true;
		}
		public bool IsHeld(){
			return isHeld;
		}
		public void ReleaseHold(){
			this.isHeld = false;
		}

		public new interface IConstArg: AbsProcess.IConstArg{
			IShootingManager shootingManager{get;}
			int processOrder{get;}
		}
		public new class ConstArg: AbsProcess.ConstArg, IConstArg{
			public ConstArg(
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

}
