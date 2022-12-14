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

    [SerializeField] ScoreSystem scoreSystem;

    [SerializeField]List<int> level_selection;

   // GameObject Previous_Level;
   // GameObject Present_Level;

    //[SerializeField]GameObject Level_SpwanPoint;

    //GameObject Player_SpwanPoint;

    [SerializeField]GameObject Black_Fade;

    [SerializeField] float Fade_speed;

    int random;

    //Difficulty Setting
    private int difficultySetting = 1;

    //Upgrade Settings
    private int upgradeType = 3;

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

        scoreSystem.Set_ScoreValues(150, 40, 2);

        // Present_Level = Instantiate(Levels[level_selection[random]], Level_SpwanPoint.transform);

        // Player_SpwanPoint = Present_Level.transform.Find("PlayerSpwanPoint").gameObject;

        //Player.transform.position = Player_SpwanPoint.transform.position;
        
         StartCoroutine(start_fade(false));
    }

    //IEnumerator literalTimeWaste()
    //{

    //}

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

    public int GetDifficulty()
    {
        return difficultySetting;
    }

    public int SetDifficulty(int diffculty)
    {
        return difficultySetting = diffculty;
    }

    public int GetUpgradeType()
    {
        return upgradeType;
    }

    public int SetUpgradeType(int type)
    {
        return upgradeType = type;
    }
}

public static class GameData
{
    private static float damagePercent = 0;
    private static float critChance = 10f;
    private static float critDamageMultiplier = 2f;
    private static float speed = 10f;
    private static bool doubleJumpEnabled = false;
    private static bool canGrapple = false;
    private static int maxDashAmount = 2;
    private static float healthPercent = 0;

    public static float GetDamagePercent()
    {
        return damagePercent;
    }

    public static float SetDamagePercent(float increase)
    {
        return damagePercent += increase;
    }

    public static float GetCritChance()
    {
        return critChance;
    }
    public static float SetCritChance(float increase)
    {
        return critChance += increase;
    }

    public static float GetCritDamageMultiplier()
    {
        return critDamageMultiplier;
    }

    public static float SetCritMultiplier(float increase)
    {
        return critDamageMultiplier += increase;
    }
    public static float GetSpeed()
    {
        return speed;
    }

    public static float SetSpeed(float increase)
    {
        return speed += increase;
    }

    public static bool GetDoubleJump()
    {
        return doubleJumpEnabled;
    }
    public static bool SetDoubleJump(bool value)
    {
        return doubleJumpEnabled = value;
    }

    public static bool GetGrapple()
    {
        return canGrapple;
    }

    public static bool SetGrapple(bool value)
    {
        return canGrapple = value;
    }

    public static int GetDashAmount()
    {
        return maxDashAmount;
    }
    public static int SetDashAmount(int increase)
    {
        return maxDashAmount += increase;
    }

    public static float GetHealthPercent()
    {
        return healthPercent;
    }

    public static float SetHealthPercent(float increase)
    {
        return healthPercent += increase;
    }
}
