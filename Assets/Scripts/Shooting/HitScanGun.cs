using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitScanGun : MonoBehaviour
{
    [SerializeField] private float damage = 10.0f;
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

    [SerializeField] private GameObject DamageTextPrefab;

    private float lastShotTime = 0.0f;

    void Start()
    {
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
                        target.TakeDamage(damage);
                        ShowDamageText(hit.transform, damage);
                        Instantiate(hitPrefab,hit.point,Quaternion.identity);
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
                            target.TakeDamage(damage);
                            ShowDamageText(hit.transform, damage);
                            Instantiate(hitPrefab, hit.point,Quaternion.identity);
                        }

                    }
                }
            }
            lastShotTime = Time.time;
        }
    }

    private void ShowDamageText(Transform enemy, float damage)
    {
        int randX = Random.Range(-1, 1);
        Vector3 textPos = new Vector3(enemy.transform.position.x + randX, enemy.transform.position.y + 1, enemy.transform.position.z);
        DamageTextPrefab.GetComponent<TextMeshPro>().text = damage.ToString();
        Instantiate(DamageTextPrefab, textPos, Camera.main.transform.rotation, enemy);
    }
}
