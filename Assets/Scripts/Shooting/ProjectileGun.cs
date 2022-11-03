using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private bool requiresReload = false, infiniteAmmo = true;
    [SerializeField] private float shootCooldown = 1.0f, shotSpeed = 10.0f, reloadTime=2.0f,targetRange = 50.0f;
    [SerializeField] private int magShots=3,totalShots = 30;
    [SerializeField] private Transform projectileSpawn;


    [SerializeField] private ParticleSystem mzzlFlash;
    [SerializeField] private Camera aimCam;
    [SerializeField] AudioSource audioShot;

    private bool canShoot = true;
    private int shotsLeft = 0;
    private float lastShotTime = 0.0f,reloadStart=0.0f;


    // Start is called before the first frame update
    void Start()
    {
        lastShotTime = -shootCooldown;
        reloadStart = -reloadTime;
        if (requiresReload)
            shotsLeft = magShots;
        else
            shotsLeft = totalShots;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastShotTime + shootCooldown < Time.time)
            Debug.Log("shotCooldownOver");
        if (reloadStart + reloadTime < Time.time)
            Debug.Log("reloadOver");
        if (totalShots > 0)
            Debug.Log("enoughShots");
        if ((lastShotTime + shootCooldown < Time.time) && (reloadStart+reloadTime<Time.time)&&(totalShots>0))
        {
            Debug.Log("setShootTrue");
            canShoot = true;
        }
        else
        {
            Debug.Log("setShootfalse");
            canShoot = false;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (canShoot)
            {
                Debug.Log("shoot");
                Shoot();
            }
            else if(reloadStart + reloadTime < Time.time)
            {
                Debug.Log("autoReload");
                if(shotsLeft<magShots)
                    Reload();
            }
        }
        if (Input.GetButton("Reload"))
        {
            Debug.Log("manualReload");
            Reload();
        }
        Debug.Log("reloadTiming: " + (reloadStart + reloadTime - Time.time));
        //Debug.Log("magShots: " + magShots);
        //Debug.Log("totalShots: " + totalShots);
        //Debug.Log("shotsLeft: " + shotsLeft);
        //Debug.Log("lastShotTiming: " + (lastShotTime + shootCooldown - Time.time));
    }
    void Shoot()
    {
        if (shotsLeft <= 0||totalShots<=0)
            return;
        Vector3 forward = aimCam.transform.forward;
        Vector3 rayDir = forward;

        Ray ray = new Ray(aimCam.transform.position, rayDir);
        RaycastHit hit;

        if(mzzlFlash!=null)
            mzzlFlash.Play();
        if(audioShot!=null)
        {
            audioShot.Play();
            audioShot.SetScheduledEndTime(AudioSettings.dspTime + shootCooldown);
        }


        Vector3 targetPos = ray.GetPoint(targetRange);
        if (Physics.Raycast(aimCam.transform.position, rayDir, out hit))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                
                targetPos = hit.point;
                
            }
        }
        GameObject newProjectile = Instantiate(projectile, projectileSpawn.position, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody>().velocity = rayDir.normalized * shotSpeed;


        shotsLeft--;
        if (!infiniteAmmo)
        {
            totalShots--;
        }
        if(shotsLeft<=0&&totalShots>0)
        {
            canShoot = false;
        }


        lastShotTime = Time.time;
    }

    void Reload()
    {
        if(requiresReload)
        {
            canShoot = false;
            shotsLeft = magShots;
            reloadStart = Time.time;
        }
    }
}
