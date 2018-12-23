﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using DKUtility;

namespace AppleShooterProto{
	public interface IResourcePanel: IUIElement, IProcessHandler{
		void Show();
		void Hide();
		bool IsShown();
		void HideInstantly();
	}
	public class ResourcePanel: UIElement, IResourcePanel{
		public ResourcePanel(IConstArg arg): base(arg){
			thisProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisResourcePanelAdaptor.GetProcessTime()
			);
		}
		IResourcePanelAdaptor thisResourcePanelAdaptor{
			get{
				return (IResourcePanelAdaptor)thisUIAdaptor;
			}
		}
		public void Show(){
			BecomeFocusedInScrollerRecursively();
			thisIsShown = true;
			thisInitialAlpha = thisResourcePanelAdaptor.GetAlpha();
			thisTargetAlpha = thisResourcePanelAdaptor.GetShowAlpha();

			thisInitialLocalPosition = GetLocalPosition();
			thisTargetLocalPosition = thisResourcePanelAdaptor.GetShowLocalPosition();

			thisProcessSuite.Start();
		}
		void MakeAllChildrenSelectable(){
			foreach(IUIElement childUIE in this.GetChildUIElements())
				childUIE.BecomeSelectable();
		}
		public void Hide(){
			thisIsShown = false;
			thisInitialAlpha = thisResourcePanelAdaptor.GetAlpha();
			thisTargetAlpha = thisResourcePanelAdaptor.GetHideAlpha();

			thisInitialLocalPosition = GetLocalPosition();
			thisTargetLocalPosition = thisResourcePanelAdaptor.GetHideLocalPosition();

			thisProcessSuite.Start();
		}
		bool thisIsShown = false;
		public bool IsShown(){
			return thisIsShown;
		}
		
		IProcessSuite thisProcessSuite;
		Vector3 thisInitialLocalPosition;
		Vector3 thisTargetLocalPosition;
		float thisInitialAlpha;
		float thisTargetAlpha;
		public void OnProcessRun(IProcessSuite suite){
		}
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisProcessSuite){
				AnimationCurve processCurve = thisResourcePanelAdaptor.GetProcessCurve();
				float processValue = processCurve.Evaluate(normalizedTime);

				Vector3 newLocalPosition = Vector3.Lerp(
					thisInitialLocalPosition,
					thisTargetLocalPosition,
					processValue
				);
				SetLocalPosition(newLocalPosition);

				float newAlpha = Mathf.Lerp(
					thisInitialAlpha,
					thisTargetAlpha,
					processValue
				);
				thisResourcePanelAdaptor.SetAlpha(newAlpha);
			}
		}
		public void OnProcessExpire(
			IProcessSuite suite
		){
			thisResourcePanelAdaptor.SetAlpha(thisTargetAlpha);
			SetLocalPosition(thisTargetLocalPosition);
		}
		public void HideInstantly(){
			SetLocalPosition(thisResourcePanelAdaptor.GetHideLocalPosition());
			thisResourcePanelAdaptor.SetAlpha(thisResourcePanelAdaptor.GetHideAlpha());
			thisIsShown = false;
		}
	}
}
