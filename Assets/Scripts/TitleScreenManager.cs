using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HomeAR.Managers;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;

    public void LoadGameScene()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(GetSceneLoadProgress());
    }

    public IEnumerator GetSceneLoadProgress()
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync((int)SceneIndexes.GAME);
        while (!operation.isDone)
        {
            slider.value = Mathf.Clamp01(operation.progress);
            yield return null;
        }

    }


}
