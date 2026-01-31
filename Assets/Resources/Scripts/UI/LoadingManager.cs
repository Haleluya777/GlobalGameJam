using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;
    [SerializeField] private GameObject Loading;
    private float currentProgress;
    private float timer;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        Loading.SetActive(true);
        StartCoroutine(LoadingAsync(sceneName));
    }

    private IEnumerator LoadingAsync(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        timer = 0f;

        while (!asyncOperation.isDone)
        {
            float realProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            timer += Time.deltaTime;
            currentProgress = Mathf.Min(realProgress, timer / 2f);

            if (asyncOperation.progress >= 0.9f && timer >= 2f)
            {
                asyncOperation.allowSceneActivation = true;
                //this.gameObject.SetActive(false);   
            }

            yield return null;
        }
        this.gameObject.SetActive(false);
    }
}
