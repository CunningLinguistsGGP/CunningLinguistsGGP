using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused;

    // Update is called once per frame
    private void Update()
    {
        OpenPauseMenu();
    }

    private void OpenPauseMenu()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            isPaused = !isPaused;

            if(isPaused)
            {
                pausePanel.SetActive(true);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
