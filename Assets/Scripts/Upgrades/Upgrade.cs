using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    private PlayerScript player;
    private Renderer renderer;
    private GameObject cube;

    private float speed = 50f;
    [SerializeField] private int upgradeType;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

        GetComponentsInGameObject();

        switch (upgradeType)
        {
            case 1:
                renderer.material.SetColor("_Color", Color.yellow);
                break;
            case 2:
                renderer.material.SetColor("_Color", Color.blue);
                break;
            case 3:
                renderer.material.SetColor("_Color", Color.yellow);
                break;
            case 4:
                renderer.material.SetColor("_Color", Color.red);
                break;
            default:
                break;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        cube.transform.RotateAround(transform.position, Vector3.up, speed * Time.deltaTime);
        cube.transform.RotateAround(transform.position, Vector3.right, speed * Time.deltaTime);


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

        renderer = cube.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        AddUpgrade();
        Destroy(gameObject);
    }
}
