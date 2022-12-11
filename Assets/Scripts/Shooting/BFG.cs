using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFG : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private bool infiniteAmmo = true;
    [SerializeField] private float shootCooldown = 30.0f, shotSpeed = 10.0f, targetRange = 50.0f;
    [SerializeField] private int maxShots = 5, shotsLeft = 5;
    [SerializeField] private Transform projectileSpawn;

    [SerializeField] private Camera aimCam;
    private AudioSource audioShot;

    private GameObject projectile;
    private bool canShoot = true, charging = false, currentProjectileFired = false;
    private float lastShotTime = 0.0f, chargeTime = 6.5f, chargedForTime = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        audioShot = GetComponent<AudioSource>();
        lastShotTime = -shootCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if ((lastShotTime + shootCooldown < Time.time)&& (shotsLeft > 0))
        {
            //Debug.Log("setShootTrue");
            canShoot = true;
        }
        else
        {
            //Debug.Log("setShootfalse");
            canShoot = false;
        }
        if(Input.GetButton("Fire1") || (Input.GetAxis("Fire1") != 0))
        {
            if(canShoot&&!charging)
            {
                charging = true;
                audioShot.Play();
                //if(!projectile.activeSelf)
                //    projectile.SetActive(true);
                projectile = Instantiate(projectilePrefab, projectileSpawn);
                projectile.GetComponent<Rigidbody>().detectCollisions = false;
            }
            if(charging)
            {
                chargedForTime += Time.deltaTime;
            }
            if(chargedForTime>chargeTime)
            {
                currentProjectileFired = true;
                charging = false;
                chargedForTime = 0.0f;
                Shoot();
            }
        }
        if(Input.GetButtonUp("Fire1") || (Input.GetAxis("Fire1") != 1))
        {
            //Debug.Log("up");
            if(!currentProjectileFired)
            {
                //Debug.Log("here");
                audioShot.Stop();
                if (projectile != null)
                {
                    projectile.SetActive(false);
                }
            }
            charging = false;
            chargedForTime = 0.0f;
        }
    }
    void Shoot()
    {
        projectile.transform.SetParent(projectile.transform.parent.parent.parent.parent);
        projectile.GetComponent<Rigidbody>().detectCollisions = true;
        projectile.transform.parent = null;

        Vector3 forward = aimCam.transform.forward;
        Vector3 rayDir = forward;
        int layerMask = 1 << 5;
        layerMask = ~layerMask;
        
        Ray ray = new Ray(aimCam.transform.position, rayDir);
        RaycastHit hit;


        Vector3 targetPos = ray.GetPoint(targetRange);
        if (Physics.Raycast(aimCam.transform.position, rayDir, out hit, 100000f, layerMask, QueryTriggerInteraction.Ignore))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {

                targetPos = hit.point;

            }
        }
        projectile.GetComponent<Rigidbody>().velocity = rayDir.normalized * shotSpeed;


        if (!infiniteAmmo)
        {
            shotsLeft--;
        }
        if (shotsLeft <= 0)
        {
            canShoot = false;
        }


        lastShotTime = Time.time;
    }
    void AddAmmo(int amount)
    {
        shotsLeft += amount;
        if (shotsLeft > maxShots)
            shotsLeft = maxShots;
    }
}
