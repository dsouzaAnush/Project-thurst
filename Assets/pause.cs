using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pause : MonoBehaviour {

	public GameObject pausePanel;
	// Use this for initialization
	public void pauseGame(){
		Time.timeScale=0;
		pausePanel.SetActive(true);
	}

	public void resumeGame(){
		pausePanel.SetActive(false);
		Time.timeScale=1;
	}

	public void mMenu(){
		pausePanel.SetActive(false);
		Time.timeScale=1;
		  SceneManager.LoadScene("menu");
	}
	public void lMenu(){
		pausePanel.SetActive(false);
		Time.timeScale=1;
			SceneManager.LoadScene("Levels");
	}
}
