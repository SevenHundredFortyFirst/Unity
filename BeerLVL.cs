using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeerLVL : MonoBehaviour
{
    public int Point;
    public Slider slider;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Point += 100;          
        }

    }
    private void Update()
    {
        slider.value = Point;
        if (Point >= 1000)
        {
            LoadNextScene();
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
