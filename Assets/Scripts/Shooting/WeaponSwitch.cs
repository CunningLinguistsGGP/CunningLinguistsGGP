using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;
    private int currentActive = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (i == currentActive)
                gameObjects[i].SetActive(true);
            else
                gameObjects[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("WeaponSwitch"))
        {
            Switch();
        }
    }

    void Switch()
    {
        if(Input.GetAxis("WeaponSwitch")>0)
        {
            gameObjects[currentActive].SetActive(false);
            currentActive++;
            currentActive%=gameObjects.Length;
            gameObjects[currentActive].SetActive(true);
        }
        else if(Input.GetAxis("WeaponSwitch") < 0)
        {
            gameObjects[currentActive].SetActive(false);
            currentActive--;
            currentActive %= gameObjects.Length;
            gameObjects[currentActive].SetActive(true);
        }
    }
}
