using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : PersistentMonoBehaviour
{
    #region Singleton
    public static SceneLoader instance;
    protected override void Awake()
    {
        if (instance == null)
        {
            instance = this;
            base.Awake();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public GameObject loadingScreen;
    private GameObject loadingScreenInstance;
    private Slider slider;

    public void LoadLevel(int sceneIndex)
    {
        SetUpLoadScreen();
        SceneStateManager.Instance.OnSceneUnload();
        StartCoroutine(LoadAsync(sceneIndex));
        loadingScreenInstance.SetActive(true);
    }

    IEnumerator LoadAsync (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }

    void SetUpLoadScreen()
    {
        loadingScreenInstance = GameObject.Find("LoadingScreen");
        if (loadingScreenInstance == null)
        {
            loadingScreenInstance = Instantiate(loadingScreen);
        }
        slider = loadingScreenInstance.GetComponentInChildren<Slider>();
        loadingScreenInstance.SetActive(false);
    }

    public int GetSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
