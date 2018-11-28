using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using UISystem;
using DKUtility;

[TestFixture, Category("UISystem")]
public class ScrollerElementSnapProcessTest {

    [Test, TestCaseSource(typeof(UpdateProcess_Demo_TestCase), "cases"), Ignore]
    public void UpdateProcess_Demo(Vector2 initialLocalPos, float targetLocalPosOnAxis, float initialVelOnAxis, int dimension, float diffThreshold, float stopVelocity){
        TestScrollerElementSnapProcess.IConstArg arg = CreateMockConstArg();
        IUIElement scrollerElement = Substitute.For<IUIElement>();
        arg.scrollerElement.Returns(scrollerElement);
        Vector3 initialLocalPosV3 = initialLocalPos;
        scrollerElement.GetLocalPosition().Returns(initialLocalPosV3);
        arg.dimension.Returns(dimension);
        arg.targetElementLocalPositionOnAxis.Returns(targetLocalPosOnAxis);
        arg.initialVelocityOnAxis.Returns(initialVelOnAxis);
        TestScrollerElementSnapProcess process = new TestScrollerElementSnapProcess(arg);

        float deltaT = .02f;
        bool done = false;
        Vector2 passedLocalPos = Vector2.zero;
        scrollerElement.SetLocalPosition(Arg.Do<Vector2>(x => passedLocalPos = x));

        float prevLocalPosOnAxis = initialLocalPos[dimension];
        
        DKUtility.DebugHelper.PrintInRed("initPos: " + initialLocalPos.ToString() + ", targetPosOnAxis: " + targetLocalPosOnAxis.ToString() + ", initVel: " + initialVelOnAxis.ToString()+ ", dimension: " + dimension.ToString());
        
        float delta;

        while(!done){
            process.UpdateProcess(deltaT);
            delta = Mathf.Abs(passedLocalPos[dimension] - prevLocalPosOnAxis) /deltaT;
            Debug.Log("scrollerElementLocalPos: " + passedLocalPos.ToString() + ", delta: " + delta.ToString());

            if(delta <= stopVelocity)
                done = true;
            
            prevLocalPosOnAxis = passedLocalPos[dimension];

        }
    }
    public class UpdateProcess_Demo_TestCase{
        public static object[] cases = {
            new object[]{new Vector2(0f, 0f), 20f, 0f, 0, .1f, .5f},
            new object[]{new Vector2(0f, 0f), 20f, 10f, 0, .1f, .5f},
            new object[]{new Vector2(0f, 0f), 20f, 100f, 0, .1f, .5f},
            new object[]{new Vector2(0f, 0f), 20f, -100f, 0, .1f, .5f},
        };
    }








    public class TestScrollerElementSnapProcess: ScrollerElementSnapProcess{
        public TestScrollerElementSnapProcess(IConstArg arg): base(arg){}
        public float GetDiffThreshold_Test(){
            return thisDiffThreshold;
        }
        public new interface IConstArg: ScrollerElementSnapProcess.IConstArg{}
    }
    TestScrollerElementSnapProcess.IConstArg CreateMockConstArg(){
        TestScrollerElementSnapProcess.IConstArg arg = Substitute.For<TestScrollerElementSnapProcess.IConstArg>();
        
        arg.processManager.Returns(Substitute.For<IProcessManager>());
        arg.scroller.Returns(Substitute.For<IScroller>());
        arg.scrollerElement.Returns(Substitute.For<IUIElement>());
        arg.dimension.Returns(0);

        arg.targetElementLocalPositionOnAxis.Returns(0f);
        arg.initialVelocityOnAxis.Returns(0f);
        
        return arg;
    }
}
