using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	[SerializeField] private bool pauseMenu;
	private bool isPaused;
	// Start is called before the first frame update
	void Awake()
	{
		Time.timeScale = 1;
		isPaused = false;
	}
	void Update()
	{
		if(pauseMenu)
		{
			if(!isPaused)
			{
				if(Input.GetKeyDown(KeyCode.Escape))
				{
					PauseGame();
				}
			}
			else
			{
				if(Input.GetKeyDown(KeyCode.Escape))
				{
					UnpauseGame();
				}
			}
		}
	}
	
	void PauseGame()
	{
		DialogueSystem.playerControl = false;
		Time.timeScale = 0;
	}
	void UnpauseGame()
	{
		DialogueSystem.playerControl = true;
		Time.timeScale = 1;
	}
	
	public void NewGame()
	{
		LoadLevel("level_1", true);
	}
	
	public void LoadLevel(string scene, bool loadingScreen)
	{
		LevelManager.Instance.LoadLevel(scene, loadingScreen);
	}
}
