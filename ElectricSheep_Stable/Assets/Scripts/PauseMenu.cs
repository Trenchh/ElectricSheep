using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    //public bool isDead = false;
    //public bool isWin = false;
    public GameObject deathMenuUI;
    public GameObject winMenuUI;
    public TextMeshProUGUI endText;
    public TextMeshProUGUI endText2;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI winText2;
    public GameObject audio;
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GameIsPaused);
        if (Input.GetKeyDown(KeyCode.Escape))// && isDead == false)
        {
            if(optionsMenuUI.activeSelf)
            {
                optionsMenuUI.SetActive(false);
                pauseMenuUI.SetActive(true);
            }
            else
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        /*if (PlayerAttack.healthCount < 1 && isWin == false)
        {
            Death();
        }
        
        if (BossBehaviour.HP < 1 && isDead == false && BossHeartChange.inRoom == true)
        {
            Win();
        }*/
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Death ()
    {
        //isDead = true;
        GameIsPaused = true;
        deathMenuUI.SetActive(true);
        Time.timeScale = 0;
        endText.text = FindObjectOfType<PlayerAttack>().finalText;
        endText2.text = FindObjectOfType<PlayerAttack>().finalText;
    }

    public void Win()
    {
        //isWin = true;
        GameIsPaused = true;
        winMenuUI.SetActive(true);
        Time.timeScale = 0;
        winText.text = FindObjectOfType<PlayerAttack>().finalText;
        winText2.text = FindObjectOfType<PlayerAttack>().finalText;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
    }

    public void Options()
    {

    }

    public void Restart()
    {
        winMenuUI.SetActive(false);
        //BossHeartChange.inRoom = false;
        GameIsPaused = false;
        audio.GetComponent<AudioSource>().Stop();
        //isDead = false;
        //isWin = false;
        SceneManager.LoadScene("ElectricSheep");
        Time.timeScale = 1f;
    }

    public void QuitToMenu()
    {
        winMenuUI.SetActive(false);
        //BossHeartChange.inRoom = false;
        GameIsPaused = false;
        audio.GetComponent<AudioSource>().Stop();
        //isDead = false;
        //isWin = false;
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }
}

