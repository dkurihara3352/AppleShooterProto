using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface IArrowTrailAdaptor: IAppleShooterMonoBehaviourAdaptor{
		void SetArrowTrailReserveAdaptor(IArrowTrailReserveAdaptor adaptor);
		IArrowTrail GetArrowTrail();
		void SetAlpha(float alpha);
		float GetAlpha();
		void SetColor(Color color);
		void EnableTrailRenderer();
		void DisableTrailRenderer();
		void ResetAlpha();
		void ClearRenderer();
	}
	[RequireComponent(typeof(TrailRenderer))]
	public class ArrowTrailAdaptor: AppleShooterMonoBehaviourAdaptor, IArrowTrailAdaptor{
		public override void SetUp(){
			thisTrail = CreateTrail();
			thisTrailRenderer = CollectTrailRenderer();
			thisMaterial = CollectMaterial();
			thisColorID = CollectColorID();
			thisDefaultAlpha = GetAlpha();
		}
		IArrowTrail thisTrail;
		IArrowTrail CreateTrail(){
			ArrowTrail.IConstArg arg = new ArrowTrail.ConstArg(
				this,
				fadeTime
			);
			return new ArrowTrail(arg);
		}
		public IArrowTrail GetArrowTrail(){
			return thisTrail;
		}
		public float fadeTime;
		public override void SetUpReference(){
			IArrowTrailReserve arrowTrailReserve = thisArrowTrailReserveAdaptor.GetArrowTrailReserve();
			thisTrail.SetArrowTrailReserve(arrowTrailReserve);
		}
		public override void FinalizeSetUp(){
			thisTrail.Deactivate();
		}
		IArrowTrailReserveAdaptor thisArrowTrailReserveAdaptor;
		public void SetArrowTrailReserveAdaptor(IArrowTrailReserveAdaptor adaptor){
			thisArrowTrailReserveAdaptor = adaptor;
		}

		TrailRenderer thisTrailRenderer;
		TrailRenderer CollectTrailRenderer(){
			return GetComponent<TrailRenderer>();
		}
		Material thisMaterial;
		Material CollectMaterial(){
			return thisTrailRenderer.material;
		}
		int thisColorID;
		int CollectColorID(){
			return Shader.PropertyToID("_TintColor");
		}
		public void SetAlpha(float alpha){
			Color col = thisMaterial.GetColor(thisColorID);
			col.a = alpha;
			thisMaterial.SetColor(thisColorID, col);
		}
		public float GetAlpha(){
			Color col = thisMaterial.GetColor(thisColorID);
			return col.a;
		}

		public void SetColor(Color color){
			Gradient gradient = new Gradient();
			gradient.SetKeys(
				new GradientColorKey[]{
					new GradientColorKey(color, 0f),
					new GradientColorKey(color, 1f)
				},
				new GradientAlphaKey[]{
					new GradientAlphaKey(1f, 0f),
					new GradientAlphaKey(0f, 1f)
				}
			);
			thisTrailRenderer.colorGradient = gradient;
		}

		public void EnableTrailRenderer(){
			thisTrailRenderer.enabled = true;
		}
		public void DisableTrailRenderer(){
			thisTrailRenderer.enabled = false;
		}
		float thisDefaultAlpha;
		public void ResetAlpha(){
			SetAlpha(thisDefaultAlpha);
		}
		public void ClearRenderer(){
			thisTrailRenderer.Clear();
		}
	}
}
