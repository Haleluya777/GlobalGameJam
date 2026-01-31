using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;
    [SerializeField] private GameObject Loading;

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
        //StartCoroutine(LoadingAsync(sceneName));
    }

    private IEnumerator LoadingAsync(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (true)
        {
            if (asyncOperation.isDone)
            {
                asyncOperation.allowSceneActivation = true;
                break;
            }
        }
        yield return null;
    }
}
