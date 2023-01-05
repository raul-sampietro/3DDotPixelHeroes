using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "MainMenuScene":
                    Quit();
                    break;

                case "MainScene":
                case "InstructionsScene":
                case "CreditsScene":
                case "GameOverScene":
                case "WinScene":
                    BackToMenu();
                    break;

                default:
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "MainMenuScene":
                    Play();
                    break;

                case "MainScene":
                    break;

                case "InstructionsScene":
                case "CreditsScene":
                    break;

                case "GameOverScene":
                case "WinScene":
                    Play();
                    break;

                default:
                    break;
            }
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void Win()
    {
        SceneManager.LoadScene("WinScene");
    }

    public void Instructions()
    {
        SceneManager.LoadScene("InstructionsScene");
    }

    public void Credits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
