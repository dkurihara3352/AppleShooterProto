using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneLoader : MonoBehaviour {
	void OnEnable(){
		// SceneManager.sceneLoaded += OnSceneLoad;
		SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
	}
	// public void OnSceneLoad(Scene scene, LoadSceneMode mode){
	// 	if(scene.buildIndex == 1){
	// 		SceneManager.SetActiveScene(scene);
	// 	}
	// }
}
