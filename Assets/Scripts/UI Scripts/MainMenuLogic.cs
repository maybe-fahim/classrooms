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
    public GameObject DifficultyMenu;

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
        
        mainMenu.GetComponent<Canvas>().enabled = false;
        DifficultyMenu.GetComponent<Canvas>().enabled = true;
        buttonSound.Play();
        
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
    public void SetEasyDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", 0); // 0 = Easy
        PlayerPrefs.Save();
        Debug.Log("Easy difficulty selected.");
        loading.GetComponent<Canvas>().enabled = true;
        SceneManager.LoadScene("Room_PCG");
    }

    public void SetMediumDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", 1); // 1 = Medium
        PlayerPrefs.Save();
        Debug.Log("Medium difficulty selected.");
        loading.GetComponent<Canvas>().enabled = true;
        SceneManager.LoadScene("Room_PCG");
    }

    public void SetHardDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", 2); // 2 = Hard
        PlayerPrefs.Save();
        Debug.Log("Hard difficulty selected.");
        loading.GetComponent<Canvas>().enabled = true;
        SceneManager.LoadScene("Room_PCG");
    }
}
