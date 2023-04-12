using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour
{

    public GameObject loadingScreen;
    public Slider slider;
    public TMP_Text progressText;

    public int loadSceneIndex = 0;
    private float _waitTime = 1f;

    void Start()
    {
        StartCoroutine(LoadAsynchronously(loadSceneIndex));
    }
  
    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        yield return new WaitForSeconds(_waitTime);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);         
        loadingScreen.SetActive(true);
        
        /////////IT WORKS !!!!!!!
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }

    }

   
}
