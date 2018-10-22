using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelLoader : MonoBehaviour
{
	[SerializeField] private GameObject loadScreen;
	[SerializeField] private Slider slider;
	[SerializeField] private Text progressText;
	
	
	public void LoadLevel(int sceneIndex)
	{
		loadScreen.SetActive(true);
		StartCoroutine(LoadLevelAsync(sceneIndex));
	}

	IEnumerator LoadLevelAsync(int sceneIndex)
	{
		AsyncOperation loadInformation = SceneManager.LoadSceneAsync(sceneIndex);
		while (!loadInformation.isDone)
		{
			var progress = Mathf.Clamp01(loadInformation.progress / 0.9f);
			slider.value = progress;
			progressText.text = progress * 100 + "%";
			Debug.Log(loadInformation.progress);
			yield return null;
		}
	}
}
