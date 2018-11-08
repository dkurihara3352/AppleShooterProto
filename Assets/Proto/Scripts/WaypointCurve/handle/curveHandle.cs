using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class curveHandle : MonoBehaviour {

	public Color handleColor;
	void OnDrawGizmos(){
		Gizmos.color = handleColor;
		Gizmos.DrawCube(this.transform.position, Vector3.one);
	}
}
