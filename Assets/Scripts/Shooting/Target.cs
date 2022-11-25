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
    [SerializeField] private bool doubleDamage;

    //Score
    public ScoreSystem s_s;

    public int enemy_type;

    void Start()
    {
        s_s = GameObject.Find("Level_Gen").GetComponent<ScoreSystem>();
    }

    public void TakeDamage(float damage)
    {
        if (doubleDamage)
        {
            damage = damage * 2;

            ShowDamageText(transform.position, damage);

            health -= damage;
            if (health <= 0)
            {

                Die();
            }
        }

        else
        {
            health -= damage;

            ShowDamageText(transform.position, damage);

            if (health <= 0)
            {

                Die();
            }
        }
    }

    private void ShowDamageText(Vector3 enemy, float damage)
    {
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
}
