using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("TestKnightScene");
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
}
