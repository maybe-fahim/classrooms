using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject howToPlayMenu;
    public GameObject loading;

    public AudioSource buttonSound;



    void Start()
    {
        mainMenu.GetComponent<Canvas>().enabled = true;
        settingsMenu.GetComponent<Canvas>().enabled = false;
        howToPlayMenu.GetComponent<Canvas>().enabled = false; 
        loading.GetComponent<Canvas>().enabled = false; 
    }

    public void StartButton()
    {
        loading.GetComponent<Canvas>().enabled = true;
        mainMenu.GetComponent<Canvas>().enabled = false;
        buttonSound.Play();
        SceneManager.LoadScene("MainScene");
    }

    public void SettingsButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = false;
        settingsMenu.GetComponent<Canvas>().enabled = true;
    }

    public void HowToPlayButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = false;
        howToPlayMenu.GetComponent<Canvas>().enabled = true;
    }

    public void ExitGameButton()
    {
        buttonSound.Play();
        Application.Quit();
        Debug.Log("App Has Exited");
    }

    public void ReturnToMainMenuButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = true;
        settingsMenu.GetComponent<Canvas>().enabled = false;
        howToPlayMenu.GetComponent<Canvas>().enabled = false;
    }



    void Update()
    {
        
    }
}
