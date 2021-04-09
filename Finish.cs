using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    
    
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadNextScene();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
    private static void LoadNextScene()
    {
        int nextSceneId;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1 >= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            nextSceneId = 0;
        }
        else
        {
            nextSceneId = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
        }


        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneId);
    }
}
