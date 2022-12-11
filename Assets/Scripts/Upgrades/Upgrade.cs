using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Upgrade : MonoBehaviour
{
    private PlayerScript player;
    private Renderer obj;
    private GameObject cube;
    private HitScanGun revolver;
    private HitScanGun shotgun;

    private float speed = 50f;
    private int upgradeType;

    //Audio
    private AudioSource audioSource;
    public AudioClip upgradeSpawn;
    public AudioClip upgradeGet;

    private bool hasBeenRunOver = false;
    private GameObject canvas;
    [SerializeField]private GameObject upgradeText;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentsInGameObject();

        audioSource.PlayOneShot(upgradeSpawn);

        upgradeType = 1;

        switch (upgradeType)
        {
            case 1:
                obj.material.SetColor("_Color", Color.red);
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
        float random;

        switch (upgradeType)
        {
            case 1:
                random = Random.Range(1, 3);

                upgradeText.GetComponent<TextMeshProUGUI>().color = Color.red;

                switch (random)
                {
                    case 1:
                        player.SetDamagePercent(10);
                        upgradeText.GetComponent<TextMeshProUGUI>().text = "10% damage increase";
                        float damageValue = revolver.GetBaseDamage() / 100 * player.GetDamagePercent();
                        revolver.UpdateGunDamage(damageValue);
                        shotgun.UpdateGunDamage(damageValue);
                        StartCoroutine(UpgradeText());
                        break;
                    case 2:
                        revolver.SetCritChance(1);
                        shotgun.SetCritChance(1);
                        upgradeText.GetComponent<TextMeshProUGUI>().text = "1% crit chance increase";
                        StartCoroutine(UpgradeText());
                        break;
                    case 3:
                        revolver.SetCritMultiplier(0.05f);
                        shotgun.SetCritMultiplier(0.05f);
                        upgradeText.GetComponent<TextMeshProUGUI>().text = "5% crit damage increase";
                        StartCoroutine(UpgradeText());
                        break;
                }  
                break;
            case 2:
                upgradeText.GetComponent<TextMeshProUGUI>().color = Color.blue;
                player.SetSpeed(speed + 5f);
                upgradeText.GetComponent<TextMeshProUGUI>().text = "Move Faster!";
                StartCoroutine(UpgradeText());
                break;
            case 3:
                upgradeText.GetComponent<TextMeshProUGUI>().color = Color.yellow;

                if (player.GetDoubleJump() != true && player.GetGrapple() != true)
                {
                    random = Random.Range(1, 3);

                    switch (random)
                    {
                        case 1:
                            player.SetDoubleJump(true);
                            upgradeText.GetComponent<TextMeshProUGUI>().text = "Double Jump Enabled!";
                            StartCoroutine(UpgradeText());
                            break;
                        case 2:
                            player.SetGrapple(true);
                            upgradeText.GetComponent<TextMeshProUGUI>().text = "Grapple Enabled!";
                            StartCoroutine(UpgradeText());
                            break;
                        case 3:
                            player.SetDashAmount(1);
                            upgradeText.GetComponent<TextMeshProUGUI>().text = "1 Extra Dash!";
                            StartCoroutine(UpgradeText());
                            break;
                    }   
                }
                else if(player.GetGrapple() == true)
                {
                    random = Random.Range(1, 2);

                    switch (random)
                    {
                        case 1:
                            player.SetDoubleJump(true);
                            upgradeText.GetComponent<TextMeshProUGUI>().text = "Double Jump Enabled!";
                            StartCoroutine(UpgradeText());
                            break;
                        case 2:
                            player.SetDashAmount(1);
                            upgradeText.GetComponent<TextMeshProUGUI>().text = "Extra Dash!";
                            StartCoroutine(UpgradeText());
                            break;
                    }
                }
                else if(player.GetDoubleJump() == true)
                {
                    random = Random.Range(1, 2);

                    switch (random)
                    {
                        case 1:
                            player.SetGrapple(true);
                            upgradeText.GetComponent<TextMeshProUGUI>().text = "Double Jump Enabled!";
                            StartCoroutine(UpgradeText());
                            break;
                        case 2:
                            player.SetDashAmount(1);
                            upgradeText.GetComponent<TextMeshProUGUI>().text = "Extra Dash!";
                            StartCoroutine(UpgradeText());
                            break;
                    }
                }
                else
                {
                    player.SetDashAmount(1);
                    upgradeText.GetComponent<TextMeshProUGUI>().text = "Extra Dash!";
                    StartCoroutine(UpgradeText());
                }
                break;
            case 4:
                upgradeText.GetComponent<TextMeshProUGUI>().color = Color.green;
                player.SetHealthPercent(10);
                float healthValue = player.GetBaseMaxHP() / 100 * player.GetHealthPercent();
                player.SetHealth(healthValue);
                upgradeText.GetComponent<TextMeshProUGUI>().text = "More Health!";
                StartCoroutine(UpgradeText());
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
        canvas = GameObject.Find("Canvas");
        upgradeText = canvas.transform.Find("Upgrade Text").gameObject;
        revolver = Camera.main.gameObject.transform.Find("Revolver").GetComponent<HitScanGun>();
        shotgun = Camera.main.gameObject.transform.Find("Shotgun").GetComponent<HitScanGun>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player" && !hasBeenRunOver)
        {
            hasBeenRunOver = true;
            obj.material.SetColor("_Color", Color.white);
            StartCoroutine(literalTimeWaste());
        }
    }

    IEnumerator literalTimeWaste()
    {
        audioSource.PlayOneShot(upgradeGet);
        AddUpgrade();
        yield return new WaitForSeconds(1.01f);
        Destroy(gameObject);
    }

    IEnumerator UpgradeText()
    {
        upgradeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        upgradeText.gameObject.SetActive(false);
    }
}
