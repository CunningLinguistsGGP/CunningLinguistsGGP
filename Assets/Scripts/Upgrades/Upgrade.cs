using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    private PlayerScript player;
    private Renderer obj;
    private GameObject cube;
    private HitScanGun gun;

    private float speed = 50f;
    [SerializeField] private int upgradeType;

    private AudioSource audioSource;
    public AudioClip upgradeSpawn;
    public AudioClip upgradeGet;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentsInGameObject();

        audioSource.PlayOneShot(upgradeSpawn);

        switch (upgradeType)
        {
            case 1:
                obj.material.SetColor("_Color", Color.yellow);
                break;
            case 2:
                obj.material.SetColor("_Color", Color.blue);
                break;
            case 3:
                obj.material.SetColor("_Color", Color.yellow);
                break;
            case 4:
                obj.material.SetColor("_Color", Color.green);
                break;
            case 5:
                obj.material.SetColor("_Color", Color.red);
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
                player.SetSpeed(speed + 5f);
                break;
            case 3:
                player.SetDashAmount(1);
                break;
            case 4:
                player.SetHealthPercent(10);
                float healthValue = player.GetBaseMaxHP() / 100 * player.GetHealthPercent();
                player.SetHealth(healthValue);
                break;
            case 5:
                //player.SetDamagePercent(10);
                break;
            default:
                break;
        }
    }

    private void GetComponentsInGameObject()
    {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();

        cube = transform.Find("Cube").gameObject;

        obj = cube.GetComponent<Renderer>();

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(literalTimeWaste());
        }
    }

    IEnumerator literalTimeWaste()
    {
        audioSource.PlayOneShot(upgradeGet);
        AddUpgrade();
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
