using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public AnimationClip fadeBlackAnim;

	// Use this for initialization
	void Start () {
        Invoke("LoadGame", fadeBlackAnim.length);
	}
	
	void LoadGame()
    {
        SceneManager.LoadScene("LabFloor");
    }
}
