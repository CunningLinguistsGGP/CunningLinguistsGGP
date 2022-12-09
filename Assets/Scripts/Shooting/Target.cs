using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Target : MonoBehaviour
{
    [SerializeField] private float health = 50.0f;

    //Floating Damage Numbers
    [SerializeField] private GameObject DamageTextPrefab;

    //Grappling
    private bool doubleDamage;
    private bool dead = false;

    //Score
    public ScoreSystem s_s;

    public int enemy_type;

    //Difficulty Settings
    private Level_Gen levelGen;
    [SerializeField] private float easyHealth = 10;
    [SerializeField] private float mediumHealth = 30;
    [SerializeField] private float hardHealth = 50;

    private HitScanGun revolver;
    private HitScanGun shotgun;

    void Start()
    {
        s_s = GameObject.Find("Level_Gen").GetComponent<ScoreSystem>();

        levelGen = GameObject.Find("Level_Gen").GetComponent<Level_Gen>();

        revolver = Camera.main.gameObject.transform.Find("Revolver").GetComponent<HitScanGun>();
        shotgun = Camera.main.gameObject.transform.Find("Shotgun").GetComponent<HitScanGun>();

        if (levelGen.GetDifficulty() == 1)
        {
            health = easyHealth;
        }
        else if (levelGen.GetDifficulty() == 2)
        {
            health = mediumHealth;
        }
        else
        {
            health = hardHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        if (doubleDamage || revolver.GetCrit() == true || shotgun.GetCrit())
        {
            damage = damage * revolver.GetCritDamageMultiplier();

            ShowDamageText(transform.position, damage);

            health -= damage;
            if (health <= 0)
            {
                dead = true;
                //StartCoroutine(Dies());
                Die();
            }
        }

        else
        {
            health -= damage;

            ShowDamageText(transform.position, damage);

            if (health <= 0)
            {
                dead = true;
                //StartCoroutine(Dies());
                Die();
            }
        }
    }

    private void ShowDamageText(Vector3 enemy, float damage)
    {
        if(doubleDamage || revolver.GetCrit() == true || shotgun.GetCrit())
        {
            GetDamageText().color = Color.yellow;
        }
        else
        {
            GetDamageText().color = Color.white;
        }

        int randX = Random.Range(-1, 1);
        Vector3 textPos = new Vector3(enemy.x + randX, enemy.y + 1, enemy.z);
        DamageTextPrefab.GetComponent<TextMeshPro>().text = damage.ToString();
        Instantiate(DamageTextPrefab, textPos, Camera.main.transform.rotation);
    }

    public bool SetDoubleDamage(bool value)
    {
        return doubleDamage = value;
    }

    void Die()
    {
        s_s.ScoreSet(enemy_type);

        Destroy(gameObject);
    }

    public bool GetIsDead()
    {
        return dead;
    }

    IEnumerator Dies()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public TextMeshPro GetDamageText()
    {
        return DamageTextPrefab.GetComponent<TextMeshPro>();
    }
}
