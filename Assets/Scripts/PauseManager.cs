using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to your pause menu UI panel.
    public bool isPaused = false;
    public Animator pauseMenuAnimation;
    public CinemachineFreeLook cinemachineCamera;
    public GameObject player;

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed && isPaused)
        {
            ResumeGame();
        }
        else if (context.performed && !isPaused)
        {
            PauseGame();
        }
    }


    void PauseGame()
    {
        //   Time.timeScale = 0;
        cinemachineCamera.GetComponent<CinemachineInputProvider>().enabled = false;
        player.GetComponent<Animator>().speed = 0f;
        isPaused = true;
        pauseMenu.SetActive(true);
        pauseMenuAnimation.SetTrigger("pause");
        Cursor.visible = true;
    }

    void ResumeGame()
    {
        StartCoroutine(ResumeGameCo());
    }

    IEnumerator ResumeGameCo()
    {
        pauseMenuAnimation.SetTrigger("resume");

        yield return new WaitForSeconds(0.3f);

        cinemachineCamera.GetComponent<CinemachineInputProvider>().enabled = true;
        player.GetComponent<Animator>().speed = 1f;
        Time.timeScale = 1;
        isPaused = false;
        pauseMenu.SetActive(false);
        Cursor.visible = false;
    }

    public void Resume()
    {
        ResumeGame();
    }

    public void Options()
    {

    }

    public void GameQuit()
    {

    }
}
