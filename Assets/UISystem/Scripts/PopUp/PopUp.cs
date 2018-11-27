using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IPopUp: IUIElement, IPopUpStateHandler, IPopUpStateImplementor{
		void SetPopUpManager(IPopUpManager manager);
		void SetParentPopUp(IPopUp parent);
		void AddChildPopUp(IPopUp child);

		bool HidesOnTappingOthers();
		void ShowHiddenProximateParentPopUpRecursively();
		void HideShownChildPopUpsRecursively();

		IPopUp GetParentPopUp();
		bool IsHidden();
		bool IsShown();
		bool IsAncestorOf(IPopUp other);

		void LogHierarchy();
	}
	public enum PopUpMode{
		Alpha,
	}
	public class PopUp : UIElement, IPopUp, IPopUpStateImplementor {
		public PopUp(
			IConstArg arg
		): base(
			arg
		){
			thisHidesOnTappingOthers = arg.hidesOnTappingOthers;
			thisStateEngine = CreateStateEngine(
				arg.processTime,
				arg.popUpMode
			);
			thisTypedAdaptor.ToggleRaycastBlock(false);
		}
		IPopUpManager thisPopUpManager;
		public void SetPopUpManager(IPopUpManager manager){
			thisPopUpManager = manager;
		}

		public bool HidesOnTappingOthers(){
			return thisHidesOnTappingOthers;
		}
		readonly bool thisHidesOnTappingOthers;
		protected readonly IPopUpStateEngine thisStateEngine;
		IPopUpStateEngine CreateStateEngine(
			float processTime,
			PopUpMode popUpMode
		){
			
			PopUpStateEngine.IConstArg arg = new PopUpStateEngine.ConstArg(
				thisUISystemProcessFactory,
				this,
				processTime
			);
			return new PopUpStateEngine(arg);
		}

		IPopUp thisParentPopUp;
		public void SetParentPopUp(IPopUp parent){
			thisParentPopUp = parent;
		}
		public IPopUp GetParentPopUp(){
			return thisParentPopUp;
		}
		IPopUp[] thisChildrenPopUp = new IPopUp[]{};
		public void AddChildPopUp(IPopUp childPopUp){
			List<IPopUp> resultList = new List<IPopUp>(thisChildrenPopUp);
			resultList.Add(childPopUp);
			thisChildrenPopUp = resultList.ToArray();
		}
		public void LogHierarchy(){
			Debug.Log(
				GetName() + "'s parentPopUp: " + GetParentPopUpName() + ", " + 
				"children: " + GetChildPopUpNameArray()
			);
		}
		string GetParentPopUpName(){
			if(thisParentPopUp == null)
				return "null";
			return thisParentPopUp.GetName();
		}
		string GetChildPopUpNameArray(){
			string result = "";
			foreach(IPopUp childPopUp in thisChildrenPopUp)
				result += childPopUp.GetName() + ", ";
			return result;
		}
		public void Hide(bool instantly){
			thisStateEngine.Hide(instantly);
		}
		public void Show(bool instantly){
			thisStateEngine.Show(instantly);
		}
		public bool IsHidden(){
			return thisStateEngine.IsHidden();
		}
		public bool IsShown(){
			return thisStateEngine.IsShown();
		}
		public virtual void OnShowBegin(){
			thisPopUpManager.RegisterPopUp(this);
			thisTypedAdaptor.ToggleRaycastBlock(true);
		}
		public virtual void OnHideBegin(){
			thisPopUpManager.UnregisterPopUp(this);
			thisTypedAdaptor.ToggleRaycastBlock(false);
		}
		public virtual void OnShowComplete(){
			thisTypedAdaptor.ToggleRaycastBlock(true);
		}
		public virtual void OnHideComplete(){
			thisTypedAdaptor.ToggleRaycastBlock(false);
		}
		public float GetAlpha(){
			return thisTypedAdaptor.GetGroupAlpha();
		}
		public void SetAlpha(float alpha){
			thisTypedAdaptor.SetGroupAlpha(alpha);
		}

		IPopUpAdaptor thisTypedAdaptor{
			get{
				return (IPopUpAdaptor)thisAdaptor;
			}
		}
		public void ShowHiddenProximateParentPopUpRecursively(){
			if(thisParentPopUp != null){
				if(thisParentPopUp.IsHidden()){
					thisParentPopUp.Show(false);
					thisParentPopUp.ShowHiddenProximateParentPopUpRecursively();
				}
			}
		}
		public void HideShownChildPopUpsRecursively(){
			foreach(IPopUp childPopUp in thisChildrenPopUp){
				if(childPopUp.IsActivated() && childPopUp.IsShown()){
					childPopUp.Hide(false);
					childPopUp.HideShownChildPopUpsRecursively();
				}
			}
		}
		public bool IsAncestorOf(IPopUp other){
			IPopUp popUpToExamine = other.GetParentPopUp();
			while(true){
				if(popUpToExamine == null)
					return false;
				if(popUpToExamine == this)
					return true;
				popUpToExamine = popUpToExamine.GetParentPopUp();
			}
		}
		protected override void OnTapImple(int tapCount){
			CheckAndPerformStaticBoundarySnapFrom(this);
			return;
		}


		public new interface IConstArg: UIElement.IConstArg{
			bool hidesOnTappingOthers{get;}
			PopUpMode popUpMode{get;}
			float processTime{get;}
		}
		public new class ConstArg: UIElement.ConstArg, IConstArg{
			public ConstArg(
				IPopUpAdaptor adaptor,
				ActivationMode activationMode,

				bool hidesOnTappingOthers,
				PopUpMode popUpMode,
				float processTime
				
			): base(
				adaptor,
				activationMode
			){
				thisHidesOnTappingOthers = hidesOnTappingOthers;
				thisPopUpMode = popUpMode;
				thisProcessTime = processTime;
			}
			readonly bool thisHidesOnTappingOthers;
			public bool hidesOnTappingOthers{get{return thisHidesOnTappingOthers;}}
			readonly PopUpMode thisPopUpMode;
			public PopUpMode popUpMode{get{return thisPopUpMode;}}
			readonly float thisProcessTime;
			public float processTime{get{return thisProcessTime;}}

		}
	}

}
