using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpinner : MonoBehaviour {
	public float spinPerSecond = 10f;
	void Update(){
		this.transform.Rotate(Vector3.up * spinPerSecond, Space.Self);
	}
}
