using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_Gen : MonoBehaviour
{
    public static Level_Gen instance;

    GameObject Previous_Level;

    GameObject Present_Level;

    GameObject Player_SpwanPoint;

    [SerializeField]GameObject Black_Fade;

    [SerializeField] float Fade_speed;

    private void Start()
    {
        instance = this;

        Fade_speed = 1f; // Default. 1 second
    }


    public void Next_level()
    {
        StartCoroutine(start_fade(true));
    }

    public IEnumerator start_fade(bool status)
    {
        for(var t = 0f; t< 1; t += Time.deltaTime / Fade_speed)
        {
            var tempColor = Black_Fade.GetComponent<Image>().color;

            Black_Fade.SetActive(true);

            switch (status)
            {
                case true:
                    tempColor.a = Mathf.Lerp(0f, 1f, t);
                    Black_Fade.GetComponent<Image>().color = tempColor;
                    Debug.Log(tempColor.a);
                    break;
                case false:
                    tempColor.a = Mathf.Lerp(1f, 0f, t);
                    Black_Fade.GetComponent<Image>().color = tempColor;
                    break;
            }

            

            yield return null;
        }

        Black_Fade.SetActive(status == true ? true : false);

        // yield return new WaitForSeconds(1);
    }
}
