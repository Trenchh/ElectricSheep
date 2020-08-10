using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    /*private void Update()
    {
        Debug.Log(PauseMenu.GameIsPaused);
    }*/
    public void PlayGame()
    {
        SceneManager.LoadScene("ElectricSheep");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
