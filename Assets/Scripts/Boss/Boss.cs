using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float shotCooldown;

    private float timer;

    private GameObject player;
    private PlayerScript playerHealth;
    private Vector3 lastPlayerPos = Vector3.zero;
    private Vector3 playerPredVel = Vector3.zero;
    private Vector3 playerPredPos = Vector3.zero;

    private bool isVulnerable = false;

    public float healthRatio = 1.0f;
    private float bossStartupTimer = 5.0f;
    private bool activeShooter = false;

    [SerializeField] private GameObject[] shields;

    [SerializeField] private GameObject projectile;
    [SerializeField] private float shotSpeed = 10.0f;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private GameObject projectileDirector;

    private int difficulty = 1;//1 = easy,2=mid,3=hard
    private int phase = 0;

    int shotSwitch = 0;

    private Level_Gen levelGen;

    [SerializeField] private ParticleSystem mzzlFlash;

    void Start()
    {

        levelGen = GameObject.Find("Level_Gen").GetComponent<Level_Gen>();
        difficulty = levelGen.GetDifficulty();

        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerScript>();

    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > bossStartupTimer)
        { 
            activeShooter = true; 
        }

        isVulnerable = !ShieldCheck();

        switch (difficulty)
        {
            //easy
            case 1:
                break;
            //mid
            case 2:
                if (healthRatio < 0.5f)
                {
                    ShieldsUp();
                    phase++;
                }

                break;
            //hard
            case 3:
                if(phase==0)
                {
                    if (healthRatio < 0.667f)
                    {
                        phase = 1;
                        ShieldsUp();
                    }
                }
                if(phase == 1)
                {
                    if (healthRatio < 0.334f)
                    {
                        phase = 2;
                        ShieldsUp();
                    }
                }
                break;
            //default to easy
            default:
                break;
        }

        if (timer >= shotCooldown&& playerHealth.currentHealth>0&&activeShooter)
        {
            timer = 0.0f;
            Shoot();
        }

        lastPlayerPos = player.transform.position;
    }

    public bool GetIsVulnerable()
    {
        return isVulnerable;
    }

    void ShieldsUp()
    {
        for (int i = 0; i < shields.Length; i++)
        {
            shields[i].SetActive(true);
            shields[i].GetComponent<Target>().ResetHealth();
        }
    }
    bool ShieldCheck()
    {
        bool res = false;
        for (int i = 0; i < shields.Length; i++)
        {
            if (shields[i].activeSelf)
                res = true;
        }
        return res;
    }

    void Shoot()
    {
        shotSwitch++;
        shotSwitch %= 2;
        switch (phase)
        {
            case 0:
                ShootDumb();
                break;
            case 1:
                ShootPredict();
                break;
            case 2:
                if(shotSwitch==0)
                {
                    ShootDumb();
                }
                else
                {
                    ShootPredict();
                }
                break;
            default: 
                ShootDumb();
                break;
        }
    }

    void ShootPredict()
    {
        Vector3 dispPlayer = this.transform.position - player.transform.position;
        playerPredVel = (player.transform.position - lastPlayerPos) / Time.deltaTime;

        float a = playerPredVel.sqrMagnitude - shotSpeed * shotSpeed;
        float b = Vector3.Dot(dispPlayer, playerPredVel);
        float c = dispPlayer.sqrMagnitude;
        float det = b * b - c * a;
        float t1 = (b + Mathf.Sqrt(det)) / a;
        float t2 = (b - Mathf.Sqrt(det)) / a;
        if (t1 > 0 && t2 > 0)
        {
            if (1000 * timer % 2 == 0)
            {
                playerPredPos = t1 * playerPredVel + player.transform.position;
            }
            else
            {
                playerPredPos = t2 * playerPredVel + player.transform.position;
            }
        }
        else if (t1 > 0)
        {
            playerPredPos = t1 * playerPredVel + player.transform.position;
        }
        else if (t2 > 0)
        {
            playerPredPos = t2 * playerPredVel + player.transform.position;
        }
        else
        {
            playerPredPos = player.transform.position;
        }

        projectileDirector.transform.LookAt(playerPredPos);
        GameObject newProjectile = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
        newProjectile.GetComponent<Rigidbody>().velocity = (playerPredPos - projectileSpawn.position).normalized * shotSpeed;
        Destroy(newProjectile, 2.0f);

    }

    void ShootDumb()
    {
        projectileDirector.transform.LookAt(player.transform.position);
        GameObject newProjectile = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
        newProjectile.GetComponent<Rigidbody>().velocity = (player.transform.position - projectileSpawn.position).normalized * shotSpeed;
        Destroy(newProjectile, 2.0f);
    }

}
