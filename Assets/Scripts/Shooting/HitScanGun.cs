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

    [SerializeField] private Camera aimCam;
    [SerializeField] private float shootCooldown = 1.0f;

    [SerializeField] AudioSource audioShot;


    private float lastShotTime = 0.0f;
    void Start()
    {
        audioShot = GetComponent<AudioSource>();
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
        RaycastHit hit;
        if(lastShotTime+shootCooldown<Time.time)
        {
            audioShot.Play();
            if (shots == 1)
            {
                if (Physics.Raycast(aimCam.transform.position, aimCam.transform.forward, out hit, range))
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
                    if (Physics.Raycast(aimCam.transform.position, aimCam.transform.forward, out hit, range))
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
