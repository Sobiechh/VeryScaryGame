using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameObject objectAboutMenu;
    private GameObject objectMenu;
    private GameObject objectMainMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        objectAboutMenu = GameObject.FindGameObjectWithTag("AboutMenuTag");
        objectMainMenu = GameObject.FindGameObjectWithTag("MainMenuTag");
        objectMenu = GameObject.FindGameObjectWithTag("MenuTag");
        objectAboutMenu.SetActive(false);
    }

    public void PlayGame () 
    {
        objectMenu.SetActive(false);
    }

    public void ShowAbout () {
        objectMainMenu.SetActive(false);
        objectAboutMenu.SetActive(true);
    }

    public void BackToMainMenu () {
        objectAboutMenu.SetActive(false);
        objectMainMenu.SetActive(true);
    }
}
