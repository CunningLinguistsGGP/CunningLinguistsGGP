using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject switcher;
    public GameObject difficulties;

    private Level_Gen levelGen;

    private void Start()
    {
        levelGen = GameObject.Find("Level_Gen").GetComponent<Level_Gen>();
    }

    public void PlayGame()
    {
        switcher.SetActive(false);
        difficulties.SetActive(true);
    }

    public void BackButton()
    {
        switcher.SetActive(true);
        difficulties.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void EasyButton()
    {
        levelGen.SetDifficulty(1);
        levelGen.Next_level();
    }

    public void MediumButton()
    {
        levelGen.SetDifficulty(2);
        levelGen.Next_level();
    }

    public void HardButton()
    {
        levelGen.SetDifficulty(3);
        levelGen.Next_level();
    }
}