using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    [SerializeField] float transitionTime = 1f;

    private static SceneLoader instance;  // Singleton instance
    private string previousScene;  // Store the previous scene name
    // public SceneLoader Instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static SceneLoader Instance
    {
        get
        {
            // if (instance == null)
            // {
            //     GameObject singletonObject = new GameObject("SceneLoaderSingleton");
            //     instance = singletonObject.AddComponent<SceneLoader>();
            //     DontDestroyOnLoad(singletonObject);
            // }
            return instance;
        }
    }

    // void Update()
    // {
    //     if (Input.GetKey(KeyCode.Space))
    //     {
    //         LoadBattleScene();
    //     }
    //     if (Input.GetKey(KeyCode.B))
    //     {
    //         LoadPreviousScene();
    //     }
    // }

    public void LoadPreviousScene()
    {
        Debug.Log(previousScene);
        StartCoroutine(LoadScene(previousScene));
    }

    public void LoadBattleScene()
    {
        previousScene = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadScene("BattleScene"));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        previousScene = SceneManager.GetActiveScene().name;
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
        transition.SetTrigger("End");
    }
}
