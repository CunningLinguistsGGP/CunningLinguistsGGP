using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanGun : MonoBehaviour
{
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float range = 100.0f;
    [SerializeField] private float shots = 1;
    [SerializeField] private float armorPen = 0.0f;
    [SerializeField] private float coneAngle = 3.0f;
    [SerializeField] private GameObject mzzlFlash;
    private ParticleSystem flash;
    private float shotDeviation = 0.0f;

    [SerializeField] private Camera aimCam;
    [SerializeField] private float shootCooldown = 1.0f;

    [SerializeField] AudioSource audioShot;


    private float lastShotTime = 0.0f;
    void Start()
    {
        flash = mzzlFlash.GetComponent<ParticleSystem>();
        audioShot = GetComponent<AudioSource>();
        shotDeviation = Mathf.Tan(coneAngle);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    
    void Shoot()
    {
        Vector3 forward = aimCam.transform.forward;
        Vector3 up = aimCam.transform.up;
        Vector3 right = aimCam.transform.right;
        Vector3 rayDir = forward;

        RaycastHit hit;
        if(lastShotTime+shootCooldown<Time.time)
        {
            flash.Stop();
            flash.Play();
            audioShot.Play();
            if (shots == 1)
            {
                if (Physics.Raycast(aimCam.transform.position, rayDir, out hit, range))
                {
                    Target target = hit.transform.GetComponent<Target>();
                    if (target != null)
                    {
                        target.TakeDamage(damage);
                    }
                }
            }
            else
            {
                for (int i = 0; i < shots; i++)
                {
                    rayDir = Vector3.Normalize(forward+shotDeviation * (Random.Range(-1.0f, 1.0f) * right + Random.Range(-1.0f, 1.0f) * up));
                    if (Physics.Raycast(aimCam.transform.position, rayDir, out hit, range))
                    {
                        Target target = hit.transform.GetComponent<Target>();
                        if (target != null)
                        {
                            target.TakeDamage(damage);
                        }
                    }
                }
            }
            lastShotTime = Time.time;
        }
    }
}