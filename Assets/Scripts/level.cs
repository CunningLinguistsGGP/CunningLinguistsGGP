using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level : MonoBehaviour
{
    [SerializeField]private int enemyAmount;

    public Transform spawner;
    public GameObject upgrade;

    private bool spawned;

    // Update is called once per frame
    void Update()
    {
        enemyAmount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyAmount == 0 && spawned == false)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        Instantiate(upgrade, spawner.transform.position, Quaternion.identity);
        spawned = true;
    }
}
