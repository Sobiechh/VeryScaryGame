using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    private GameObject objectGameOver;

    // Start is called before the first frame update
    void Start()
    {
        objectGameOver = GameObject.FindGameObjectWithTag("GameOverTag");
    }

    public void playAgain()
    {
        Scene[] scenes = SceneManager.GetAllScenes();

        // foreach (Scene sc in scenes)
        // {
        //     SceneManager.LoadScene(sc.name);
        // }
        SceneManager.LoadScene("StartScene");
        SceneManager.LoadScene("MainScene");
        SceneManager.LoadScene("CameraAndCharacter");
        SceneManager.LoadScene("StartBase");
        SceneManager.LoadScene("MENU");
    }
}
