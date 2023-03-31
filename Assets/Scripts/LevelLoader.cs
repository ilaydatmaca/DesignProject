using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
   public GameObject loadingScreen;
   public Slider slider;

   private void Awake()
   {
      LoadLevel(1);
   }

   public void LoadLevel(int sceneIndex)
   {
      StartCoroutine(AsynchronousLoad(sceneIndex));
   }
   
   IEnumerator AsynchronousLoad(int sceneIndex)
   {
      AsyncOperation ao = SceneManager.LoadSceneAsync(sceneIndex);
      loadingScreen.SetActive(true);
      while (! ao.isDone)
      {
         float progress = Mathf.Clamp01(ao.progress / .9f);
         slider.value = progress;
         yield return null;
      }
   }
   
}
