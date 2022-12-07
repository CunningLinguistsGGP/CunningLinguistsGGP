using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Envi_DMG : MonoBehaviour
{
    [SerializeField]int Damage;

    PlayerScript playerScript;

    private void OnTriggerStay(Collider other)
    {
        StartCoroutine(take_Damage(other.gameObject));
        Debug.Log("damage");
    }

    IEnumerator take_Damage(GameObject other)
    {
        Debug.Log("inside loop");
       // playerScript = other.GetComponent<PlayerScript>();
        //playerScript.currentHealth -= Damage;
       // playerScript.SetSliderHealth(playerScript.currentHealth);

        yield return new WaitForSeconds(2);
    }
}
