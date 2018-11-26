using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using UISystem;
using DKUtility;

[TestFixture, Category("UISystem")]
public class ProximateParentScrollerCalculatorTest {
	[Test]
	public void Calculate_NoParentUIE_ReturnsNull(){
		IUIElement uie = Substitute.For<IUIElement>();
		IUIElement parent = null;
		uie.GetParentUIElement().Returns(parent);

		IProximateParentTypedUIECalculator<ISomeTestUIElement> calculator = new ProximateParentTypedUIECalculator<ISomeTestUIElement>(uie);

		Assert.That(calculator.Calculate(), Is.Null);
	}
	[Test]
	public void Calculate_HasNonScrollerParent_ReturnsNull(){
		IUIElement uie = Substitute.For<IUIElement>();
		IUIElement parent = Substitute.For<IUIElement>();
		IUIElement nullParent = null;

		uie.GetParentUIElement().Returns(parent);
		parent.GetParentUIElement().Returns(nullParent);

		IProximateParentTypedUIECalculator<ISomeTestUIElement> calculator = new ProximateParentTypedUIECalculator<ISomeTestUIElement>(uie);

		Assert.That(calculator.Calculate(), Is.Null);
	}
	[Test]
	public void Calculate_HasScrollerParentDirectlyAbove_ReturnsIt(){
		IUIElement uie = Substitute.For<IUIElement>();
		ISomeTestUIElement typedParent = Substitute.For<ISomeTestUIElement>();
		IUIElement nullParent = null;

		uie.GetParentUIElement().Returns(typedParent);
		typedParent.GetParentUIElement().Returns(nullParent);

		IProximateParentTypedUIECalculator<ISomeTestUIElement> calculator = new ProximateParentTypedUIECalculator<ISomeTestUIElement>(uie);

		Assert.That(calculator.Calculate(), Is.SameAs(typedParent));
	}
	[Test]
	public void Calculate_HasScrollerParentNotDirecltyAbove_ReturnsIt(){
		IUIElement uie = Substitute.For<IUIElement>();
		IUIElement wrongTypedParent = Substitute.For<IUIElement>();
		ISomeTestUIElement typedParent = Substitute.For<ISomeTestUIElement>();
		IUIElement nullParent = null;

		uie.GetParentUIElement().Returns(wrongTypedParent);
		wrongTypedParent.GetParentUIElement().Returns(typedParent);
		typedParent.GetParentUIElement().Returns(nullParent);

		IProximateParentTypedUIECalculator<ISomeTestUIElement> calculator = new ProximateParentTypedUIECalculator<ISomeTestUIElement>(uie);

		Assert.That(calculator.Calculate(), Is.SameAs(typedParent));
	}
	[Test]
	public void Calculate_HasMultipleScrollerParentAbove_ReturnsTheColosest(){
		IUIElement uie = Substitute.For<IUIElement>();
		ISomeTestUIElement closeTypedParent = Substitute.For<ISomeTestUIElement>();
		ISomeTestUIElement farTypedParent = Substitute.For<ISomeTestUIElement>();
		IUIElement nullParent = null;

		uie.GetParentUIElement().Returns(closeTypedParent);
		closeTypedParent.GetParentUIElement().Returns(farTypedParent);
		farTypedParent.GetParentUIElement().Returns(nullParent);

		IProximateParentTypedUIECalculator<ISomeTestUIElement> calculator = new ProximateParentTypedUIECalculator<ISomeTestUIElement>(uie);

		Assert.That(calculator.Calculate(), Is.SameAs(closeTypedParent));
	}

	public interface ISomeTestUIElement: IUIElement{}
}
