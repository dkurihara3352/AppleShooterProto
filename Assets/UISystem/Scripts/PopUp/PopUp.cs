using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IPopUp: IUIElement, IPopUpEventTrigger{
		void SetPopUpManager(IPopUpManager manager);
		void SetParentPopUp(IPopUp parent);
		void AddChildPopUp(IPopUp child);

		void OnHideBegin();
		void OnShowBegin();
		void OnHideComplete();
		void OnShowComplete();

		bool HidesOnTappingOthers();
		void ShowHiddenProximateParentPopUpRecursively();
		void HideShownChildPopUpsRecursively();

		IPopUp GetParentPopUp();
		// IPopUp GetProximateParentPopUp();
		// void RegisterProximateChildPopUp(IPopUp childPopUp);
		bool IsHidden();
		bool IsShown();
		bool IsAncestorOf(IPopUp other);
		// void SetUpPopUpHierarchy();

	}
	public enum PopUpMode{
		Alpha,
	}
	public class PopUp : UIElement, IPopUp {
		public PopUp(
			IConstArg arg
		): base(
			arg
		){
			// thisPopUpManager = arg.popUpManager;

			thisHidesOnTappingOthers = arg.hidesOnTappingOthers;
			thisStateEngine = CreateStateEngine(arg.popUpMode);

			
			// if(arg.popUpMode == PopUpMode.Alpha)
			// 	this.GetUIAdaptor().SetUpCanvasGroupComponent();
			// GetPopUpAdaptor().ToggleRaycastBlock(false);
		}
		// protected virtual IPopUpAdaptor GetPopUpAdaptor(){
		// 	return (IPopUpAdaptor)GetUIAdaptor();
		// }
		IPopUpManager thisPopUpManager;
		public void SetPopUpManager(IPopUpManager manager){
			thisPopUpManager = manager;
		}

		public bool HidesOnTappingOthers(){
			return thisHidesOnTappingOthers;
		}
		readonly bool thisHidesOnTappingOthers;
		protected readonly IPopUpStateEngine thisStateEngine;
		IPopUpStateEngine CreateStateEngine(PopUpMode popUpMode){
			IPopUpStateEngineConstArg popUpStateEngineConstArg = new PopUpStateEngineConstArg(
				thisUISystemProcessFactory,
				this,
				thisPopUpManager,
				popUpMode
			);
			return new PopUpStateEngine(popUpStateEngineConstArg);
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
		public virtual void OnShowBegin(){}
		public virtual void OnHideBegin(){}
		public virtual void OnShowComplete(){}
		public virtual void OnHideComplete(){}
		// public void SetUpPopUpHierarchy(){
		// 	thisProximateParentPopUp = FindProximateParentPopUp();

		// 	if(thisProximateParentPopUp != null)
		// 		thisProximateParentPopUp.RegisterProximateChildPopUp(this);
		// 	thisProximateChildPopUps = new List<IPopUp>();
		// }
		// protected virtual IPopUp FindProximateParentPopUp(){
		// 	return FindProximateParentTypedUIElement<IPopUp>();
		// }
		// IPopUp thisProximateParentPopUp;
		// public IPopUp GetProximateParentPopUp(){
		// 	return thisProximateParentPopUp;
		// }
		public void ShowHiddenProximateParentPopUpRecursively(){
			if(thisParentPopUp != null){
				if(thisParentPopUp.IsHidden()){
					thisParentPopUp.Show(false);
					thisParentPopUp.ShowHiddenProximateParentPopUpRecursively();
				}
			}
		}
		// List<IPopUp> thisProximateChildPopUps;
		// public void RegisterProximateChildPopUp(IPopUp childPopUp){
		// 	thisProximateChildPopUps.Add(childPopUp);
		// }
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
		}
		public new class ConstArg: UIElement.ConstArg, IConstArg{
			public ConstArg(
				IPopUpAdaptor adaptor,
				ActivationMode activationMode,

				bool hidesOnTappingOthers,
				PopUpMode popUpMode
				
			): base(
				adaptor,
				activationMode
			){
				thisHidesOnTappingOthers = hidesOnTappingOthers;
				thisPopUpMode = popUpMode;
			}
			readonly bool thisHidesOnTappingOthers;
			public bool hidesOnTappingOthers{get{return thisHidesOnTappingOthers;}}
			readonly PopUpMode thisPopUpMode;
			public PopUpMode popUpMode{get{return thisPopUpMode;}}

		}
	}

}
