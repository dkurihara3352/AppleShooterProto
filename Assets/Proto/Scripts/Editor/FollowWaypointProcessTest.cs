using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using AppleShooterProto;
using DKUtility;

[TestFixture, Category("AppleShooterProto")]
public class FollowWaypointProcessTest {
	/* SetNewTargetWaypoint */
		[Test]
		public void SetNewTargetWaypoint_SetsNextWaypoint(){
			IFollowWaypointProcessConstArg arg = CreateMockConstArg();
			TestFollowWaypointProcess process = new TestFollowWaypointProcess(arg);
			
			IWaypoint nextWaypoint = Substitute.For<IWaypoint>();
			arg.follower.GetCurrentWaypoint().Returns(nextWaypoint);

			process.SetNewTargetWaypoint_Test();

			Assert.That(process.GetTargetWaypoint_Test(), Is.SameAs(nextWaypoint));
		}
		[Test]
		public void SetNewTargetWaypoint_SetsInitPosition(){
			IFollowWaypointProcessConstArg arg = CreateMockConstArg();
			TestFollowWaypointProcess process = new TestFollowWaypointProcess(arg);
			
			IWaypoint nextWaypoint = Substitute.For<IWaypoint>();
			Vector3 prevPosition = new Vector3(10f, 10f);
			nextWaypoint.GetPreviousWaypointPosition().Returns(prevPosition);
			arg.follower.GetCurrentWaypoint().Returns(nextWaypoint);

			process.SetNewTargetWaypoint_Test();

			Assert.That(process.GetInitPosition_Test(), Is.EqualTo(prevPosition));
		}
		[Test]
		public void SetNewTargetWaypoint_SetsTargetPosition(){
			IFollowWaypointProcessConstArg arg = CreateMockConstArg();
			TestFollowWaypointProcess process = new TestFollowWaypointProcess(arg);
			
			IWaypoint nextWaypoint = Substitute.For<IWaypoint>();
			Vector3 position = new Vector3(10f, 10f);
			nextWaypoint.GetPosition().Returns(position);
			arg.follower.GetCurrentWaypoint().Returns(nextWaypoint);

			process.SetNewTargetWaypoint_Test();

			Assert.That(process.GetTargetPosition_Test(), Is.EqualTo(position));
		}
		[Test]
		public void SetNewTargetWaypoint_ResetsElapsedTime(){
			IFollowWaypointProcessConstArg arg = CreateMockConstArg();
			TestFollowWaypointProcess process = new TestFollowWaypointProcess(arg);

			process.SetNewTargetWaypoint_Test();

			Assert.That(process.GetElapsedTimeForCurrentWaypoint_Test(), Is.EqualTo(0f));
		}
		[Test]
		public void SetNewTargetWaypoint_SetsRequiredTime([Values(10f)] float anyTime){
			IFollowWaypointProcessConstArg arg = CreateMockConstArg();
			TestFollowWaypointProcess process = new TestFollowWaypointProcess(arg);
			
			IWaypoint nextWaypoint = Substitute.For<IWaypoint>();
			arg.follower.GetCurrentWaypoint().Returns(nextWaypoint);
			nextWaypoint.GetRequiredTime().Returns(anyTime);

			process.SetNewTargetWaypoint_Test();

			Assert.That(process.GetRequiredTimeForCurrentWaypoint_Test(), Is.EqualTo(anyTime));
		}
	/* RequiredTimeForCurrentWaypointHasPassed */
		[Test, TestCaseSource(typeof(RequiredTimeForCurrentWaypointHasPassed_TestCase), "validCases")]
		public void RequiredTimeForCurrentWaypointHasPassed_SumOfDeltaTimeAndElapsedIsGreaterOrEqualToRequired_ReturnsTrue(
			float deltaTime,
			float elapsedTime,
			float requiredTime,
			bool expected
		){
			IFollowWaypointProcessConstArg arg = CreateMockConstArg();
			TestFollowWaypointProcess process = new TestFollowWaypointProcess(arg);
			process.SetElapsedTime_Test(elapsedTime);
			process.SetRequiredTime_Test(requiredTime);

			bool actual = process.RequiredtimeForCurrentWaypointHasPassed_Test(deltaTime);

			Assert.That(actual, Is.EqualTo(expected));
		}
		public class RequiredTimeForCurrentWaypointHasPassed_TestCase{
			public static object[] validCases = {
				new object[]{
					1f,
					19f,
					20f,
					true
				},
				new object[]{
					0f,
					20f,
					20f,
					true
				},
				new object[]{
					19.999f,
					.001f,
					20f,
					true
				},
				new object[]{
					1f,
					18f,
					20f,
					false
				},
				new object[]{
					.1f,
					19.899f,
					20f,
					false
				},
			};
		}
	/* MoveFollowerToTargetWaypoint */
		[Test, TestCaseSource(typeof(MoveFollowerToTargetWaypoint_TestCase), "cases")]
		public void MoveFollowerToTargetWaypoint_LerpsFollowerPosition(
			float elapsedTime,
			float requiredTime,
			Vector3 initialPosition,
			Vector3 targetPosition,
			Vector3 expectedPosition
		){
			IFollowWaypointProcessConstArg arg = CreateMockConstArg();
			TestFollowWaypointProcess process = new TestFollowWaypointProcess(arg);

			process.SetElapsedTime_Test(elapsedTime);
			process.SetRequiredTime_Test(requiredTime);
			process.SetInitPosition_Test(initialPosition);
			process.SetTargetPosition_Test(targetPosition);

			process.MoveFollowerToTargetWaypoint_Test();

			arg.follower.Received().SetPosition(expectedPosition);
		}
		static Vector3 initialPosition = new Vector3(0f, 0f, 0f);
		static Vector3 targetPosition = new Vector3(10f, 10f, 10f);
		public class MoveFollowerToTargetWaypoint_TestCase{
			public static object[] cases = {
				new object[]{
					0f,
					1f,
					initialPosition,
					targetPosition,
					new Vector3(0f, 0f, 0f)
				},
				new object[]{
					.5f,
					1f,
					initialPosition,
					targetPosition,
					new Vector3(5f, 5f, 5f)
				},
				new object[]{
					1f,
					1f,
					initialPosition,
					targetPosition,
					new Vector3(10f, 10f, 10f)
				},
				new object[]{
					10f,
					1f,
					initialPosition,
					targetPosition,
					new Vector3(10f, 10f, 10f)
				},
			};
		}
	/*  */
	

	public class TestFollowWaypointProcess: FollowWaypointProcess{
		public TestFollowWaypointProcess(IFollowWaypointProcessConstArg arg): base(arg){}
		public void SetNewTargetWaypoint_Test(){
			this.SetNewTargetWaypoint();
		}
		public IWaypoint GetTargetWaypoint_Test(){
			return thisTargetWaypoint;
		}
		public Vector3 GetInitPosition_Test(){
			return thisInitPosition;
		}
		public Vector3 GetTargetPosition_Test(){
			return thisTargetPosition;
		}
		public float GetElapsedTimeForCurrentWaypoint_Test(){
			return thisElapsedTimeForCurrentWaypoint;
		}
		public float GetRequiredTimeForCurrentWaypoint_Test(){
			return thisRequiredTimeForCurrentWaypoint;
		}
		public bool RequiredtimeForCurrentWaypointHasPassed_Test(float deltaTime){
			return this.RequiredTimeForCurrentWaypointHasPassed(deltaTime);
		}
		public void SetElapsedTime_Test(float time){
			thisElapsedTimeForCurrentWaypoint = time;
		}
		public void SetRequiredTime_Test(float time){
			thisRequiredTimeForCurrentWaypoint = time;
		}
		public void SetInitPosition_Test(Vector3 position){
			thisInitPosition = position;
		}
		public void SetTargetPosition_Test(Vector3 position){
			thisTargetPosition = position;
		}
		public void MoveFollowerToTargetWaypoint_Test(){
			this.MoveFollowerToTargetWaypoint();
		}
	}
	public IFollowWaypointProcessConstArg CreateMockConstArg(){
		IFollowWaypointProcessConstArg arg = Substitute.For<IFollowWaypointProcessConstArg>();
		arg.processManager.Returns(Substitute.For<IProcessManager>());
		arg.follower.Returns(Substitute.For<IWaypointsFollower>());
		arg.speed.Returns(1f);
		return arg;
	}
}
