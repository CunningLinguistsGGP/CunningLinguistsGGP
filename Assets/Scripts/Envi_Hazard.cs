using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Envi_Hazard : MonoBehaviour
{
    float timer;

    [SerializeField]float damage;

    [SerializeField]float DamagedelayTime = 1f;

    PlayerScript playerscript;

    private void OnTriggerStay(Collider other)
    {
        timer += Time.deltaTime;

        if (timer >= DamagedelayTime)
        {
            playerscript = other.gameObject.GetComponent<PlayerScript>();
            playerscript.currentHealth -= damage;
            playerscript.SetSliderHealth(playerscript.currentHealth);
        }

    }
}
