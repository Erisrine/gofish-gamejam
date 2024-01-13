using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	[SerializeField] private Animator loadingScreenAnimator;
	[SerializeField] private TMP_Text progressText;
	[SerializeField] private Slider progressSlider;
	
   public static LevelManager Instance {get; private set; }
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else 
		{
			Instance = this;
		}
	}
	
	public void LoadLevel(string scene, bool loadingScreen)
	{
		//load level
		if(!loadingScreen)
		{
			SceneManager.LoadSceneAsync(scene);
			Debug.Log("Loading level " + scene +" without loading screen");
		}
		else
		{
			StartCoroutine(LoadSceneWithLoadingScreen(scene));
		}
	}
	
	private IEnumerator LoadSceneWithLoadingScreen(string scene)
	{
		Debug.Log("Loading level " + scene +" with loading screen");
		progressText.text = "";
		progressSlider.value = 0;
		loadingScreenAnimator.SetTrigger("SlideIn");
		yield return new WaitForSeconds(1.5f);
		AsyncOperation loadScene = SceneManager.LoadSceneAsync(scene);
		loadScene.allowSceneActivation = false;
		
		while (!loadScene.isDone)
		{
			float progress = Mathf.RoundToInt(Mathf.Lerp (0f, 100f, Mathf.InverseLerp (0f, 0.9f, loadScene.progress)));
			progressText.text = progress + "%";
			progressSlider.value = progress;
			
			
			if (loadScene.progress >= 0.9f)
			{
				Debug.Log("Scene " + scene + " loaded.");
				yield return new WaitForSeconds(3f);
				loadScene.allowSceneActivation = true;
				Debug.Log("Scene " + scene + " activated.");
			}
			
			yield return null;
		}
		yield return null;
		loadingScreenAnimator.SetTrigger("SlideOut");
		Debug.Log("Loading done.");
	}
	
}
