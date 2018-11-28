using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ITestShootingManagerAdaptor: IShootingManagerAdaptor{}
	public class TestShootingManagerAdaptor: ShootingManagerAdaptor, IShootingManagerAdaptor{
		public override void SetUpReference(){
			return;
		}
		protected override IShootingManager CreateShootingManager(){
			TestShootingManager.IConstArg arg = new TestShootingManager.ConstArg(
				this,
				0,
				1,

				null,
				0f,
				0f,

				0f,
				0f,
				
				0f,
				0f,
				0f,
				0f,
				0f
			);
			return new TestShootingManager(arg);
		}
	}
	public interface ITestShootingManager: IShootingManager{}
	public class TestShootingManager: ShootingManager, ITestShootingManager{
		public TestShootingManager(
			IConstArg arg
		): base(arg){}
		public override void SetNockedArrow(IArrow arrow){

		}
		public override void RegisterShot(IArrow arrow){

		}
		public override void CheckAndClearNockedArrow(IArrow arrow){

		}
		public override bool AcceptsNewShot(){
			return false;
		}
		public override float GetFlightSpeed(){
			return 0f;
		}
		public override Vector3 GetFlightDirection(){
			return Vector3.zero;
		}
		public override float GetFlightGravity(){
			return 0f;
		}
		public override Vector3 GetLauncherVelocity(){
			return Vector3.zero;
		}
		public override void SpawnLandedArrowOn(
			IShootingTarget target,
			Vector3 hitPosition,
			Quaternion rotation
		){

		}
	}
}
