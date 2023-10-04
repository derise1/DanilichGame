using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public bool PauseGame;
    public GameObject pauseGameMenu;
    public GameObject settOpen;
    public GameObject uiPlayer;
    public GameObject levelOver;
    public GameObject startText;
    public GameObject diePanel;
    public bool levelEnd;
    public bool die;

    private void Start() {
        StartLevel();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(!levelEnd && !die){
                if(PauseGame){
                    Resume();
                    startText.SetActive(false);
                }
                else{
                    PauseMenu();
                }
            }
        }
    }
    
    public void Die(){
        die = true;
        PauseGame = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        diePanel.SetActive(true);
        pauseGameMenu.SetActive(false);
        settOpen.SetActive(false);
        uiPlayer.SetActive(false);
    }

    public void PlayAgain(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void StartLevel(){
        Cursor.lockState = CursorLockMode.Locked;
        startText.SetActive(true);
        pauseGameMenu.SetActive(false);
        settOpen.SetActive(false);
        uiPlayer.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = true;
    }

    public void LevelEnd(){
        Cursor.lockState = CursorLockMode.Confined;
        levelEnd = true;
        PauseGame = true;
        levelOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume(){
        Cursor.lockState = CursorLockMode.Locked;
        pauseGameMenu.SetActive(false);
        settOpen.SetActive(false);
        uiPlayer.SetActive(true);
        Time.timeScale = 1f;
        PauseGame = false;
    }

    public void SettOpen(){
        settOpen.SetActive(true);
        pauseGameMenu.SetActive(false);
        uiPlayer.SetActive(false);
    }

    public void Back(){
        pauseGameMenu.SetActive(true);
        settOpen.SetActive(false);
        uiPlayer.SetActive(false);
    }

    public void PauseMenu(){
        Cursor.lockState = CursorLockMode.Confined;
        pauseGameMenu.SetActive(true);
        settOpen.SetActive(false);
        uiPlayer.SetActive(false);
        Time.timeScale = 0f;
        PauseGame = true;
    }

    public void LoadMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
