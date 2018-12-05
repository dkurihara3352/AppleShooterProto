using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBase{
	public interface IFrostGlassAdaptor: IMonoBehaviourAdaptor{
		IFrostGlass GetFrostGlass();
		float GetProcessTime();
		float GetFrostValue();
		void SetFrostValue(float value);
	}
	public class FrostGlassAdaptor : MonoBehaviourAdaptor, IFrostGlassAdaptor {
		public override void SetUp(){
			thisFrostGlass = CreateFrostGlass();
			thisMaterial = CollectMaterial();
			thisRadiusHash = Shader.PropertyToID("_Radius");
			thisColorHash = Shader.PropertyToID("_Color");
		}
		public override void FinalizeSetUp(){
			SetFrostValue(initialFrost);
		}
		[Range(0f, 1f)]
		public float initialFrost = 1f;
		IFrostGlass thisFrostGlass;
		public IFrostGlass GetFrostGlass(){
			return thisFrostGlass;
		}
		IFrostGlass CreateFrostGlass(){
			FrostGlass.IConstArg arg = new FrostGlass.ConstArg(
				this
			);
			return new FrostGlass(arg);
		}
		public float processTime;
		public float GetProcessTime(){
			return processTime;
		}
		public UnityEngine.UI.Image image;
		Material CollectMaterial(){
			return image.material;
		}
		Material thisMaterial;
		int thisRadiusHash;
		int thisColorHash;
		public float originalImageAlpha = .2f;
		public float GetFrostValue(){
			return thisMaterial.GetFloat(thisRadiusHash) / maxRadius;
		}
		public void SetFrostValue(float frostValue){
			float newRadius = CalcRadius(frostValue);
			thisMaterial.SetFloat(thisRadiusHash, newRadius);
			Color newColor = CalcColor(frostValue);
			thisMaterial.SetColor(thisColorHash, newColor);
		}
		public float maxRadius = 10f;
		float CalcRadius(float frostValue){
			return Mathf.Lerp(
				0f, 
				maxRadius,
				frostValue
			);
		}
		Color CalcColor(float frostValue){
			float newAlpha = Mathf.Lerp(
				0f,
				originalImageAlpha,
				frostValue
			);
			Color col = thisMaterial.GetColor(thisColorHash);
			col.a = newAlpha;
			return col;
		}
	}
}
