using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DKUtility;
using NUnit.Framework;
using NSubstitute;

[TestFixture, Category("DKUtility")]
public class ProcessComparerTest{
	[Test, TestCaseSource(typeof(Sort_TestCase), "cases")]
	public void Sort_Test(
		int[] processOrderes,
		int[] expected
	){
		List<IProcess> processList = new List<IProcess>();
		foreach(int processOrder in processOrderes){
			IProcess process = Substitute.For<IProcess>();
			process.GetProcessOrder().Returns(processOrder);
			processList.Add(process);
		}

		AbsProcess.IProcessComparer comparer = new AbsProcess.ProcessComparer();

		processList.Sort(comparer);

		int[] actualProcessOrders = new int[processOrderes.Length];
		int count = 0;
		foreach(IProcess process in processList){
			actualProcessOrders[count ++] = process.GetProcessOrder();
		}

		Assert.That(actualProcessOrders, Is.EqualTo(expected));
	}
	public class Sort_TestCase{
		public static object[] cases = {
			new object[]{
				new int[]{-1, -1, -1, 2, 0, 4, -1},
				new int[]{-1, -1, -1, -1, 0, 2, 4}
			},
			new object[]{
				new int[]{3, 8, 2, 4, 4, 4, -1, -10},
				new int[]{-10, -1, 2, 3, 4, 4, 4, 8}
			},
		};
	}
}
