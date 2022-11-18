using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level_Gen : MonoBehaviour
{
    public static Level_Gen instance;

    //[SerializeField] int[] Levels;

   // [SerializeField] GameObject Player;

    [SerializeField]List<int> level_selection;

   // GameObject Previous_Level;
   // GameObject Present_Level;

    //[SerializeField]GameObject Level_SpwanPoint;

    //GameObject Player_SpwanPoint;

    [SerializeField]GameObject Black_Fade;

    [SerializeField] float Fade_speed;

    int random;

    private void Start()
    {
        instance = this;

        Fade_speed = 1f; // Default. 1 second

        //level_selection = new List<int>();

       /* for(int i=0; i< Levels.Length; i++)
        {
            level_selection.Add(i);
        }*/

        DontDestroyOnLoad(this);
    }


    public void Next_level()
    {
        StartCoroutine(start_fade(true));

        //if (Present_Level != null) { Previous_Level = Present_Level; Previous_Level.gameObject.SetActive(false); }

        random = Random.Range(0, level_selection.Count);

        SceneManager.LoadScene(level_selection[random]);

        level_selection.RemoveAt(random);

        // Present_Level = Instantiate(Levels[level_selection[random]], Level_SpwanPoint.transform);

        // Player_SpwanPoint = Present_Level.transform.Find("PlayerSpwanPoint").gameObject;

        //Player.transform.position = Player_SpwanPoint.transform.position;



         StartCoroutine(start_fade(false));
    }

    public IEnumerator start_fade(bool status)
    {
        for (var t = 0f; t < 1; t += Time.deltaTime / Fade_speed)
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

        yield return new WaitForSeconds(1);
    }
}
