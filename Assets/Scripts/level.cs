using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level : MonoBehaviour
{
    //Level Progress
    [SerializeField] private int enemyAmount = 10;
    private int currentEnemyAmount;
    [SerializeField]private GameObject upgradeSpawner;
    public GameObject upgrade;
    private Level_Gen levelgen;
    private bool spawned;

    //Random Enemy Spawning
    public List<GameObject> enemyTypes;
    private int enemyTypeToSpawn;
    public List<GameObject> spawnPoints = new List<GameObject>();
    public int maxEnemies = 5;

    private void Start()
    {
        GetComponentsOfGameObject();
    }

    // Update is called once per frame
    void Update()
    {
        currentEnemyAmount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (currentEnemyAmount == 0 && spawned == false)
        {
            Spawn();
            //levelgen.Next_level();
            StartCoroutine(loadnextlevel());
        }
        else
        {
            SpawnEnemies();
        }
    }

    private void Spawn()
    {
        if(upgradeSpawner != null)
        {
            Instantiate(upgrade, upgradeSpawner.transform.position, Quaternion.identity);
            spawned = true; 
        }
        else
        {
            Debug.LogError("Upgrade Spawner Missing");
        }
    }

    private void SpawnEnemies()
    {
        if (enemyAmount > 0 && currentEnemyAmount < maxEnemies)
        {
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                if(spawnPoints[i].GetComponent<RandomEnemySpawn>().flyingEnemyOnly)
                {
                    enemyTypeToSpawn = enemyTypes.Count - 1;
                }
                else
                {
                    enemyTypeToSpawn = Random.Range(0, enemyTypes.Count - 1);
                }
                for (int j = 0; j < maxEnemies - currentEnemyAmount; j++)
                {
                    Instantiate(enemyTypes[enemyTypeToSpawn].gameObject, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
                    enemyAmount -= 1;
                }
            }
        }
    }

    private void GetComponentsOfGameObject()
    {
        levelgen = GameObject.Find("Level_Gen").GetComponent<Level_Gen>();
        upgradeSpawner = GameObject.Find("UpgradeSpawner").gameObject;
        foreach(GameObject spawn in GameObject.FindGameObjectsWithTag("SpawnPoint"))
        {
            spawnPoints.Add(spawn);
        }
    }

    IEnumerator loadnextlevel()
    {
        yield return new WaitForSeconds(10);
        levelgen.Next_level();
    }
}
