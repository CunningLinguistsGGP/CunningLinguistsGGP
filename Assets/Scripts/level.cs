using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level : MonoBehaviour
{
    [SerializeField]private int enemyAmount;

    public Transform spawner;
    public GameObject upgrade;
    private Level_Gen levelgen;

    private bool spawned;

    private void Start()
    {
        levelgen = GameObject.Find("Level_Gen").GetComponent<Level_Gen>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyAmount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyAmount == 0 && spawned == false)
        {
            Spawn();
            //levelgen.Next_level();
            StartCoroutine(loadnextlevel());
        }
    }

    private void Spawn()
    {
        Instantiate(upgrade, spawner.transform.position, Quaternion.identity);
        spawned = true;
    }

    IEnumerator loadnextlevel()
    {
        yield return new WaitForSeconds(10);
        levelgen.Next_level();
    }
}
