using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using UnityEngine.UI;

namespace SlickBowShooting{
	public interface IResourcePanelAdaptor: IUIAdaptor{
		IResourcePanel GetResourcePanel();
		float GetAlpha();
		void SetAlpha(float alpha);
		float GetShowAlpha();
		float GetHideAlpha();
		Vector3 GetShowLocalPosition();
		Vector3 GetHideLocalPosition();
		AnimationCurve GetProcessCurve();
		float GetProcessTime();
	}
	[RequireComponent(typeof(CanvasGroup))]
	public class ResourcePanelAdaptor: UIAdaptor, IResourcePanelAdaptor{
		public override void SetUp(){
			base.SetUp();
			thisCanvasGroup = GetComponent<CanvasGroup>();
		}
		public override void FinalizeSetUp(){
			base.FinalizeSetUp();
			thisResourcePanel.HideInstantly();
		}
		void MakeAllChildrenSelectable(){
			foreach(IUIElement child in GetChildUIElements())
				child.BecomeSelectable();
		}
		protected override IUIElement CreateUIElement(){
			ResourcePanel.IConstArg arg = new ResourcePanel.ConstArg(
				this,
				activationMode
			);
			return new ResourcePanel(arg);
		}
		IResourcePanel thisResourcePanel{
			get{
				return (IResourcePanel)thisUIElement;
			}
		}
		public IResourcePanel GetResourcePanel(){
			return thisResourcePanel;
		}

		public float GetAlpha(){
			return thisCanvasGroup.alpha;
		}
		public void SetAlpha(float alpha){
			thisCanvasGroup.alpha = alpha;
		}
		public float showAlpha = 1f;
		public float GetShowAlpha(){
			return showAlpha;
		}
		public float hideAlpha = 0f;
		public float GetHideAlpha(){
			return hideAlpha;
		}
		public Vector3 hideOffset = new Vector3(0f, 100f, 0f);
		public Vector3 GetShowLocalPosition(){
			return thisShowLocalPosition;
		}
		Vector3 thisShowLocalPosition{
			get{
				return showPositionAdaptor.GetLocalPosition();
			}
		}
		public UIAdaptor showPositionAdaptor;
		public Vector3 GetHideLocalPosition(){
			return thisHideLocalPosition;
		}
		Vector3 thisHideLocalPosition{
			get{
				return hidePositionAdaptor.GetLocalPosition();
			}
		}
		public UIAdaptor hidePositionAdaptor;
		public AnimationCurve processCurve;
		public AnimationCurve GetProcessCurve(){
			return processCurve;
		}
		public float processTime = 1f;
		public float GetProcessTime(){
			return processTime;
		}
	}
}

