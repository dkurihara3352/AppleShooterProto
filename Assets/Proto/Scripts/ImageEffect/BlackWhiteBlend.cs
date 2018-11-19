using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlackWhiteBlend : MonoBehaviour {

	public Material bwMaterial;
	[Range(0f, 1f)]
	public float intensity = 0f;
	void OnRenderImage(RenderTexture source, RenderTexture dest){
		if(intensity == 0f){
			Graphics.Blit(source, dest);
			return;
		}else{
			bwMaterial.SetFloat("_BWBlend", intensity);
			Graphics.Blit(source, dest, bwMaterial);
		}
	}
}
