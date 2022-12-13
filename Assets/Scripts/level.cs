using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level : MonoBehaviour
{
    //Level Progress 
    //enemyAmount = how many enemies will spawn per level, currentEnemyAmount = how many currently in the level, maxEnemies = how many can be spawned at once
    public int enemyAmount = 10;
    private int currentEnemyAmount;
    private GameObject upgradeSpawner;
    public GameObject upgrade;
    private Level_Gen levelgen;
    private bool spawned;
    private bool transitioning = false;

    //Random Enemy Spawning
    public List<GameObject> enemyTypes;
    private int enemyTypeToSpawn;
    private List<GameObject> spawnPoints = new List<GameObject>();
    public int maxEnemies = 5;
    public GameObject flying;
    public GameObject shield;
    private bool enemySpawned = false;

    //Upgrade Type Holding and Next Level Loading
    [SerializeField] private List<GameObject> doorSpawnPoints = new List<GameObject>();

    private void Start()
    {
        GetComponentsOfGameObject();
    }

    // Update is called once per frame
    void Update()
    {
        currentEnemyAmount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyAmount <= 0 && currentEnemyAmount <= 0 && !spawned && !transitioning)
        {
            transitioning = true;

            for (int i = 0; i < doorSpawnPoints.Count; i++)
            {
                int rand = Random.Range(1, 5);

                doorSpawnPoints[i].transform.Find("Cube").gameObject.SetActive(true);
                doorSpawnPoints[i].transform.Find("Cube").gameObject.GetComponent<DoorScript>().SetDoorType(rand);
            }
            Spawn();
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
            if(currentEnemyAmount < maxEnemies - spawnPoints.Count)
            {
                for (int i = 0; i < spawnPoints.Count; i++)
                {
                    if (spawnPoints[i].GetComponent<RandomEnemySpawn>().flyingEnemyOnly)
                    {
                        enemyTypeToSpawn = enemyTypes.IndexOf(flying);
                    }
                    else if(spawnPoints[i].GetComponent<RandomEnemySpawn>().shieldEnemyOnly && !enemySpawned)
                    {
                        enemySpawned = true;
                        enemyTypeToSpawn = enemyTypes.IndexOf(shield);
                    }
                    else
                    {
                        enemyTypeToSpawn = Random.Range(0, enemyTypes.Count - 2);
                    }

                    Instantiate(enemyTypes[enemyTypeToSpawn].gameObject, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
                    currentEnemyAmount += 1;
                    enemyAmount -= 1;
                }
            }

            else
            {
                RandSpawnEnemies();
            }
        }
    }

    private void RandSpawnEnemies()
    {
        if (enemyAmount > 0 && currentEnemyAmount < maxEnemies)
        {
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                int randSpawn = Random.Range(0, spawnPoints.Count - 1);

                if (spawnPoints[i].GetComponent<RandomEnemySpawn>().flyingEnemyOnly)
                {
                    enemyTypeToSpawn = enemyTypes.IndexOf(flying);
                }
                else if (spawnPoints[i].GetComponent<RandomEnemySpawn>().shieldEnemyOnly)
                {
                    enemyTypeToSpawn = enemyTypes.IndexOf(shield);
                }
                else
                {
                    enemyTypeToSpawn = Random.Range(0, enemyTypes.Count - 2);
                }

                if (i == randSpawn)
                {
                    Instantiate(enemyTypes[enemyTypeToSpawn].gameObject, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
                    currentEnemyAmount += 1;
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

        foreach (GameObject spawn in GameObject.FindGameObjectsWithTag("DoorSpawnPoint"))
        {
            doorSpawnPoints.Add(spawn);
        }
    }
}
