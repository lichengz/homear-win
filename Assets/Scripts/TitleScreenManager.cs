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
    // [SerializeField]
    // [Range(0, 1)]
    // float progressAnimationMultiplier = 0.25f;
    // AsyncOperation loadOperation;
    // float currentValue = 0;
    // float targetValue = 0;
    // bool startLoading = false;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // if (!startLoading) return;
        // targetValue = loadOperation.progress / 0.9f;
        // currentValue = Mathf.MoveTowards(currentValue, targetValue, progressAnimationMultiplier * Time.deltaTime);
        // slider.value = currentValue;
        // if (Mathf.Approximately(currentValue, 1))
        // {
        //     loadOperation.allowSceneActivation = true;
        // }
    }

    public void LoadGameScene()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(GetSceneLoadProgress());
        // loadOperation = SceneManager.LoadSceneAsync((int)SceneIndexes.GAME);
        // loadOperation.allowSceneActivation = false;
        // startLoading = true;
    }

    public IEnumerator GetSceneLoadProgress()
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync((int)SceneIndexes.GAME);
        while (!operation.isDone)
        {
            slider.value = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }

    }


}
