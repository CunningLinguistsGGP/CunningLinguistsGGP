using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class HitScanGun : MonoBehaviour
{
    [SerializeField] private float damage = 10.0f;
    private float baseDamageValue = 10f;
    [SerializeField] private float range = 100.0f;

    [SerializeField] private float shots = 1;

    //[SerializeField] private float armorPen = 0.0f;
    [SerializeField] private float coneAngle = 3.0f;
    [SerializeField] private ParticleSystem mzzlFlash;
    [SerializeField] private GameObject hitPrefab;
    private float shotDeviation = 0.0f;

    [SerializeField] private Camera aimCam;
    [SerializeField] private float shootCooldown = 1.0f;

    [SerializeField] AudioSource audioShot;

    Controls ctrl;

    private float lastShotTime = 0.0f;

    //Crit Stuff
    private float critChance = 10f;
    private float critDamageMultiplier = 2f;
    private bool crit = false;

    //Upgrade Stuff
    private PlayerScript player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();
        audioShot = GetComponent<AudioSource>();
        shotDeviation = Mathf.Tan(coneAngle);

        ctrl = new Controls();
        ctrl.Player.Enable();
        ctrl.Player.Shoot.performed += fire;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    public void fire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Shoot();
        }
    }
    
    public void Shoot()
    {
        Vector3 forward = aimCam.transform.forward;
        Vector3 up = aimCam.transform.up;
        Vector3 right = aimCam.transform.right;
        Vector3 rayDir = forward;
        int layerMask = 1 << 5;
        layerMask = ~layerMask;

        RaycastHit hit;
        if(lastShotTime+shootCooldown<Time.time)
        {
            mzzlFlash.Play();
            audioShot.Play();
            audioShot.SetScheduledEndTime(AudioSettings.dspTime+shootCooldown);
            if (shots == 1)
            {
                if (Physics.Raycast(aimCam.transform.position, rayDir, out hit, range, layerMask, QueryTriggerInteraction.Ignore))
                {
                    Target target = hit.transform.GetComponent<Target>();
                    if (target != null)
                    {
                        float randValue = Random.Range(0, 100);

                        if(randValue < critChance)
                        {
                            crit = true;
                            target.TakeDamage(damage);
                            Instantiate(hitPrefab, hit.point, Quaternion.identity);
                            crit = false;
                        }
                        else
                        {
                            target.TakeDamage(damage);   
                            Instantiate(hitPrefab, hit.point, Quaternion.identity);
                        }  
                    }
                }
            }
            else
            {
                for (int i = 0; i < shots; i++)
                {
                    rayDir = Vector3.Normalize(forward+shotDeviation * (Random.Range(-1.0f, 1.0f) * right + Random.Range(-1.0f, 1.0f) * up));
                    if (Physics.Raycast(aimCam.transform.position, rayDir, out hit, range, layerMask, QueryTriggerInteraction.Ignore))
                    {
                        Target target = hit.transform.GetComponent<Target>();
                        if (target != null)
                        {
                            float randValue = Random.Range(0, 100);

                            if (randValue < critChance)
                            {
                                crit = true;
                                target.TakeDamage(damage);
                                Instantiate(hitPrefab, hit.point, Quaternion.identity);
                                crit = false;
                            }
                            else
                            {
                                target.TakeDamage(damage);
                                Instantiate(hitPrefab, hit.point, Quaternion.identity);
                            }
                        }
                    }
                }
            }
            lastShotTime = Time.time;
        }
    }

    public float GetBaseDamage()
    {
        return baseDamageValue;
    }

    public float SetCritChance(float increase)
    {
        return critChance += increase;
    }

    public float SetCritMultiplier(float increase)
    {
        return critDamageMultiplier += increase;
    }

    public float UpdateGunDamage(float increase)
    {
        return damage += baseDamageValue / 100 * player.GetDamagePercent();
    }

    public bool GetCrit()
    {
        return crit;
    }

    public float GetCritDamageMultiplier()
    {
        return critDamageMultiplier;
    }
}
