using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    private PlayerScript player;

    private GameObject cube;

    private float speed = 50f;
    private int upgradeType;
    private int upgradeCount = 4;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

        GetComponentsInGameObject();

        upgradeType = Random.Range(1, upgradeCount);

        Debug.Log(upgradeType);
    }

    // Update is called once per frame
    void Update()
    {
        cube.transform.Rotate(Vector3.up * speed * Time.deltaTime);
        cube.transform.Rotate(Vector3.right * speed * Time.deltaTime);
    }

    private void AddUpgrade()
    {
        switch (upgradeType)
        {
            case 1:
                player.SetDoubleJump(true);
                break;
            case 2:
                float speedValue = player.GetSpeed() / 100 * 5;
                player.SetSpeed(speedValue);
                break;
            case 3:
                player.SetDashAmount(1);
                break;
            case 4:
                float healthValue = player.GetHealth() / 100 * 5;
                player.SetHealth(healthValue);
                break;
            default:
                break;
        }
    }

    private void GetComponentsInGameObject()
    {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();

        cube = transform.Find("Cube").gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        AddUpgrade();
        Destroy(gameObject);
    }
}
