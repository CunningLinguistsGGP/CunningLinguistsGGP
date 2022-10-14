using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private Transform pausePanel;
    private bool isPaused;

    private void Start()
    {
        GetComponentsInGameObject();
    }

    // Update is called once per frame
    private void Update()
    {
        OpenPauseMenu();
    }

    private void GetComponentsInGameObject()
    {
        pausePanel = transform.Find("Options");
    }

    private void OpenPauseMenu()
    {
        if(Input.GetButtonDown("Pause"))
        {
            isPaused = !isPaused;

            if(isPaused)
            {
                pausePanel.gameObject.SetActive(true);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                pausePanel.gameObject.SetActive(false);
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
